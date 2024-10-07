using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ManagersController : Patterns.Singleton<ManagersController>
{
    [SerializeField] private GameObject managerPrefab;
    [SerializeField] private GameObject managerDetailPrefab;
    public List<ManagerDataSO> managerDataSOs => _managerDataSOList;
    private List<ManagerDataSO> _managerDataSOList => MainGameData.managerDataSOList;
    private List<ManagerSpecieDataSO> _managerSpecieDataSOList => MainGameData.managerSpecieDataSOList;
    private List<ManagerTimeDataSO> _managerTimeDataSOList => MainGameData.managerTimeDataSOList;
    private double _ShaftHireCost = 100;
    private double _ElevatorHireCost = 1000;
    private double _CounterHireCost = 1000;

    public double CurrentCost
    {
        get
        {
            return CurrentManagerLocation.LocationType switch
            {
                ManagerLocation.Shaft => _ShaftHireCost,
                ManagerLocation.Elevator => _ElevatorHireCost,
                ManagerLocation.Counter => _CounterHireCost,
                _ => 0
            };
        }
    }

    public List<Manager> ShaftManagers = new List<Manager>();
    public List<Manager> ElevatorManagers = new List<Manager>();
    public List<Manager> CounterManagers = new List<Manager>();

    [SerializeField] private GameObject managerPanel;
    [SerializeField] private GameObject managerDetailPanel;

    public BaseManagerLocation CurrentManagerLocation { get; set; }

    private bool isDone = false;
    public bool IsDone => isDone;



    #region TOOL DEBUG
    [Range(0, 10)]
    [SerializeField] private float _debugSpeedGame = 1;
	#endregion
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	private void OnEnable()
    {
        ManagerLocationUI.OnTabChanged += SetUpCurrentManagerLocation;
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        managerPrefab = Resources.Load<GameObject>("Prefabs/UI/ManagerChooseUI");
        managerDetailPrefab = Resources.Load<GameObject>("Prefabs/UI/ManagerPanelUI");

        managerPanel = Instantiate(managerPrefab, GameUI.Instance.transform);
        managerPanel.SetActive(false);
        managerDetailPanel = Instantiate(managerDetailPrefab, GameUI.Instance.transform);
        managerDetailPanel.SetActive(false);
    }

    public void OpenManagerPanel(BaseManagerLocation location = null)
    {
        // CurrentManagerLocation = location;
        // managerPanel.SetActive(true);

        // if (CurrentManagerLocation.Manager != null)
        // {
        //     managerPanel.GetComponent<ManagerChooseUI>().SetupTab(CurrentManagerLocation.Manager.BoostType,CurrentManagerLocation.LocationType);
        // }
        // else
        // {
        //     managerPanel.GetComponent<ManagerChooseUI>().SetupTab(BoostType.Costs,CurrentManagerLocation.LocationType);
        // }
        CurrentManagerLocation = ShaftManager.Instance.Shafts[0].ManagerLocation;
        managerPanel.SetActive(true);
        managerPanel.GetComponent<ManagerChooseUI>().SetupTab(BoostType.Speed, ManagerLocation.Shaft);
    }

    public void OpenManagerDetailPanel(bool isOpen, Manager data)
    {
        managerDetailPanel.GetComponent<ManagerPanelUI>().SetManager(data);
        managerDetailPanel.SetActive(isOpen);
    }

    private void SetUpCurrentManagerLocation(ManagerLocation location)
    {
        CurrentManagerLocation = location switch
        {
            ManagerLocation.Shaft => ShaftManager.Instance.Shafts[0].ManagerLocation,
            ManagerLocation.Elevator => ElevatorSystem.Instance.ManagerLocation,
            ManagerLocation.Counter => Counter.Instance.ManagerLocation,
            _ => null
        };
    }

    #region ----Manager Control Methods----
    public void RemoveManager(Manager manager)
    {
        if (manager.IsAssigned)
        {
            manager.UnassignManager();
        }

        switch (manager.LocationType)
        {
            case ManagerLocation.Shaft:
                ShaftManagers.Remove(manager);
                break;
            case ManagerLocation.Elevator:
                ElevatorManagers.Remove(manager);
                break;
            case ManagerLocation.Counter:
                CounterManagers.Remove(manager);
                break;
        }
    }

    public void SellManager(Manager manager)
    {
        BoostType type = manager.BoostType;
        if (manager.IsAssigned)
        {
            manager.UnassignManager();
        }
        var sellCost = GetSellCost(manager);
        Debug.Log("Sell Cost: " + sellCost);
        PawManager.Instance.AddPaw(sellCost);
        RemoveManager(manager);
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(type, false);
    }

    private ManagerDataSO GetManagerData(ManagerLocation location, BoostType type, ManagerLevel level)
    {
        var managerData = _managerDataSOList.FirstOrDefault(x => x.managerLocation == location && x.managerLevel == level && x.boostType == type);
        return managerData;
    }

    private ManagerTimeDataSO GetManagerTimeData(ManagerLevel level)
    {
        var managerTimeData = _managerTimeDataSOList.FirstOrDefault(x => x.managerLevel == level);
        return managerTimeData;
    }

    private ManagerSpecieDataSO GetManagerSpecieData(ManagerSpecie specie)
    {
        var managerSpecieData = _managerSpecieDataSOList.FirstOrDefault(x => x.managerSpecie == specie);
        return managerSpecieData;
    }

    private Manager GetNewManagerData(ManagerLocation location)
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        ManagerLevel level = randomValue switch
        {
            < 65 => ManagerLevel.Intern,
            < 90 => ManagerLevel.Junior,
            _ => ManagerLevel.Senior
        };
        /*        BoostType type = UnityEngine.Random.Range(0, 3) switch
                {
                    0 => BoostType.Costs,
                    1 => BoostType.Efficiency,
                    _ => BoostType.Speed
                };*/

        var specieDataList = _managerSpecieDataSOList.ToList();
        var specieData = specieDataList[UnityEngine.Random.Range(0, specieDataList.Count)];

        var managerData = GetManagerData(location, specieData.BoostType, level);
        var timeData = GetManagerTimeData(level);


        Manager manager = new();
        manager.SetManagerData(managerData);
        manager.SetTimeData(timeData);
        manager.SetSpecieData(specieData);

        return manager;
    }

    public Manager CreateManager()
    {
        if (CurrentManagerLocation == null)
        {
            return null;
        }

        Manager manager = GetNewManagerData(CurrentManagerLocation.LocationType);
        switch (manager.LocationType)
        {
            case ManagerLocation.Shaft:
                ShaftManagers.Add(manager);
                break;
            case ManagerLocation.Elevator:
                ElevatorManagers.Add(manager);
                break;
            case ManagerLocation.Counter:
                CounterManagers.Add(manager);
                break;
        }
        PawManager.Instance.RemovePaw(GetHireCost());
        SetNewCost(CurrentManagerLocation.LocationType);
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(manager.BoostType, false);

        return manager;
    }

    public double GetHireCost()
    {
        if (CurrentManagerLocation == null)
        {
            return 0;
        }

        return CurrentManagerLocation.LocationType switch
        {
            ManagerLocation.Shaft => _ShaftHireCost,
            ManagerLocation.Elevator => _ElevatorHireCost,
            ManagerLocation.Counter => _CounterHireCost,
            _ => 0
        };
    }

    public double GetSellCost(Manager manager)
    {
        return manager.LocationType switch
        {
            ManagerLocation.Shaft => _ShaftHireCost * 0.2,
            ManagerLocation.Elevator => _ElevatorHireCost * 0.2,
            ManagerLocation.Counter => _CounterHireCost * 0.2,
        };
    }

    private void SetNewCost(ManagerLocation managerLocation)
    {
        switch (managerLocation)
        {
            case ManagerLocation.Shaft:
                _ShaftHireCost *= 1.8;
                break;
            case ManagerLocation.Elevator:
                _ElevatorHireCost *= 2;
                break;
            case ManagerLocation.Counter:
                _CounterHireCost *= 2;
                break;
        }
    }

    public void BoostAllManager()
    {
        switch (CurrentManagerLocation.LocationType)
        {
            case ManagerLocation.Shaft:
                var shafts = ShaftManager.Instance.Shafts;
                foreach (var shaft in shafts)
                {
                    shaft.ManagerLocation.RunBoost();
                }
                break;
            default:
                CurrentManagerLocation.RunBoost();
                break;
        }
    }

    public void AssignManager(Manager manager, BaseManagerLocation newLocation)
    {
        var managerOldLocation = manager.Location;
        var locationOldManager = newLocation.Manager;

        if (managerOldLocation != null && locationOldManager != null)
        {
            locationOldManager.AssignManager(managerOldLocation);
            manager.AssignManager(newLocation);
        }
        else
        {
            manager.AssignManager(newLocation);
        }
        Debug.Log("AssignManager :" + newLocation);
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(manager.BoostType, false); //reload list manager in inventory

    }

    public void UnassignManager(Manager manager)
    {
        manager.UnassignManager();
        BoostType type = manager.BoostType;
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(type, false); //reload list manager in inventory
        ManagerSelectionShaft.OnReloadManager?.Invoke();//reload scroll selected manager

    }
    public bool MergeManager(Manager firstManager, Manager secondManager)
    {
        bool CanMerge = CanMergeManagers(firstManager, secondManager);
        if (!CanMerge)
        {
            return false;
        }
        return true;
    }



    public void MergeManagerTimes(Manager firstManager, Manager secondManager)
    {
        firstManager.SetCurrentTime(
            Mathf.Max(firstManager.CurrentBoostTime, secondManager.CurrentBoostTime),
            Mathf.Max(firstManager.CurrentCooldownTime, secondManager.CurrentCooldownTime)
        );
    }

    public void UpgradeManager(Manager manager)
    {
        if (manager.Level == ManagerLevel.Executive)
        {
            return;
        }

        var upgradeData = GetManagerData(manager.LocationType, manager.BoostType, manager.Level + 1);
        var timeData = GetManagerTimeData(upgradeData.managerLevel);
        var specieData = GetManagerSpecieData(manager.Specie);

        manager.SetManagerData(upgradeData);
        manager.SetTimeData(timeData);
        manager.SetSpecieData(specieData);

    }


    public bool CanMergeManagers(Manager firstManager, Manager secondManager)
    {
        Debug.Log($"First index: {firstManager.Index} Second index: {secondManager.Index}");

        return firstManager.Level != ManagerLevel.Executive &&
               secondManager.Level != ManagerLevel.Executive &&
               firstManager.LocationType == secondManager.LocationType &&
               firstManager.Level == secondManager.Level &&
               firstManager.Specie == secondManager.Specie;
    }

    #endregion

    #region ----Load Save Region----
    public async UniTaskVoid Save()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();
        List<ManagerSaveData> saveShaftManagers = new List<ManagerSaveData>();
        List<ManagerSaveData> saveElevatorManagers = new List<ManagerSaveData>();
        List<ManagerSaveData> saveCounterManagers = new List<ManagerSaveData>();

        foreach (var manager in ShaftManagers)
        {
            saveShaftManagers.Add(new ManagerSaveData
            {
                location = manager.LocationType,
                boostType = manager.BoostType,
                level = manager.Level,
                specie = manager.Specie,
                currentBoostTime = manager.CurrentBoostTime,
                currentCooldownTime = manager.CurrentCooldownTime
            });
        }

        foreach (var manager in ElevatorManagers)
        {
            saveElevatorManagers.Add(new ManagerSaveData
            {
                location = manager.LocationType,
                boostType = manager.BoostType,
                level = manager.Level,
                specie = manager.Specie,
                currentBoostTime = manager.CurrentBoostTime,
                currentCooldownTime = manager.CurrentCooldownTime
            });
        }

        foreach (var manager in CounterManagers)
        {
            saveCounterManagers.Add(new ManagerSaveData
            {
                location = manager.LocationType,
                boostType = manager.BoostType,
                level = manager.Level,
                specie = manager.Specie,
                currentBoostTime = manager.CurrentBoostTime,
                currentCooldownTime = manager.CurrentCooldownTime
            });
        }

        saveData.Add("ShaftManagers", saveShaftManagers);
        saveData.Add("ElevatorManagers", saveElevatorManagers);
        saveData.Add("CounterManagers", saveCounterManagers);
        saveData.Add("ShaftHireCost", _ShaftHireCost);
        saveData.Add("ElevatorHireCost", _ElevatorHireCost);
        saveData.Add("CounterHireCost", _CounterHireCost);
        string json = JsonConvert.SerializeObject(saveData);

        Debug.Log("save: " + json);
        PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("ManagersController", json);
    }

    public void Load()
    {
        if (PlayFabManager.Data.PlayFabDataManager.Instance.ContainsKey("ManagersController"))
        {

            string json = PlayFabManager.Data.PlayFabDataManager.Instance.GetData("ManagersController");
            Data saveData = JsonConvert.DeserializeObject<Data>(json);
            _ShaftHireCost = saveData.ShaftHireCost;
            _ElevatorHireCost = saveData.ElevatorHireCost;
            _CounterHireCost = saveData.CounterHireCost;

            foreach (var managerData in saveData.ShaftManagers)
            {
                Manager manager = new();
                manager.SetManagerData(GetManagerData(managerData.location, managerData.boostType, managerData.level));
                manager.SetTimeData(GetManagerTimeData(managerData.level));
                manager.SetSpecieData(GetManagerSpecieData(managerData.specie));
                manager.SetCurrentTime(managerData.currentBoostTime, managerData.currentCooldownTime);
                ShaftManagers.Add(manager);
            }

            foreach (var managerData in saveData.ElevatorManagers)
            {
                Manager manager = new();
                manager.SetManagerData(GetManagerData(managerData.location, managerData.boostType, managerData.level));
                manager.SetTimeData(GetManagerTimeData(managerData.level));
                manager.SetSpecieData(GetManagerSpecieData(managerData.specie));
                manager.SetCurrentTime(managerData.currentBoostTime, managerData.currentCooldownTime);
                ElevatorManagers.Add(manager);
            }

            foreach (var managerData in saveData.CounterManagers)
            {
                Manager manager = new();
                manager.SetManagerData(GetManagerData(managerData.location, managerData.boostType, managerData.level));
                manager.SetTimeData(GetManagerTimeData(managerData.level));
                manager.SetSpecieData(GetManagerSpecieData(managerData.specie));
                manager.SetCurrentTime(managerData.currentBoostTime, managerData.currentCooldownTime);
                CounterManagers.Add(manager);
            }
        }
        isDone = true;
    }
    class ManagerSaveData
    {
        public ManagerLocation location;
        public BoostType boostType;
        public ManagerLevel level;
        public ManagerSpecie specie;
        public float currentBoostTime;
        public float currentCooldownTime;
    }

    class Data
    {
        public List<ManagerSaveData> ShaftManagers;
        public List<ManagerSaveData> ElevatorManagers;
        public List<ManagerSaveData> CounterManagers;
        public double ShaftHireCost;
        public double ElevatorHireCost;
        public double CounterHireCost;
    }
    #endregion


    #region ----SUPPORT----
    [Button]
    public void ScaleTime()
    {
        Time.timeScale = _debugSpeedGame;
    }

    #endregion
}
