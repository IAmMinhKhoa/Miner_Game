using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseManagerLocation : MonoBehaviour
{
    public event Action<Manager> OnChangeManager;
    private Manager _manager;
    public Manager Manager => _manager;
    [SerializeField] private int locationType;
    [SerializeField] private IBasePlace _basePlace;
    public IBasePlace BasePlace => _basePlace;

    public ManagerLocation LocationType => (ManagerLocation)locationType;
    private bool isBoosting;

    void Start()
    {
        GameObject parent = transform.parent.gameObject;

        if (locationType == 0)
        {
            _basePlace = parent.GetComponent<Shaft>();
        }
    }

    public virtual void RunBoost()
    {
        if (_manager == null)
        {
            return;
        }

        if (!_manager.CanActiveBoost())
        {
            return;
        }

        _manager.RunBoost();
    }

    public void SetManager(Manager manager)
    {
        _manager = manager;
        OnChangeManager?.Invoke(_manager);
    }
    public float GetManagerBoost(BoostType currentBoostAction)
    {
        if (Manager == null
        || Manager.BoostType != currentBoostAction
        || !Manager.IsBoosting)
        {
            return 1f;
        }
        else
        {
            return Manager.BoostType switch
            {
                BoostType.Costs => 1f - Manager.BoostValue,
                BoostType.Efficiency => Manager.BoostValue,
                BoostType.Speed => Manager.BoostValue,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
