using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ElevatorUpgrade : BaseUpgrade
{
    private ElevatorSystem elevatorSystem;

    private void Start()
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
    protected override float GetNextUpgradeCostScale()
    {
        return CurrentLevel switch
        {
            <= 2400 => 0.2f,
            _ => 0f
        };
    }

    public void InitValue(int level)
    {
        Init(1041.67f, level);
    }
}
