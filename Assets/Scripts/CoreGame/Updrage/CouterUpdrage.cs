using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CounterUpgrade : BaseUpgrade
{
    private Counter counter;
	private Dictionary<int, int> milestoneLevels = new Dictionary<int, int>
	{
		{ 10, 4 },
		{ 25, 4 },
		{ 50, 4 },
		{ 100, 4 },
		{ 200, 4 },
		{ 300, 4 },
		{ 400, 4 },
		{ 500, 4 },
		{ 600, 4 },
		{ 700, 4 },
		{ 800, 4 },
		{ 900, 4 },
		{ 1000, 4 },
		{ 1100, 4 },
		{ 1200, 4 },
		{ 1300, 4 },
		{ 1400, 4 },
		{ 1500, 4 },
		{ 1600, 4 },
		{ 1700, 4 },
		{ 1800, 4 },
		{ 1900, 4 },
		{ 2000, 4 },
		{ 2100, 4 },
		{ 2200, 4 },
		{ 2300, 4 },
		{ 2400, 4 },
	};
	protected override float CostsBoost
    {
        get => counter.CostsBoost;
    }
    private void Awake()
    {
        counter = GetComponent<Counter>();
    }
    protected override void RunUpgrade()
    {
		float nextScale = GetNextExtractionSpeedScale(CurrentLevel);
		counter.BoostScale *= 1 + nextScale;
		if (milestoneLevels.TryGetValue(CurrentLevel, out int superMoney))
		{
			SuperMoneyManager.Instance.AddMoney(superMoney);
		}
		if (IsNeedCreateTransporter(CurrentLevel))
		{
			counter.CreateTransporter();
		}
		counter.OnUpgrade?.Invoke(CurrentLevel);
	}

    private float GetNextExtractionSpeedScale(int level)
    {
        return level switch
        {
            < 2 => 0,
            25 => 1.6f,
            50 or 100 or 200 or 400 => 1.2f,
            _ => level switch
            {
                < 25 => 0.3f,
                <= 2400 => 0.1f,
                _ => 0
            }
        };
    }

    private bool IsNeedCreateTransporter(int level)
    {
        return level == 10 || level == 100 || level == 300 || level == 500;
    }
    public override float GetNextUpgradeCostScale(int level)
    {
        return level switch
        {
            < 2400 => 0.2f,
            _ => 0f
        };
    }

    public void InitValue(int level)
	{
		//Init(1041.67f, level);
		Init(1041.67f, level);
		counter.OnUpgrade?.Invoke(level);
	}

    public override int GetNumberWorkerAtLevel(int level)
    {
        int initWorker = 1;
        return level switch
        {
            >= 500 => initWorker + 4,
            >= 300 => initWorker + 3,
            >= 100 => initWorker + 2,
            >= 10 => initWorker + 1,
            _ => initWorker
        };
    }

    public override double GetProductionScale(int amoutOfNextLevel)
    {
        if (amoutOfNextLevel <= 0)
        {
            return 1d;
        }

        double scale = 1.00;
        for (int i = 1; i <= amoutOfNextLevel; i++)
        {
            scale *= 1 + GetNextExtractionSpeedScale(CurrentLevel + i);
        }

        return scale;
    }
}
