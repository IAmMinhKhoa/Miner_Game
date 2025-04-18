using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NOOD;
using UnityEngine;
using UnityEngine.Serialization;

public partial class Shaft : MonoBehaviour
{
    public Action<int> OnUpgrade;

    [Header("Location")]
    [SerializeField] private Transform m_brewLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_brewerLocation;
    [SerializeField] public int shaftIndex;
    [SerializeField] private BaseManagerLocation managerLocation;
    public BaseManagerLocation ManagerLocation => managerLocation;

    public Transform BrewLocation => m_brewLocation;
    public Transform DepositLocation => m_depositLocation;
    public Transform BrewerLocation => m_brewerLocation;

    [Header("Boost")]
    [SerializeField] private double m_scaleCakeValue = 1f;
    [SerializeField] private double m_scaleBakingTime = 1f;
    [SerializeField] private double m_indexBoost = 1f;
    [SerializeField] private double managerBoost = 1f;
    public TransportConfig Config;
    public TransportMachineShaft transportMachine;
    /*[Header("Brewer")]
    private List<Brewer> _brewers = new();
    public List<Brewer> Brewers => _brewers;*/
    public Deposit CurrentDeposit { get; set; }
    [Header("Skin")]
    private ShaftSkin _shaftSkin;

    public ShaftSkin shaftSkin
    {
	    get => _shaftSkin;
	    set
	    {
		    _shaftSkin = value;

	    }

    }

    public int numberBrewer = 1;

    public double ScaleCakeValue
    {
        get { return m_scaleCakeValue; }
        set { m_scaleCakeValue = value; }
    }
    public double ScaleBakingTime
    {
	    get { return m_scaleBakingTime; }
	    set { m_scaleBakingTime = value; }
    }
    public double IndexBoost //Hệ số scale giá trị cua tầng, tầng cân cao thì hệ số làm càng nhanh và nhiều
    {
        get { return m_indexBoost; }
        set { m_indexBoost = value; }
    }

    public double EfficiencyBoost
    {
        get { return IndexBoost * ScaleCakeValue * GetManagerBoost(BoostType.Efficiency); }
    }

    public double GetPureEfficiencyPerSecond()
    {

        return IndexBoost*((ScaleCakeValue*Config.Value)/(ScaleBakingTime*Config.ProductPerSecond));
    }



    public double GetShaftNS()
    {
	    /*Debug.Log("Khoa check NS shaft:\n   "
	              + "Efficiency per second: " + GetPureEfficiencyPerSecond() + ",\n "
	              + "Efficiency boost: " + GetManagerBoost(BoostType.Efficiency) + ",\n "
	              + "Speed boost: " + GetManagerBoost(BoostType.Speed));*/

        return GetPureEfficiencyPerSecond() * GetManagerBoost(BoostType.Efficiency) * GetManagerBoost(BoostType.Speed) * GetGlobalBoost();
    }

    public float GetGlobalBoost()
    {
        return BoostManager.Instance.CurrentBoostValue;
    }

    public float SpeedBoost
    {
        get { return GetManagerBoost(BoostType.Speed); }
    }

    public float CostsBoost
    {
        get { return GetManagerBoost(BoostType.Costs); }
    }


    public void UpdateUI()
    {
        if (TryGetComponent(out ShaftUI shaftUI))
        {
            shaftUI.ChangeSkin(_shaftSkin);
        }
    }

    /*public void CreateBrewer()
    {

        GameObject brewGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Brewer);
        /*float randomX = UnityEngine.Random.Range(m_brewerLocation.position.x, m_brewLocation.position.x);#1#
        float randomX = 0.5f;

        Vector3 spawnPosition = m_brewerLocation.position;
        spawnPosition.x = randomX;
        brewGO.transform.position = spawnPosition;
        brewGO.transform.SetParent(m_brewerLocation);
        brewGO.GetComponent<Brewer>().CurrentShaft = this;

        _brewers.Add(brewGO.GetComponent<Brewer>());
    }*/

    private void CreateDeposit()
    {
        GameObject depositGO = GameData.Instance.InstantiatePrefab(PrefabEnum.ShaftDeposit);
        depositGO.transform.position = m_depositLocation.position;
        Deposit deposit = depositGO.GetComponent<Deposit>();
        deposit.transform.SetParent(transform);
        CurrentDeposit = deposit;
    }

    public float GetManagerBoost(BoostType currentBoostAction)
    {
        return managerLocation.GetManagerBoost(currentBoostAction);
    }

    void Awake()
    {
        CreateDeposit();
        managerLocation.OnChangeManager += SetManager;

    }
    private void OnDestroy()
    {
        managerLocation.OnChangeManager -= SetManager;
    }
    private void SetManager(Manager manager)
    {
        if (manager == null)
        {
            //	Debug.Log("999999");
        }
        AddManagerButtonInteract(false);
    }

    void Start()
    {
        /*for (int i = 0; i <  numberBrewer; i++)
        {
            CreateBrewer();
        }*/
        UpdateUI();
    }

    /*void Update()
    {
        gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(IsTableUsing());
    }*/

    public void SetDepositValue(double value)
    {
        CurrentDeposit.AddPaw(value);
    }


    /*private bool IsTableUsing()
    {
        foreach (var brewer in _brewers)
        {
            if (brewer.isBrewing)
            {
                return true;
            }
        }
        return false;
    }

    public async UniTask AwakeWorker(bool isActiveFromTutorial = false)
    {
		if (isActiveFromTutorial)
		{
			_brewers[0].SetValueParameterIsRequireCallToTutorial();
		}
        foreach (var brewer in _brewers)
        {
            if (!brewer.IsWorking)
            {
                brewer.forceWorking = true;
            }
            await UniTask.Delay(100);
        }
    }*/
    public void AddManagerButtonInteract(bool isShowing)
    {
        if (TryGetComponent(out ShaftUI shaftUI))
        {
            shaftUI.AddManagerButtonInteract(isShowing);
			shaftUI.TurnOffAllEffect();
        }
    }
}


