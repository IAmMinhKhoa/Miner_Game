using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ElevatorSystem : Patterns.Singleton<ElevatorSystem>
{
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorLocation;

    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform ElevatorLocation => elevatorLocation;

    [SerializeField] private double moveTimeScale = 1;
    [SerializeField] private double loadSpeedScale = 1;

    [Header("Prefabs")]
    [SerializeField] private ElevatorController elevatorPrefab;
    
    public double MoveTimeScale
    {
        get => moveTimeScale;
        set => moveTimeScale = value;
    }

    public double LoadSpeedScale
    {
        get => loadSpeedScale;
        set => loadSpeedScale = value;
    }

    void Start()
    {
        if (!Load())
        {
            CreateElevator();
            gameObject.GetComponent<ElevatorUpgrade>().InitValue(1);
        }
    }

    private void CreateElevator()
    {
        ElevatorController elevatorGO = Instantiate(elevatorPrefab, elevatorLocation.position, Quaternion.identity);
        elevatorGO.elevator = this;
    }

    public void Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "moveTimeScale", moveTimeScale },
            { "loadSpeedScale", loadSpeedScale },
            { "elevatorDeposit", elevatorDeposit.CurrentPaw },
            {"level", gameObject.GetComponent<ElevatorUpgrade>().CurrentLevel}
        };

        string json = JsonConvert.SerializeObject(saveData);
        PlayerPrefs.SetString("Elevator", json);
    }

    private bool Load()
    {
        if (PlayerPrefs.HasKey("Elevator"))
        {
            string json = PlayerPrefs.GetString("Elevator");
            Data saveData = JsonConvert.DeserializeObject<Data>(json);

            moveTimeScale = saveData.moveTimeScale;
            loadSpeedScale = saveData.loadSpeedScale;
            elevatorDeposit.AddPaw(saveData.elevatorDeposit);
            gameObject.GetComponent<ElevatorUpgrade>().InitValue(saveData.level);

            CreateElevator();
            return true;
        }
        else
        {
            return false;
        }
    }

    class Data
    {
        public double moveTimeScale;
        public double loadSpeedScale;
        public double elevatorDeposit;
        public int level;
    }
}
