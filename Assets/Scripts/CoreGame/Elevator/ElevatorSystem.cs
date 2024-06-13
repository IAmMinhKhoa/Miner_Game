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
}
