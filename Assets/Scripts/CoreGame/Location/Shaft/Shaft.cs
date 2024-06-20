using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Location")]
    [SerializeField] private Transform m_brewLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_brewerLocation;

    public Transform BrewLocation => m_brewLocation;
    public Transform DepositLocation => m_depositLocation;
    public Transform BrewerLocation => m_brewerLocation;

    [Header("Boost")]
    [SerializeField] private double m_levelBoost = 1f;
    [SerializeField] private double m_indexBoost = 1f;

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

    private List<Brewer> _brewers = new();
    public List<Brewer> Brewers => _brewers;
    public Deposit CurrentDeposit { get; set; }
    public int ShaftIndex { get; set; }
    public void CreateBrewer()
    {
        GameObject brewGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Brewer);
        brewGO.transform.position = m_brewLocation.position;
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
        CurrentDeposit = deposit;
    }

    void Start()
    {
        CreateDeposit();
        CreateBrewer();
    }
}
