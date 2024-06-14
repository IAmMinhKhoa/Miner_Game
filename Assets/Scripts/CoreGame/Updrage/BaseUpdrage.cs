using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade,int> OnUpgrade;

    [Header("Upgrade Cost")]
    [SerializeField]
    private BigInteger initialCost = 10;
    [SerializeField]
    private BigInteger m_costMultiplier = 2;

    public int CurrentLevel { get; set; }
    public BigInteger CurrentCost { get; private set; }

    public virtual void Upgrade(BigInteger updateAmount)
    {
        if (updateAmount > 0)
        {
            for (int i = 0; i < updateAmount; i++)
            {
                UpgradeSuccess();
                UpdateUpgradeValue();
                RunUpgrade();
            }
        }
    }

    protected virtual void UpgradeSuccess()
    {
        GoldManager.Instance.RemoveGold(CurrentCost);
        CurrentLevel++;
        Debug.Log("Upgrade success" + CurrentLevel);
        OnUpgrade?.Invoke(this, CurrentLevel);
    }

    protected virtual void UpdateUpgradeValue()
    {

    }

    protected virtual void RunUpgrade()
    {
        
    }

    protected virtual float GetNextUpgradeScale()
    {
        return 0f;
    }
}