using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;

public class Counter : Patterns.Singleton<Counter>
{
    public Action<int> OnUpgrade;
    public Action OnUpdateCounterInventoryUI;
    [Header("Location")]
    [SerializeField] private Transform m_counterLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_transporterLocation;
    [SerializeField] private BaseManagerLocation m_managerLocation;
    [SerializeField] private BaseConfig couterConfig;
    public BaseManagerLocation ManagerLocation => m_managerLocation;

    public Transform CounterLocation => m_counterLocation;
    public Transform DepositLocation => m_depositLocation;
    public Transform TransporterLocation => m_transporterLocation;

    [Header("Boost")]
    [SerializeField] private double m_boostScale = 1f;

    public double BoostScale
    {
        get { return m_boostScale; }
        set { m_boostScale = value; }
    }

    public double EfficiencyBoost
    {
        get { return GetManagerBoost(BoostType.Efficiency); }
    }

    public float SpeedBoost
    {
        get { return GetManagerBoost(BoostType.Speed); }
    }

    public float CostsBoost
    {
        get { return GetManagerBoost(BoostType.Costs); }
    }

    private List<Transporter> _transporters = new();
    public List<Transporter> Transporters => _transporters;

    public Deposit CounterDeposit { get; set; }

    public Deposit ElevatorDeposit { get; set; }

    private bool isDone = false;
    public bool IsDone => isDone;

    private CounterSkin _counterSkin;

    public CounterSkin counterSkin
    {
        get => _counterSkin;
        set
        {
            _counterSkin = value;
        }
    }
    protected override void Awake()
    {
        isPersistent = false;
        base.Awake();
		m_managerLocation.OnChangeManager += SetManager;

    }

	private void SetManager(Manager manager)
	{
		if (TryGetComponent<CounterUI>(out CounterUI counterUI))
		{
			counterUI.AddManagerInteract(manager == null);
		}
	}

	public void UpdateUI()
    {
        if (TryGetComponent<CounterUI>(out CounterUI counterUI))
        {
            counterUI.ChangeSkin(counterSkin);
        }
    }
    public void CreateTransporter()
    {
        Debug.Log("Create Transporter");
        GameObject transporterGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Transporter);
        transporterGO.transform.position = m_counterLocation.position;
        transporterGO.transform.SetParent(m_transporterLocation);
        Transporter transporter = transporterGO.GetComponent<Transporter>();
        transporter.Counter = this;
		transporter.OnCashier += CountTransporterInCashier;
        _transporters.Add(transporter);
        if (_transporters.Count > 1)
        {
            UpdateUI();
            transporter.HideNumberText();
        }
    }

    private void CreateDeposit()
    {
        ElevatorDeposit = ElevatorSystem.Instance.ElevatorDeposit;

        GameObject depositGO = GameData.Instance.InstantiatePrefab(PrefabEnum.ShaftDeposit);
        depositGO.transform.position = m_depositLocation.position;
        depositGO.transform.SetParent(m_depositLocation);

        CounterDeposit = depositGO.GetComponent<Deposit>();
    }

    private float GetManagerBoost(BoostType currentBoostAction)
    {
        return m_managerLocation.GetManagerBoost(currentBoostAction);
    }

    public void RunBoost()
    {
        m_managerLocation.RunBoost();
    }

    public double GetPureEfficiencyPerSecond()
    {
        return Transporters.Count * m_boostScale * couterConfig.ProductPerSecond * couterConfig.WorkingTime
        / (2f * (couterConfig.WorkingTime + couterConfig.MoveTime));
    }

    public double GetTotalNS()
    {
		if (ManagerLocation.Manager == null)
		{
			return 0;
		}
        return GetPureEfficiencyPerSecond() * GetManagerBoost(BoostType.Efficiency) * GetManagerBoost(BoostType.Speed);
    }
    void Start()
    {
    }

    public void InitializeCounter()
    {
        if (!Load())
        {
            Debug.Log("Count Init");
            CreateDeposit();
            gameObject.GetComponent<CounterUpgrade>().InitValue(1);
            CreateTransporter();
        }
        isDone = true;
    }

    public void Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "boostScale", m_boostScale },
            {"level", gameObject.GetComponent<CounterUpgrade>().CurrentLevel},
            {"managerIndex", m_managerLocation.Manager != null ? m_managerLocation.Manager.Index : -1}
        };
        if (saveData == null)
        {
            return;
        }
        string json = JsonConvert.SerializeObject(saveData);
        PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("Counter", json);
    }

    private bool Load()
    {
		GetComponent<CounterUI>().UpdateSkeletonData();
		if (PlayFabManager.Data.PlayFabDataManager.Instance.ContainsKey("Counter"))
        {
            string json = PlayFabManager.Data.PlayFabDataManager.Instance.GetData("Counter");
            Data saveData = JsonConvert.DeserializeObject<Data>(json);

            m_boostScale = saveData.boostScale;
            CounterUpgrade upgrader = gameObject.GetComponent<CounterUpgrade>();
            upgrader.InitValue(saveData.level);
            ElevatorDeposit = ElevatorSystem.Instance.ElevatorDeposit;
            int numberWorker = upgrader.GetNumberWorkerAtLevel(saveData.level);
            for (int i = 0; i < numberWorker; i++)
            {
                CreateTransporter();
            }

            if (saveData.managerIndex != -1)
            {
                ManagersController.Instance.CounterManagers[saveData.managerIndex].SetupLocation(m_managerLocation);
            }

            return true;
        }
        return false;
    }

    public async UniTask AwakeWorker()
    {
        foreach (var transporter in _transporters)
        {
            if (!transporter.IsWorking)
            {
                transporter.forceWorking = true;
            }
            await UniTask.Delay(100);
        }
    }

	private int _outCashier;
	private int outCashier
	{
		get => _outCashier;
		set
		{
			_outCashier = value;
			if (_outCashier == _transporters.Count)
			{
				GetComponent<CounterUI>().PlayCollectAnimation(false);
				_outCashier = 0;
			}
		}
	}

	public void CountTransporterInCashier(bool isOnCashier)
	{
	
		if (isOnCashier)
		{
			GetComponent<CounterUI>().PlayCollectAnimation(true);
		}
		else
		{
			outCashier++; 
		}
	}

	class Data
    {
        public double boostScale;
        public double elevatorDeposit;
        public int transporter;
        public int level;
        public int managerIndex;
    }
}
