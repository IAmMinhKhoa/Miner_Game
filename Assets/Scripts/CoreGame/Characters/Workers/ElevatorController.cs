using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using NOOD;
using NOOD.Sound;

public class ElevatorController : BaseWorker
{
    private int _currentShaftIndex = -1;
    private Deposit _currentDeposit;
    private TextMeshPro numberText;
    [SerializeField] public ElevatorSystem elevator;
    [SerializeField] private float moveBackTime = 0f;
    [SerializeField] private float firstShaftMoveTimeScale = 0.724f;
    [SerializeField] private bool isWorking = false;
    private double checkWorkingTime = 0;
    public bool IsWorking => isWorking;
    public double MaxCapacity { get; private set; }
    public override double ProductPerSecond
    {
        get => config.ProductPerSecond * elevator.LoadSpeedScale * elevator.EfficiencyBoost * elevator.SpeedBoost * elevator.GetGlobalBoost();
    }

    public override float WorkingTime
    {
        get => config.WorkingTime / elevator.SpeedBoost;
    }

    public override float MoveTime
    {
        get => config.MoveTime * (float)elevator.MoveTimeScale / elevator.SpeedBoost;
    }


    private void Start()
    {
        transform.position = elevator.ElevatorLocation.position;
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(this.transform);
        numberText.transform.localPosition = new Vector3(0, 0.4f, 0);
        collectTransform = this.transform;

        PlayAnimation(WorkerState.Idle, true);
    }
    private void Update()
    {
        if (!isWorking)
        {
            if (elevator.ManagerLocation.Manager != null)
            {
                forceWorking = true;
            }

            if (forceWorking)
            {
				isWorking = true;
                forceWorking = false;
                MoveToNextShaft();
            }
        }
    }


    private void MoveToNextShaft()
    {
        if (ShaftManager.Instance.Shafts.Count == 0)
        {
            isWorking = false;
            return;
        }

        Debug.Log("MoveToNextShaft");
        if (!IsCollecting)
        {
            Debug.Log("IsCollecting false");
            Vector2 nextPosition = elevator.ElevatorLocation.position;

            _currentDeposit = elevator.ElevatorDeposit;
            depositTransform = _currentDeposit.transform;

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
            moveBackTime = MoveTime * firstShaftMoveTimeScale;

            _currentDeposit = currentShaft.CurrentDeposit;
            depositTransform = _currentDeposit.transform;

            float nextTime = moveBackTime;

            Move(fixPos, nextTime);
            return;
        }
        else if (_currentShaftIndex == ShaftManager.Instance.Shafts.Count)
        {
            ChangeGoal();
            Vector2 nextPosition = elevator.ElevatorLocation.position;

            _currentDeposit = elevator.ElevatorDeposit;
            depositTransform = elevator.ElevatorLocation;

            Move(nextPosition, moveBackTime);
        }
        else
        {
            Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
            Vector2 nextPos = currentShaft.DepositLocation.position;
            Vector2 fixPos = new(transform.position.x, nextPos.y);
            moveBackTime += MoveTime;

            _currentDeposit = currentShaft.CurrentDeposit;
            depositTransform = _currentDeposit.transform;
            Move(fixPos, MoveTime);
        }
    }

    protected override async void Collect()
    {
        Debug.Log("Collect");
        //put in different place
        if (_currentDeposit && !_currentDeposit.CanCollectPaw())
        {
            MoveToNextShaft();
            return;
        }

        float collectTime;
        double amount;
        MaxCapacity = WorkingTime * ProductPerSecond;
        //Debug.Log("maxCapacity: " + maxCapacity);

        //if the amount of paw in the deposit is less than the max capacity, collect and move back
        if (CurrentProduct + _currentDeposit.CurrentPaw > MaxCapacity)
        {
            amount = MaxCapacity - CurrentProduct;
            collectTime = (float)(amount / ProductPerSecond);
            if (collectTime > 4f)
            {
                Debug.LogError("Collect time is too long!");
            }
            ChangeGoal();
        }
        else
        {
            amount = _currentDeposit.CurrentPaw;
            collectTime = (float)(amount / ProductPerSecond);
            if (collectTime > 4f)
            {
                Debug.LogError("Collect time is too long!");
            }

        }

        await IECollect(amount, collectTime);
    }

    protected override async UniTask IECollect(double amount, float collectTime)
    {
        PlayTextAnimation(amount);
        await UniTask.Delay((int)(collectTime * 1000));
        checkWorkingTime += collectTime;
        CurrentProduct += amount;
        _currentDeposit.RemovePawEle(amount);
        MoveToNextShaft();
        OnChangePawDone?.Invoke(CurrentProduct);
    }

    protected override async void Deposit()
    {
        Debug.Log("Deposit");
        await IEDeposit();
        OnChangePawDone?.Invoke(CurrentProduct);
    }

    protected override async UniTask IEDeposit()
    {
        PlayTextAnimation(CurrentProduct, true);
        await UniTask.Delay((int)(WorkingTime * 1000));
        elevator.ElevatorDeposit.AddPaw(CurrentProduct);
        CurrentProduct = 0;
        Debug.Log("Deposit" + moveBackTime);

        Debug.Log("checkWorkingTime: " + checkWorkingTime);
        checkWorkingTime = 0;

        _currentShaftIndex = -1;
        ChangeGoal();
        isWorking = false;
        PlayAnimation(WorkerState.Idle, false);
    }

    private async void PlayTextAnimation(double amount, bool reverse = false)
    {
        if (reverse)
        {
            double temp = CurrentProduct;
            double firstValue = CurrentProduct;
            double lastValue = 0;
            while (temp > lastValue)
            {
                await UniTask.Yield();
                temp -= firstValue * Time.deltaTime / WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(lastValue));
            return;
        }
        else
        {
            double temp = CurrentProduct;
            double max = temp + amount;
            while (temp < max)
            {
                await UniTask.Yield();
                temp += ProductPerSecond * Time.deltaTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(max));
        }
    }
}
