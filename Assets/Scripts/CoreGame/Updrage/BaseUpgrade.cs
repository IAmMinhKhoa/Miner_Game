using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade,int> OnUpgrade;

    [Header("Upgrade Cost")]
    [SerializeField] private double initialCost = 100;
    [SerializeField] private double currentCost = 100;

    public int CurrentLevel { get; set; }
    public double CurrentCost { get => currentCost; set => currentCost = value; }

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
        CurrentLevel++;
        CurrentCost *= 1 + GetNextUpgradeCostScale();
        OnUpgrade?.Invoke(this, CurrentLevel);

    }

    protected virtual void RunUpgrade()
    {
        
    }

    protected virtual float GetNextUpgradeCostScale()
    {
        return 0f;
    }
}