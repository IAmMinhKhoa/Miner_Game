using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class BaseUpdrage : MonoBehaviour
{
    public static Action<BaseUpdrage,int> OnUpgrade;

    [Header("Upgrade Info")]
    [SerializeField]
    protected float m_collectPerSecondMutiplier = 2;
    [SerializeField]
    protected float m_collectCapacityMutiplier = 2;
    [SerializeField]
    protected float m_moveSpeedMutiplier = 2;

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
                RunUpdrage();
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

    protected virtual void RunUpdrage()
    {
        
    }

    protected virtual float GetNextUpgradeScale()
    {
        return 0f;
    }
}