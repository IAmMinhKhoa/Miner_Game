using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class Deposit : MonoBehaviour
{
    public Action<double> OnChangePaw;
    public Action<double> OnChangePawEle;
    public double CurrentPaw { get; private set; }
    
    void Awake()
    {
        CurrentPaw = 0;
    }
	private void Start()
	{

	}
	public bool CanCollectPaw()
    {
        return CurrentPaw > 0;
    }

    public void AddPaw(double amount)
    {
        CurrentPaw += amount;
        OnChangePaw?.Invoke(CurrentPaw);

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
        OnChangePaw?.Invoke(CurrentPaw);
    }
	public void RemovePawEle(double amount)
	{
		if (CurrentPaw <= amount)
		{
			CurrentPaw = 0;
		}
		else
		{
			CurrentPaw -= amount;
		}
		OnChangePawEle?.Invoke(CurrentPaw);
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

        OnChangePaw?.Invoke(CurrentPaw);
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
