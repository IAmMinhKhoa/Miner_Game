using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class BaseUpgrade : MonoBehaviour
{
	public static Action<BaseUpgrade, int> OnUpgrade;
	public static Action OnUpgradeSuccess;

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
			OnUpgradeSuccess?.Invoke();
		}
	}

	protected virtual void UpgradeSuccess()
	{
		PawManager.Instance.RemovePaw(CurrentCost);

	}

	protected virtual void UpdateUpgradeValue()
	{
		level++;
		costScale *= 1 + GetNextUpgradeCostScale(level);
		OnUpgrade?.Invoke(this, CurrentLevel);

	}

	protected virtual void RunUpgrade()
	{

	}
	public virtual float GetNextUpgradeCostScale(int level)
	{
		return 0f;
	}

	private double CalculateScaleBaseOnLevel(int level)
	{
		double scale = 1.00;
		for (int i = 1; i <= level; i++)
		{
			scale *= 1 + GetNextUpgradeCostScale(level);
		}
		return scale;
	}

	public virtual double GetProductionCakeScale(int amoutOfNextLevel) //lấy tỉ lệ giá trị bánh
	{
		return 1f;
	}
	public virtual double GetProductionBakingTime(int amoutOfNextLevel) //lấy tỉ le tốc độ của bánh
	{
		return 1f;
	}
	public virtual double GetSpeedScale(int amoutOfNextLevel)
	{
		return 0f;
	}

	public double GetInitialCost()
	{
		return initialCost;
	}

	public virtual int GetNumberWorkerAtLevel(int level)
	{
		return 0;
	}
}
