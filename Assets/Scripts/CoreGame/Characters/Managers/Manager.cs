using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public BaseManagerLocation Location { get; set; }
    public ManagerDataSO Data => _data;
    private float _cooldownTime = 0f;
    private float _boostTime = 0f;
    [SerializeField] private GameObject splineData;
    [SerializeField] private ManagerDataSO _data;

    void Awake()
    {
        splineData.SetActive(false);
    }

    public int Index
    {
        get
        {
            return Data.managerLocation switch
            {
                ManagerLocation.Shaft => ManagersController.Instance.ShaftManagers.IndexOf(this),
                ManagerLocation.Elevator => ManagersController.Instance.ElevatorManagers.IndexOf(this),
                ManagerLocation.Counter => ManagersController.Instance.CouterManagers.IndexOf(this),
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
        ActiveBoost();
    }

    private async UniTaskVoid ActiveBoost()
    {
        _boostTime = Data.boostTime;
        _cooldownTime = Data.cooldownTime;
        while (_boostTime > 0)
        {
            _boostTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        await Cooldown();
    }

    private async UniTask Cooldown()
    {
        while (_cooldownTime > 0)
        {
            _cooldownTime -= Time.deltaTime;
            await UniTask.Yield();
        }
    }
    public void SetData(ManagerDataSO data)
    {
        _data = data;
    }
}
