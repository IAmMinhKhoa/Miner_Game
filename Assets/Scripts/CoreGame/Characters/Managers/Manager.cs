using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public BaseManagerLocation Location { get; set; }
    public ManagerDataSO Data { get; set; }
    private float _cooldownTime = 0f;
    private float _boostTime = 0f;


    private bool IsAssigned
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
        Location.Manager = this;
    }

    public void UnassignManager()
    {
        Location.Manager = null;
        Location = null;
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
}
