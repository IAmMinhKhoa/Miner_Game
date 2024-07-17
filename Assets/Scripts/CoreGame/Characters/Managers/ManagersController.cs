using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using log4net.Core;
using Codice.CM.Common;

public class ManagersController : Patterns.Singleton<ManagersController>
{
    [SerializeField] private GameObject managerPrefab;
    public List<ManagerDataSO> managerDataSOs => _managerDataSOList;
    private List<ManagerDataSO> _managerDataSOList => MainGameData.managerDataSOList;
    private List<ManagerSpecieDataSO> _managerSpecieDataSOList => MainGameData.managerSpecieDataSOList;
    private List<ManagerTimeDataSO> _managerTimeDataSOList => MainGameData.managerTimeDataSOList;

    public List<Manager> ShaftManagers = new List<Manager>();
    public List<Manager> ElevatorManagers = new List<Manager>();
    public List<Manager> CounterManagers = new List<Manager>();

    [SerializeField] private GameObject managerPanel;
    [SerializeField] private GameObject managerDetailPanel;

    public BaseManagerLocation CurrentManagerLocation { get; set; }
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (_mainCamera != null)
        //     {
        //         Vector2 rayOrigin = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //         RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

        //         // Visualize the ray in Scene view
        //         Debug.DrawLine(rayOrigin, rayOrigin + Vector2.up * 100, Color.red, 2f);

        //         if (hit.collider != null)
        //         {
        //             Debug.Log(hit.collider.name);
        //             var managerLocation = hit.transform.GetComponent<BaseManagerLocation>();
        //             if (managerLocation != null)
        //             {
        //                 CurrentManagerLocation = managerLocation;
        //                 var manager = managerLocation.Manager;
        //                 OpenManagerPanel(true);
        //             }
        //         }
        //     }
        // }
    }

    public void OpenManagerPanel(BaseManagerLocation location)
    {
        CurrentManagerLocation = location;
        managerPanel.SetActive(true);
        
        if (CurrentManagerLocation.Manager != null)
        {
            managerPanel.GetComponent<ManagerChooseUI>().SetupTab(CurrentManagerLocation.Manager.BoostType,CurrentManagerLocation.LocationType);
        }
        else
        {
            managerPanel.GetComponent<ManagerChooseUI>().SetupTab(BoostType.Costs,CurrentManagerLocation.LocationType);
        }
    }

    public void OpenManagerDetailPanel(bool isOpen, Manager data)
    {
        managerDetailPanel.GetComponent<ManagerPanelUI>().SetManager(data);
        managerDetailPanel.SetActive(isOpen);
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
        RemoveManager(manager);
        Destroy(manager.gameObject);
        ManagerChooseUI.OnRefreshManagerTab?.Invoke(type);
    }

    private ManagerDataSO GetManagerData(ManagerLocation location, BoostType type, ManagerLevel level)
    {
        var managerData = _managerDataSOList.FirstOrDefault(x => x.managerLocation == location && x.boostType == type && x.managerLevel == level);
        return managerData;
    }

    private ManagerTimeDataSO GetManagerTimeData(ManagerLevel level)
    {
        var managerTimeData = _managerTimeDataSOList.FirstOrDefault(x => x.managerLevel == level);
        return managerTimeData;
    }

    private ManagerSpecieDataSO GetManagerSpecieData(ManagerSpecie specie, ManagerLevel level)
    {
        var managerSpecieData = _managerSpecieDataSOList.FirstOrDefault(x => x.managerSpecie == specie && x.managerLevel == level);
        return managerSpecieData;
    }

    public void UpgradeManager(Manager manager)
    {
        if (manager.Level == ManagerLevel.Executive)
        {
            return;
        }

        var upgradeData = GetManagerData(manager.LocationType, manager.BoostType, manager.Level + 1);
        var timeData = GetManagerTimeData(upgradeData.managerLevel);
        var specieData = GetManagerSpecieData(manager.Specie, upgradeData.managerLevel);
        manager.SetManagerData(upgradeData);
        manager.SetTimeData(timeData);
        manager.SetSpecieData(specieData);
    }

    public void MergeManager(Manager firstManager, Manager secondManager)
    {
        if (!CheckMergeConditions(firstManager, secondManager))
        {
            return;
        }

        firstManager.SetCurrentTime(Mathf.Max(firstManager.CurrentBoostTime, secondManager.CurrentBoostTime),
        Mathf.Max(firstManager.CurrentCooldownTime, secondManager.CurrentCooldownTime));

        RemoveManager(secondManager);
        UpgradeManager(firstManager);
    }

    private bool CheckMergeConditions(Manager firstManager, Manager secondManager)
    {
        if (firstManager.Level == ManagerLevel.Executive || secondManager.Level == ManagerLevel.Executive)
        {
            return false;
        }

        if (firstManager.LocationType != secondManager.LocationType)
        {
            return false;
        }

        if (firstManager.Level != secondManager.Level)
        {
            return false;
        }

        if (firstManager.Specie != secondManager.Specie)
        {
            return false;
        }

        return true;
    }

    public void HireManager()
    {
        var locationType = CurrentManagerLocation.LocationType;

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
        BoostType type = UnityEngine.Random.Range(0, 3) switch
        {
            0 => BoostType.Costs,
            1 => BoostType.Efficiency,
            _ => BoostType.Speed
        };

        var managerData = GetManagerData(location, type, level);
        var timeData = GetManagerTimeData(level);
        var specieDataList = _managerSpecieDataSOList.Where(x => x.managerLevel == level).ToList();
        var specieData = specieDataList[UnityEngine.Random.Range(0, specieDataList.Count)];

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

        return manager;
    }

    #endregion

    #region ----Load Save Region----
    public void SaveData()
    {
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
    #endregion
}
