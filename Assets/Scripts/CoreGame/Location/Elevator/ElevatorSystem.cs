using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ElevatorSystem : Patterns.Singleton<ElevatorSystem>
{
    public Action OnElevatorControllerArrive;
    public Action OnUpdateElevatorInventoryUI;

    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorLocation;
    [SerializeField] private BaseManagerLocation managerLocation;
    [SerializeField] private GameObject lyNuocs;
    [SerializeField] private BaseConfig elevatorCtrlConfig;

    public BaseManagerLocation ManagerLocation => managerLocation;
    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform ElevatorLocation => elevatorLocation;

    private ElevatorController elevatorController;
    public ElevatorController ElevatorController => elevatorController;

    [SerializeField] private double moveTimeScale = 1;
    [SerializeField] private double loadSpeedScale = 1;

    [Header("Prefabs")]
    [SerializeField] private ElevatorController elevatorPrefab;
    public ElevatorController ElevatorPrefabController => elevatorPrefab;

	
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
    private ElevatorSkin _elevatorSkin;
    public ElevatorSkin elevatorSkin
    {
        get => _elevatorSkin;
        set
        {
            _elevatorSkin = value;
        }
    }

    public double GetPureProductionInCycle()
    {
        return elevatorCtrlConfig.ProductPerSecond * elevatorCtrlConfig.WorkingTime * loadSpeedScale;
    }

    public double GetPureMoveTime()
    {
        return elevatorCtrlConfig.MoveTime * MoveTimeScale;
    }

    public double GetPureLoadTime()
    {
        return elevatorCtrlConfig.WorkingTime;
    }

    public double GetMoveTimeInCycle()
    {
        return GetPureMoveTime() * (ShaftManager.Instance.Shafts.Count - 1) * 2 + GetPureMoveTime() * 0.724f * 2f + GetPureLoadTime() * 2;
    }

    public double GetTempMoveTimeInCycle(int index)
    {
        return GetPureMoveTime() * index * 2 + GetPureMoveTime() * 0.724f * 2f + GetPureLoadTime() * 2;
    }

    private bool isDone = false;

    public bool IsDone => isDone;
    public float GetManagerBoost(BoostType currentBoostAction)
    {
        return managerLocation.GetManagerBoost(currentBoostAction);
    }

    public void RunBoost()
    {
        managerLocation.RunBoost();
    }
    public void UpdateUI()
    {
        if (TryGetComponent(out ElevatorUI elevatorUI))
        {
            elevatorUI.ChangeSkin(_elevatorSkin);
        }
        else
        {
            Debug.Log("Faild to update background sprite");
        }
    }
    protected override void Awake()
    {
        isPersistent = false;
        base.Awake();
        managerLocation.OnChangeManager += SetManager;
    }

    private void SetManager(Manager manager)
    {
        if (TryGetComponent(out ElevatorUI elevatorUI))
        {
            elevatorUI.AddManagerInteract(manager == null);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }
    void Start()
    {
        elevatorDeposit.OnChangePaw += ElevatorDeposit_OnChangePawHandler;
    }

    private void ElevatorDeposit_OnChangePawHandler(double value)
    {
        if (value > 0)
        {
            lyNuocs.SetActive(true);
        }
        else
        {
            lyNuocs.SetActive(false);
        }
    }

    public void InitializeElevators()
    {
        if (!Load())
        {
            gameObject.GetComponent<ElevatorUpgrade>().InitValue(1);
            CreateElevator();
        }

        isDone = true;
    }

    private void CreateElevator()
    {
        ElevatorController elevatorCtrl = Instantiate(elevatorPrefab, elevatorLocation.position, Quaternion.identity);
        this.elevatorController = elevatorCtrl;
        elevatorCtrl.elevator = this;
        elevatorCtrl.OnArriveTarget += ElevatorController_OnArriveTargetHandler;
    }

    private void ElevatorController_OnArriveTargetHandler(Vector3 vector)
    {
        if (vector == elevatorLocation.position)
        {
            // Arrive and start deposit paw
            OnElevatorControllerArrive?.Invoke();
        }
    }

    public double GetTotalNS()
    {
        return GetPureProductionInCycle() / GetMoveTimeInCycle() * GetManagerBoost(BoostType.Speed) * GetManagerBoost(BoostType.Efficiency);
    }

    public double GetTotalNSVersion2()
    {
        //rule if elvevator not have manager -> NSPaw =0
        if (managerLocation.Manager == null)
        {
            return 0;
        }

        int index = 0;
        int maxIndex = ShaftManager.Instance.Shafts.Count - 1;
        double loadCapacity = GetPureProductionInCycle() * GetManagerBoost(BoostType.Efficiency);
        for (int i = 0; i <= maxIndex; i++)
        {
            double moveTime = GetTempMoveTimeInCycle(i) / GetManagerBoost(BoostType.Speed);

            double q = 0d;
            for (int j = 0; j <= i; j++)
            {
                var shaft = ShaftManager.Instance.Shafts[j];
                q += shaft.GetShaftNS() * moveTime;
            }
            //Debug.Log("q: " + q + "index: " + i + "loadCapacity: " + loadCapacity);

            index = i;
            if (q >= loadCapacity)
            {
                break;
            }
        }

        return GetPureProductionInCycle() / GetTempMoveTimeInCycle(index) * GetManagerBoost(BoostType.Speed) * GetManagerBoost(BoostType.Efficiency) * GetGlobalBoost();
    }

    public float GetGlobalBoost()
    {
        return BoostManager.Instance.CurrentBoostValue;
    }

    public void Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { "moveTimeScale", moveTimeScale },
            { "loadSpeedScale", loadSpeedScale },
            { "elevatorDeposit", elevatorDeposit.CurrentPaw },
            {"level", gameObject.GetComponent<ElevatorUpgrade>().CurrentLevel},
            {"managerIndex", managerLocation.Manager != null ? managerLocation.Manager.Index : -1}
        };
        if (saveData == null)
        {
            return;
        }
        string json = JsonConvert.SerializeObject(saveData);
        PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("Elevator", json);
    }

    private bool Load()
    {
        GetComponent<ElevatorUI>().UpdateSkeletonData();
        if (PlayFabManager.Data.PlayFabDataManager.Instance.ContainsKey("ShaftManager"))
        {
            string json = PlayFabManager.Data.PlayFabDataManager.Instance.GetData("Elevator");
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

    public void AwakeWorker(bool isTriggerByTutorial = false)
    {
		if(isTriggerByTutorial)
		{
			elevatorController.SetValueParameterIsRequireCallToTutorial();
		}
        if (!elevatorController.IsWorking)
        {
            elevatorController.forceWorking = true;
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
