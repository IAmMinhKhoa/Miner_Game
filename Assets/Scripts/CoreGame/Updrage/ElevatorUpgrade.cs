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
        switch (level)
        {
            case 25:
                return 1.6f;
            case 50:
            case 100:
            case 200:
            case 400:
                return 1.2f;
            default:
                if (level > 1 && level < 25)
                {
                    return 0.32f;
                }
                else if (level <= 2400)
                {
                    return 0.11f;
                }

                return 0;
        }
    }
}
