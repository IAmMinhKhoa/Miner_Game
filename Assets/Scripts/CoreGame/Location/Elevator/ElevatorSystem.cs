using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSystem : MonoBehaviour
{
    [SerializeField]
    private Deposit elevatorDeposit;

    [SerializeField]
    private Transform elevatorLocation;

    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform ElevatorLocation => elevatorLocation;

    [SerializeField]
    private double moveTimeScale = 1;
    [SerializeField]
    private double loadSpeedScale = 1;
    public double MoveTimeScale
    {
        get => moveTimeScale;
        set => moveTimeScale = value;
    }

    public double LoadSpeedScale
    {
        get => loadSpeedScale;
        set => loadSpeedScale = value;
    }
}
