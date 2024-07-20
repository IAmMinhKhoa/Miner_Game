using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Manager
{
    public BaseManagerLocation Location { get; set; }
    //public ManagerDataSO Data => _data;
    //[SerializeField] private GameObject splineData;
    [SerializeField] private ManagerDataSO _data;
    [SerializeField] private ManagerSpecieDataSO _specieData;
    [SerializeField] private ManagerTimeDataSO _timeData;

    private string viewPath = "Prefabs/Character/ManagerView";
    private ManagerView _view;

    public Sprite Icon => _specieData.icon;
    public ManagerSpecie Specie => _specieData.managerSpecie;
    public string Name => _data.managerName;
    public ManagerLocation LocationType => _data.managerLocation;
    public ManagerLevel Level => _data.managerLevel;
    public BoostType BoostType => _data.boostType;
    public float BoostValue => _data.boostValue;
    public float BoostTime => _timeData.boostTime;
    public float CooldownTime => _timeData.cooldownTime;


    private bool _isBoosting = false;
    public bool IsBoosting => _isBoosting;
    private float currentBoostTime;
    private float currentCooldownTime;

    public float CurrentBoostTime => currentBoostTime;
    public float CurrentCooldownTime => currentCooldownTime;

    public void SetSpecieData(ManagerSpecieDataSO data)
    {
        _specieData = data;
    }

    public void SetTimeData(ManagerTimeDataSO data)
    {
        _timeData = data;
    }

    public void SetManagerData(ManagerDataSO data)
    {
        _data = data;
    }

    public void SetCurrentTime(float boostTime, float cooldownTime)
    {
        currentBoostTime = boostTime;
        currentCooldownTime = cooldownTime;
    }

    public int Index
    {
        get
        {
            return LocationType switch
            {
                ManagerLocation.Shaft => ManagersController.Instance.ShaftManagers.IndexOf(this),
                ManagerLocation.Elevator => ManagersController.Instance.ElevatorManagers.IndexOf(this),
                ManagerLocation.Counter => ManagersController.Instance.CounterManagers.IndexOf(this),
                _ => 0,
            };
        }
    }


    public bool IsAssigned
    {
        get
        {
            return Location != null;
        }
    }

    public void AssignManager()
    {
        if (IsAssigned)
        {
            UnassignManager();
        }
        var currentManager = ManagersController.Instance.CurrentManagerLocation.Manager;
        currentManager?.UnassignManager();

        Location = ManagersController.Instance.CurrentManagerLocation;
        Location.SetManager(this);
        _view = GameObject.Instantiate(Resources.Load<ManagerView>(viewPath), ManagersController.Instance.transform);
        _view.transform.position = Location.transform.position;
    }

    public void SetupLocation(BaseManagerLocation location)
    {
        Location = location;
        Location.SetManager(this);
        Debug.Log("Setup Location: " + location.LocationType + "/" + this.IsAssigned);
        _view = GameObject.Instantiate(Resources.Load<ManagerView>(viewPath), ManagersController.Instance.transform);
        _view.transform.position = Location.transform.position;
    }

    public void UnassignManager()
    {
        Location.SetManager(null);
        Location = null;
        GameObject.Destroy(_view.gameObject);
    }

    public void SwapManager()
    {
        
    }

    public void RunBoost()
    {
        if (_isBoosting)
        {
           return;
        }
        Debug.Log("Run Boost");
        currentBoostTime = BoostTime * 60;
        currentCooldownTime = CooldownTime * 60;
        ActiveBoost();
    }
    private async UniTaskVoid ActiveBoost()
    {
        _isBoosting = true;        
        while (currentBoostTime > 0)
        {
            Debug.Log("Boosting:" + LocationType + "/" + currentBoostTime);
            currentBoostTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        _isBoosting = false;
        await Cooldown();
    }
    private async UniTask Cooldown()
    {
        while (currentCooldownTime > 0)
        {
            Debug.Log("Boosting:" + LocationType + "/" + currentCooldownTime);
            currentCooldownTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        _isBoosting = false;
    }
    public void SetData(ManagerDataSO data)
    {
        _data = data;
    }

    public bool CanActiveBoost()
    {
        return currentCooldownTime <= 0;
    }

    public void RunTimer()
    {
        if (currentBoostTime > 0)
        {
            ActiveBoost();
        }
        else if (currentCooldownTime > 0)
        {
            Cooldown();
        }
    }
}
