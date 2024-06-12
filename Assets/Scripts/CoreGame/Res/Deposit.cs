using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class Deposit : MonoBehaviour
{
    public BigInteger CurrentGold { get; set; }

    void Start()
    {
        CurrentGold = 0;
    }
    public bool CanCollectGold()
    {
        return CurrentGold > 0;
    }

    public BigInteger CollectGold(BaseMiner miner)
    {
        BigInteger currentCapacity = miner.GoldCapacity - miner.CurrentGold;
        return EvaluateAmountCollect(currentCapacity);
    }

    public BigInteger RemoveGold(BigInteger amount)
    {
        if (CurrentGold < amount)
        {
            return 0;
        }
        else
        {
            CurrentGold -= amount;
            return amount;
        }
    }

    public void DepositGold(BigInteger amount)
    {
        CurrentGold += amount;
    }

    public BigInteger EvaluateAmountCollect(BigInteger collectCapacity)
    {
        if (CurrentGold < collectCapacity)
        {
            return CurrentGold;
        }
        else
        {
            return collectCapacity;
        }
    }
}
