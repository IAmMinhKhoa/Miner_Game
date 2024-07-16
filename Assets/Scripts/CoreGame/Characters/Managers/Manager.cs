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

        Location = ManagersController.Instance.CurrentManagerLocation;
        Location.SetManager(this);
        splineData.SetActive(true);
    }

    public void UnassignManager()
    {
        Location.SetManager(null);
        Location = null;
        splineData.SetActive(false);
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
        _isBoosting = true;
        currentBoostTime = BoostTime;
        currentCooldownTime = CooldownTime;
        while (currentBoostTime > 0)
        {
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
