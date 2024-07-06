using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using Newtonsoft.Json;

public class Couter : Patterns.Singleton<Couter>
{
    [Header("Prefab")]
    [SerializeField] private Transporter m_transporterPrefab;
    [SerializeField] private Deposit m_depositPrefab;

    [Header("Location")]
    [SerializeField] private Transform m_couterLocation;
    [SerializeField] private Transform m_depositLocation;
    [SerializeField] private Transform m_transporterLocation;

    [Header("Elevator")]
    [SerializeField] private ElevatorSystem m_elevatorSystem;

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

    public Deposit CouterDeposit { get; set; }

    public Deposit ElevatorDeposit { get; set; }

    public void CreateTransporter()
    {
        GameObject transporterGO = GameData.Instance.InstantiatePrefab(PrefabEnum.Transporter);
        transporterGO.transform.position = m_couterLocation.position;
        transporterGO.transform.SetParent(m_transporterLocation);
        Transporter transporter = transporterGO.GetComponent<Transporter>();
        transporter.Couter = this;

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
    void Start()
    {
        if (!Load())
        {
            CreateDeposit();
            gameObject.GetComponent<CouterUpdrage>().InitValue(1);
            CreateTransporter();
        }
    }

    public void Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "boostScale", m_boostScale },
            {"transporter", _transporters.Count},
            {"level", gameObject.GetComponent<CouterUpdrage>().CurrentLevel},
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
            gameObject.GetComponent<CouterUpdrage>().InitValue(saveData.level);
            ElevatorDeposit = ElevatorSystem.Instance.ElevatorDeposit;

            for (int i = 0; i < saveData.transporter; i++)
            {
                CreateTransporter();
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
    }
}
