using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ElevatorSystem : Patterns.Singleton<ElevatorSystem>
{
    public Action<ElevatorController> OnCreateElevatorController;

    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorLocation;
    [SerializeField] private BaseManagerLocation managerLocation;

    public BaseManagerLocation ManagerLocation => managerLocation;
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

    private bool isDone = false;
    public bool IsDone => isDone;
    private float GetManagerBoost(BoostType currentBoostAction)
    {
        return managerLocation.GetManagerBoost(currentBoostAction);
    }

    public void RunBoost()
    {
        managerLocation.RunBoost();
    }

    void Start()
    {
        
    }

    public void InitializeElevators()
    {
        if (!Load())
        {
            CreateElevator();
            gameObject.GetComponent<ElevatorUpgrade>().InitValue(1);
        }
        isDone = true;   
    }

    private void CreateElevator()
    {
        ElevatorController elevatorGO = Instantiate(elevatorPrefab, elevatorLocation.position, Quaternion.identity);
        elevatorGO.elevator = this;
        OnCreateElevatorController?.Invoke(elevatorGO);
    }

    public async UniTaskVoid Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "moveTimeScale", moveTimeScale },
            { "loadSpeedScale", loadSpeedScale },
            { "elevatorDeposit", elevatorDeposit.CurrentPaw },
            {"level", gameObject.GetComponent<ElevatorUpgrade>().CurrentLevel},
            {"managerIndex", managerLocation.Manager != null ? managerLocation.Manager.Index : -1}
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

            if (saveData.managerIndex != -1)
            {
                ManagersController.Instance.ElevatorManagers[saveData.managerIndex].SetupLocation(managerLocation);
            }

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
        public int managerIndex;
    }
}
