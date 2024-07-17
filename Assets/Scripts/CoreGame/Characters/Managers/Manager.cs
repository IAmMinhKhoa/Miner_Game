using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public BaseManagerLocation Location { get; set; }
    //public ManagerDataSO Data => _data;
    [SerializeField] private GameObject splineData;
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
    public float currentBoostTime;
    public float currentCooldownTime;



    void Awake()
    {
        splineData.SetActive(false);
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
        //splineData.SetActive(true);
        _view = Instantiate(Resources.Load<ManagerView>(viewPath), ManagersController.Instance.transform);
        _view.transform.position = Location.transform.position;
    }

    public void UnassignManager()
    {
        Location.SetManager(null);
        Location = null;
        splineData.SetActive(false);
        Destroy(_view.gameObject);
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
        ActiveBoost();
    }

    private bool CheckMergeConditions(Manager otherManager)
    {
        if (otherManager.Level == ManagerLevel.Executive)
        {
            return false;
        }

        if (otherManager.LocationType != LocationType)
        {
            return false;
        }

        if (otherManager.Specie != Specie)
        {
            return false;
        }

        if (otherManager.Level != Level)
        {
            return false;
        }
        //Select new SO there

        return true;
    }

    public void Merge(Manager otherManager)
    {
        if (!CheckMergeConditions(otherManager))
        {
            return;
        }

        ManagersController.Instance.RemoveManager(otherManager);
        Destroy(otherManager.gameObject);
    }

    private async UniTaskVoid ActiveBoost()
    {
        //Debug.Log("Active Boost");
        _isBoosting = true;
        //Debug.Log("Boosting:" + _isBoosting);
        currentBoostTime = BoostTime * 60;
        currentCooldownTime = CooldownTime * 60;
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
}
