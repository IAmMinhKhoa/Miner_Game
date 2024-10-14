using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePlace
{
    public Action OnChangeAttribute { get; set; }
    public Transform WorkerLocation { get; }
    public Transform DepositLocation { get; }
    public Transform PickupLocation { get; }
    public BaseManagerLocation ManagerLocation { get; }

    public double EfficiencyBoost
    {
        get;
    }

    public float SpeedBoost
    {
        get;
    }

    public float CostsBoost
    {
        get;
    }
}
