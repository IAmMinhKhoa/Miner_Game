using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterUpgrade : BaseUpgrade
{
    private Counter counter;

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

        if (IsNeedCreateTransporter(CurrentLevel))
        {
            counter.CreateTransporter();
        }
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
        Init(1041.67f, level);
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
