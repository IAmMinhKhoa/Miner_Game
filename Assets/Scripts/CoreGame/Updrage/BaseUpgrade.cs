using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade,int> OnUpgrade;

    [Header("Upgrade Cost")]
    [SerializeField] protected double initialCost = 100;
    [SerializeField] private double costScale = 1.00;
    [SerializeField] int level = 1;

    public int CurrentLevel => level;

    protected virtual float CostsBoost => 1.00f;
    public double CurrentCost
    {
        get => initialCost * costScale * CostsBoost;
    }

    protected void Init(double initialCost, int level)
    {
        this.initialCost = initialCost;
        this.level = level;

        this.costScale = CalculateScaleBaseOnLevel(level);
    }

    /*
        * Upgrade the current upgrade
        * @param updateAmount: the amount of upgrade
        UpgradeSuccess: the action when upgrade success (remove paw)
        UpdateUpgradeValue: update the current level and cost -> update the UI
        RunUpgrade: the action when upgrade success
    */
    public virtual void Upgrade(int updateAmount)
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
        PawManager.Instance.RemovePaw(CurrentCost);
        
    }

    protected virtual void UpdateUpgradeValue()
    {
        level++;
        costScale *= 1 + GetNextUpgradeCostScale();
        OnUpgrade?.Invoke(this, CurrentLevel);

    }

    protected virtual void RunUpgrade()
    {
        
    }
    protected virtual float GetNextUpgradeCostScale()
    {
        return 0f;
    }

    private double CalculateScaleBaseOnLevel(int level)
    {
        double scale = 1.00;
        for (int i = 1; i <= level; i++)
        {
            scale *= 1 + GetNextUpgradeCostScale();
        }
        return scale;
    }
}