using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerLocation : MonoBehaviour
{
    private Manager _manager;
    public Manager Manager => _manager;
    [SerializeField] private int locationType;
    public ManagerLocation LocationType => (ManagerLocation)locationType;
    private bool isBoosting;

    

    public virtual void RunBoost()
    {
        if (_manager == null || _manager.CanActiveBoost() == false)
        {
            return;
        }
        Debug.Log("Mnanager run boost");
        _manager.RunBoost();
    }

    public void SetManager(Manager manager)
    {
        _manager = manager;
        if (manager == null)
        {
            return;
        }
        manager.gameObject.transform.position = transform.position;
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
            };
        }
    }
}
