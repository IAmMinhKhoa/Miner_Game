using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couter : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Transporter m_transporterPrefab;
    [SerializeField] private Deposit m_depositPrefab;

    [Header("Location")]
    [SerializeField] private Transform m_couterLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_transporterLocation;

    public Transform CouterLocation => m_couterLocation;
    public Transform DepositLocation => m_depositLocation;
    public Transform TransporterLocation => m_transporterLocation;

    [Header("Boost")]
    [SerializeField] private double m_boostScale = 1f;

    public double BoostScale
    {
        get { return m_boostScale; }
        set { m_boostScale = value; }
    }

    private List<Transporter> _transporters = new();
    public List<Transporter> Transporters => _transporters;

    public Deposit CurrentDeposit { get; set; }
    public double CurrentProduct { get; set; }

    public void CreateTransporter()
    {
        Transporter transporter = Instantiate(m_transporterPrefab, m_transporterLocation.position, Quaternion.identity);
        transporter.transform.SetParent(m_transporterLocation);
        transporter.Couter = this;

        _transporters.Add(transporter);
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
        CreateTransporter();
    }
}
