using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class Deposit : MonoBehaviour
{
    public double CurrentPaw { get; private set; }
    
    void Start()
    {
        CurrentPaw = 0;
    }

    public bool CanCollectPaw()
    {
        return CurrentPaw > 0;
    }

    public void AddPaw(double amount)
    {
        CurrentPaw += amount;
    }

    public void RemovePaw(double amount)
    {
        if (CurrentPaw <= amount)
        {
            CurrentPaw = 0;
        }
        else
        {
            CurrentPaw -= amount;
        }
    }
}
