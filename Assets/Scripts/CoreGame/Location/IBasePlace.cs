using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePlace
{
    public Transform WorkerLocation {get;}
    public Transform DepositLocation {get;}
    public Transform PickupLocation {get;}
    public BaseManagerLocation ManagerLocation {get;}

    public virtual double EfficiencyBoost
    {
        get { return GetManagerBoost(BoostType.Efficiency); }
    }

    public virtual float SpeedBoost
    {
        get { return GetManagerBoost(BoostType.Speed); }
    }

    public virtual float CostsBoost
    {
        get { return GetManagerBoost(BoostType.Costs); }
    }

    private float GetManagerBoost(BoostType currentBoostAction)
    {
        return ManagerLocation.GetManagerBoost(currentBoostAction);
    }
}
