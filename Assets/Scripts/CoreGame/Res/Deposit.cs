using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class Deposit : MonoBehaviour
{
    public double CurrentPaw { get; private set; }
    
    void Awake()
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

    public double TakePawn(double amount)
    {
        double amountToTake = amount;
        if (CurrentPaw < amount)
        {
            amountToTake = CurrentPaw;
            CurrentPaw = 0;
        }
        else
        {
            CurrentPaw -= amount;
        }

        return amountToTake;
    }

    public double CalculateAmountPawCanCollect(double capacity)
    {
        if (capacity - CurrentPaw >= 0)
        {
            double paw = CurrentPaw;
            return paw;
        }
        else
        {
            return capacity;
        }
    }
}
