using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class Shaft : MonoBehaviour
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
    [SerializeField] private double m_levelBoost = 1f;
    [SerializeField] private double m_indexBoost = 1f;
    [SerializeField] private double managerBoost = 1f;
    [SerializeField] private BaseConfig m_config;

    public int numberBrewer = 1;

    public double LevelBoost
    {
        get { return m_levelBoost; }
        set { m_levelBoost = value; }
    }

    public double IndexBoost
    {
        get { return m_indexBoost; }
        set { m_indexBoost = value; }
    }

    public double EfficiencyBoost
    {
        get { return IndexBoost * LevelBoost * GetManagerBoost(BoostType.Efficiency); }
    }

    public double GetPureEfficiencyPerSecond()
    {
        return IndexBoost * LevelBoost * m_config.ProductPerSecond * m_config.WorkingTime
        / (m_config.WorkingTime + 2d * m_config.MoveTime);
    }

    public double GetShaftNS()
    {
        return GetPureEfficiencyPerSecond() * GetManagerBoost(BoostType.Efficiency) * GetManagerBoost(BoostType.Speed);
    }

    public float SpeedBoost
    {
        get { return GetManagerBoost(BoostType.Speed); }
    }

    public float CostsBoost
    {
        get { return GetManagerBoost(BoostType.Costs); }
    }

    private List<Brewer> _brewers = new();
    public List<Brewer> Brewers => _brewers;
    public Deposit CurrentDeposit { get; set; }
    [Header("Skin")]
    private ShaftSkin _shaftSkin;

    public ShaftSkin shaftSkin
    {
        get => _shaftSkin;
        set
        {
            Debug.Log($"Set data to shaft {shaftIndex}");
            _shaftSkin = value;
            ShaftUI shaftUI;
            TryGetComponent<ShaftUI>(out shaftUI);
            if (shaftUI != null)
            {
                //  shaftUI.OnChangeSkin?.Invoke(value);
                shaftUI.ChangeSkin(value);
            }
        }
    }


    public void CreateBrewer()
    {
        GameObject brewGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Brewer);
        float randomX = UnityEngine.Random.Range(m_brewerLocation.position.x, m_brewLocation.position.x);
        Vector3 spawnPosition = m_brewerLocation.position;
        spawnPosition.x = randomX;
        brewGO.transform.position = spawnPosition;
        brewGO.transform.SetParent(m_brewerLocation);
        brewGO.GetComponent<Brewer>().CurrentShaft = this;

        _brewers.Add(brewGO.GetComponent<Brewer>());
    }

    private void CreateDeposit()
    {
        GameObject depositGO = GameData.Instance.InstantiatePrefab(PrefabEnum.ShaftDeposit);
        depositGO.transform.position = m_depositLocation.position;
        Deposit deposit = depositGO.GetComponent<Deposit>();
        deposit.transform.SetParent(transform);
        Debug.Log("create:" + depositGO);
        CurrentDeposit = deposit;
    }

    public float GetManagerBoost(BoostType currentBoostAction)
    {
        return managerLocation.GetManagerBoost(currentBoostAction);
    }

    void Awake()
    {
        CreateDeposit();
    }

    void Start()
    {
        for (int i = 0; i < numberBrewer; i++)
        {
            CreateBrewer();
        }
    }

    void Update()
    {
        gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(IsTableUsing());
    }

    public void SetDepositValue(double value)
    {
        CurrentDeposit.AddPaw(value);
    }

    public void UpgradeTable()
    {

    }

    private bool IsTableUsing()
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
}
public class ShaftSkin
{
    public int index;
    public string idBackGround;
    public string idWaitTable;
    public string idMilkCup;

    public ShaftSkin(int index, string idBackGround = "1", string idWaitTable = "1", string idMilkCup = "1")
    {
        this.index = index;
        this.idBackGround = idBackGround;
        this.idWaitTable = idWaitTable;
        this.idMilkCup = idMilkCup;
    }
}
