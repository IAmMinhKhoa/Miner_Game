using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ElevatorController : BaseWorker
{
    private int _currentShaftIndex = -1;
    private Deposit _currentDeposit;

    [SerializeField]
    private ElevatorSystem elevator;

    [SerializeField]
    private float moveBackTime = 0f;

    [SerializeField]
    private float firstShaftMoveTimeScale = 0.724f;

    [SerializeField] private bool isWorking = false;

    private void Start()
    {
        transform.position = elevator.ElevatorLocation.position;
    }
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (!isWorking)
        //     {
        //         isWorking = true;
        //         MoveToNextShaft();
        //     }
        // }

        if (!isWorking)
            {
                isWorking = true;
                MoveToNextShaft();
            }
    }

    private void MoveToNextShaft()
    {
        if (!IsCollecting)
        {
            Vector2 nextPosition = elevator.ElevatorLocation.position;

            _currentDeposit = elevator.ElevatorDeposit;

            Move(nextPosition, moveBackTime);
            return;
        }
        // Move to the next floor
        _currentShaftIndex++;
        if (_currentShaftIndex == 0)
        {
            Shaft currentShaft = ShaftManager.Instance.Shafts[0];
            Vector2 nextPos = currentShaft.DepositLocation.position;
            Vector2 fixPos = new(transform.position.x, nextPos.y);
            moveBackTime = config.MoveTime * firstShaftMoveTimeScale;

            _currentDeposit = currentShaft.CurrentDeposit;

            Move(fixPos, config.MoveTime * firstShaftMoveTimeScale);
        }
        else if (_currentShaftIndex == ShaftManager.Instance.Shafts.Count)
        {
            ChangeGoal();
            Vector2 nextPosition = elevator.ElevatorLocation.position;

            _currentDeposit = elevator.ElevatorDeposit;

            Move(nextPosition, moveBackTime);
        }
        else
        {
            Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
            Vector2 nextPos = currentShaft.DepositLocation.position;
            Vector2 fixPos = new(transform.position.x, nextPos.y);

            moveBackTime += config.MoveTime;

            _currentDeposit = currentShaft.CurrentDeposit;
            Move(fixPos);
        }
    }

    protected override async void Collect()
    {
        //put in different place
        if (_currentDeposit && !_currentDeposit.CanCollectPaw())
        {
            MoveToNextShaft();
            return;
        }

        float collectTime = 0;
        double amount = 0;
        double maxCapacity = config.ProductPerSecond * config.WorkingTime;

        //if the amount of paw in the deposit is less than the max capacity, collect and move back
        if (CurrentProduct + _currentDeposit.CurrentPaw > maxCapacity)
        {
            amount = maxCapacity - CurrentProduct;
            collectTime = (float)(amount / config.ProductPerSecond);
            ChangeGoal();
        }
        else
        {
            amount = _currentDeposit.CurrentPaw;
            collectTime = (float)(amount / config.ProductPerSecond);

        }

        await IECollect(amount, collectTime);
    }

    protected override async UniTask IECollect(double amount, float collectTime)
    {
        await UniTask.Delay((int)(collectTime * 1000));
        CurrentProduct += amount;
        _currentDeposit.RemovePaw(amount);
        MoveToNextShaft();
    }

    protected override async void Deposit()
    {
        await IEDeposit();
    }

    protected override async UniTask IEDeposit()
    {
        await UniTask.Delay((int)(config.WorkingTime * 1000));
        elevator.ElevatorDeposit.AddPaw(CurrentProduct);
        CurrentProduct = 0;
        _currentShaftIndex = -1;
        ChangeGoal();
        isWorking = false;
    }
}
