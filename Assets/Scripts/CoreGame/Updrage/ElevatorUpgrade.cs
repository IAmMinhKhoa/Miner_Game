using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ElevatorUpgrade : BaseUpgrade
{
    private ElevatorSystem elevatorSystem;

    protected override float CostsBoost
    {
        get => elevatorSystem.CostsBoost;
    }

    private void Awake()
    {
        elevatorSystem = GetComponent<ElevatorSystem>();
        //Init(1041.67f, 1);
    }

    protected override void RunUpgrade()
    {
        double nextScale = GetMoveTimeScale(CurrentLevel);
        elevatorSystem.MoveTimeScale *= nextScale;
        elevatorSystem.LoadSpeedScale *= 1 + GetNextLoadingSpeedScale(CurrentLevel);
    }

    private double GetMoveTimeScale(int level)
    {
        return 1f - 15 / (0.15f * (level - 1) + 5) / 100;
    }

    private float GetNextLoadingSpeedScale(int level)
    {
        return level switch
        {
            < 2 => 0,
            25 => 1.6f,
            50 or 100 or 200 or 400 => 1.2f,
            _ => level switch
            {
                < 25 => 0.32f,
                <= 2400 => 0.11f,
                _ => 0
            }
        };
    }
    public override float GetNextUpgradeCostScale(int level)
    {
        return level switch
        {
            <= 2400 => 0.2f,
            _ => 0f
        };
    }

    public void InitValue(int level)
    {
        Init(1041.67f, level);
    }

    public override double GetScaleBuff(int amoutOfNextLevel)
    {
        if (amoutOfNextLevel <= 0)
        {
            return 1d;
        }

        double scale = 1.00;
        for (int i = 1; i <= amoutOfNextLevel; i++)
        {
            scale *= 1 + GetNextLoadingSpeedScale(CurrentLevel + i);
        }

        return scale;
    }
}
