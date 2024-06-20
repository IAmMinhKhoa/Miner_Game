using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    private ElevatorSystem elevatorSystem;

    private void Start()
    {
        elevatorSystem = GetComponent<ElevatorSystem>();
        CurrentLevel = 1;
    }

    protected override void RunUpgrade()
    {
        double nextScale = GeNextMoveTimeScale(CurrentLevel);
        elevatorSystem.MoveTimeScale *= nextScale;
        elevatorSystem.LoadSpeedScale *= 1 + GetNextLoadingSpeedScale(CurrentLevel);
    }

    private double GeNextMoveTimeScale(int level)
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
}
