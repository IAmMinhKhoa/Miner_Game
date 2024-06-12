using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Brewer m_brewerPrefab;
    [SerializeField] private Deposit m_depositPrefab;

   [Header("Location")]
    [SerializeField] private Transform m_brewLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_brewerLocation;

    public Transform BrewLocation => m_brewLocation;
    public Transform DepositLocation => m_depositLocation;
    public Transform BrewerLocation => m_brewerLocation;

    private List<Brewer> _brewers = new();
    public List<Brewer> Brewers => _brewers;

    public Deposit CurrentDeposit { get; set; }

    public void CreateBrewer()
    {
        Brewer brewer = Instantiate(m_brewerPrefab, m_brewerLocation.position, Quaternion.identity);
        brewer.transform.SetParent(m_brewerLocation);
        brewer.CurrentShaft = this;

        if (_brewers.Count > 0)
        {
            brewer.ProductPerSecond = _brewers[0].ProductPerSecond;
            brewer.WorkingTime = _brewers[0].WorkingTime;
        }

        _brewers.Add(brewer);
    }

    private void CreateDeposit()
    {
        Deposit deposit = Instantiate(m_depositPrefab, m_depositLocation.position, Quaternion.identity);
        deposit.transform.SetParent(transform);
        CurrentDeposit = deposit;
    }

    void Start()
    {
        CreateDeposit();
        CreateBrewer();
    }
}
