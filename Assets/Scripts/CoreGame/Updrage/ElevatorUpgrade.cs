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
        elevatorSystem.LoadSpeedScale *= 1.32f;
    }

    private double GeNextMoveTimeScale(int level)
    {
        return 1f - 15 / (0.15f * (level - 1) + 5) / 100;
    }
}
