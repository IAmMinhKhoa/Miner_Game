using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public class Counter : Patterns.Singleton<Counter>
{
    [Header("Prefab")]
    [SerializeField] private Transporter m_transporterPrefab;
    [SerializeField] private Deposit m_depositPrefab;

    [Header("Location")]
    [SerializeField] private Transform m_couterLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_transporterLocation;
    [SerializeField] private BaseManagerLocation m_managerLocation;
    public BaseManagerLocation ManagerLocation => m_managerLocation;

    [Header("Elevator")]
    [SerializeField] private ElevatorSystem m_elevatorSystem;

    public Transform CounterLocation => m_couterLocation;
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

    public Deposit CouterDeposit { get; set; }

    public Deposit ElevatorDeposit { get; set; }

    private bool isDone = false;
    public bool IsDone => isDone;

    public void CreateTransporter()
    {
        GameObject transporterGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Transporter);
        transporterGO.transform.position = m_couterLocation.position;
        transporterGO.transform.SetParent(m_transporterLocation);
        Transporter transporter = transporterGO.GetComponent<Transporter>();
        transporter.Counter = this;

        _transporters.Add(transporter);
    }

    private void CreateDeposit()
    {
        ElevatorDeposit = m_elevatorSystem.ElevatorDeposit;

        GameObject depositGO = GameData.Instance.InstantiatePrefab(PrefabEnum.ShaftDeposit);
        depositGO.transform.position = m_depositLocation.position;
        depositGO.transform.SetParent(m_depositLocation);

        CouterDeposit = depositGO.GetComponent<Deposit>();
    }

    private float GetManagerBoost(BoostType currentBoostAction)
    {
        return m_managerLocation.GetManagerBoost(currentBoostAction);
    }

    public void RunBoost()
    {
        m_managerLocation.RunBoost();
    }
    void Start()
    {
    }

    public void InitializeCounter()
    {
        if (!Load())
        {
            CreateDeposit();
            gameObject.GetComponent<CounterUpgrade>().InitValue(1);
            CreateTransporter();
        }
        isDone = true;
    }

    public async UniTaskVoid Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "boostScale", m_boostScale },
            {"transporter", _transporters.Count},
            {"level", gameObject.GetComponent<CounterUpgrade>().CurrentLevel},
            {"managerIndex", m_managerLocation.Manager != null ? m_managerLocation.Manager.Index : -1}
        };

        string json = JsonConvert.SerializeObject(saveData);
        Debug.Log("save couter: " + json);
        PlayerPrefs.SetString("Couter", json);
    }

    private bool Load()
    {
        if (PlayerPrefs.HasKey("Couter"))
        {
            string json = PlayerPrefs.GetString("Couter");
            Data saveData = JsonConvert.DeserializeObject<Data>(json);

            m_boostScale = saveData.boostScale;
            gameObject.GetComponent<CounterUpgrade>().InitValue(saveData.level);
            ElevatorDeposit = ElevatorSystem.Instance.ElevatorDeposit;

            for (int i = 0; i < saveData.transporter; i++)
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

    class Data
    {
        public double boostScale;
        public double elevatorDeposit;
        public int transporter;
        public int level;
        public int managerIndex;
    }
}
