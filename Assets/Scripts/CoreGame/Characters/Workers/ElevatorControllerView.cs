using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NOOD;
using Sirenix.Utilities;
using Spine.Unity;

using UnityEngine;

public class ElevatorControllerView : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _frontElevator, _backElevator, _elevatorStaff, _refrigerator, _refrigeratorDoor;

    [SerializeField]private GameObject[] _lyNuocs;
    private ElevatorController _elevatorController;

    void Awake()
    {
        _elevatorController = GetComponent<ElevatorController>();
        _lyNuocs.ForEach(x => x.SetActive(false));
    }

    void Start()
    {
        _elevatorController.OnMoveToTarget += ElevatorController_OnMoveToTargetHandler;
        _elevatorController.OnArriveTarget += ElevatorControllerController_OnArriveHandler;
        _elevatorController.OnChangePawDone += ElevatorController_OnChangePawDoneHandler;
    }
    void OnDisable()
    {
        _elevatorController.OnMoveToTarget -= ElevatorController_OnMoveToTargetHandler;
        _elevatorController.OnArriveTarget -= ElevatorControllerController_OnArriveHandler;
        _elevatorController.OnChangePawDone -= ElevatorController_OnChangePawDoneHandler;
    }

    private void ElevatorController_OnChangePawDoneHandler(double amount)
    {
        int percent = (int)((amount / _elevatorController.MaxCapacity * 100)/10);
        for(int i = 0; i < _lyNuocs.Length; i++)
        {
            if (i == 0 && amount > 0) 
            {
                _lyNuocs[i].SetActive(true);
                continue;
            }
            _lyNuocs[i].SetActive(i < percent);
        }
    }

    private void ElevatorControllerController_OnArriveHandler(Vector3 vector)
    {
        _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Idle", true);
        _backElevator.AnimationState.SetAnimation(0, "Thangmay - Idle", true);
        if(vector == ElevatorSystem.Instance.ElevatorLocation.position)
        {
            OpenRefrigerator(true);
        }
    }

    private void ElevatorController_OnMoveToTargetHandler(Vector3 vector)
    {
        OpenRefrigerator(false);
        if(this.transform.position.y > vector.y)
        {
            // Move down
            _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Down", true);
            _backElevator.AnimationState.SetAnimation(0, "Thangmay - Down", true);
        }
        else
        {
            // Move up
            _frontElevator.AnimationState.SetAnimation(0, "Thangmay - Up", true);
            _backElevator.AnimationState.SetAnimation(0, "Thangmay - Up", true);
        }
        _elevatorStaff.AnimationState.SetAnimation(0, "Idle", true);
    }

    private void OpenRefrigerator(bool isOpen)
    {
        if(isOpen)
        {
            _refrigeratorDoor.AnimationState.SetAnimation(0, "Cuatulanh - Open", false);
        }
        else
        {
            if(_refrigeratorDoor.AnimationName == "Cuatulanh - Open")
            {
                _refrigeratorDoor.AnimationState.SetAnimation(0, "Cuatulanh - Idle", true);
            }
        }
    }
}
