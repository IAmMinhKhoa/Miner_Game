using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpdrage : BaseUpdrage
{
    private ElevatorSystem elevatorSystem;

    private void Start()
    {
        elevatorSystem = GetComponent<ElevatorSystem>();
        CurrentLevel = 1;
    }

    protected override void RunUpdrage()
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
