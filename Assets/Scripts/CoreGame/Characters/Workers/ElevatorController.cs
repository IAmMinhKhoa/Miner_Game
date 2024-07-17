using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using NOOD;

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

    public double ProductPerSecond
    {
        get => config.ProductPerSecond * elevator.LoadSpeedScale * elevator.EfficiencyBoost * elevator.SpeedBoost;
    }

    protected override float WorkingTime
    {
        get => config.WorkingTime / elevator.SpeedBoost;
    }

    protected override float MoveTime
    {
        get => config.MoveTime * (float)elevator.MoveTimeScale / elevator.SpeedBoost;
    }

    private void Start()
    {
        transform.position = elevator.ElevatorLocation.position;

        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(this.transform.Find("Body").transform);
        numberText.transform.localPosition = new Vector3(0, 1.2f, 0);
    }
    private void Update()
    {
        if (!isWorking)
        {
            isWorking = true;
            MoveToNextShaft();
        }
    }

    private void MoveToNextShaft()
    {
        if (ShaftManager.Instance.Shafts.Count == 0)
        {
            isWorking = false;
            return;
        }

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
            moveBackTime = MoveTime * firstShaftMoveTimeScale;

            _currentDeposit = currentShaft.CurrentDeposit;

            float nextTime = moveBackTime;

            Move(fixPos, nextTime);
            return;
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
            moveBackTime += MoveTime;

            _currentDeposit = currentShaft.CurrentDeposit;
            Move(fixPos, MoveTime);
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

        float collectTime;
        double amount;
        double maxCapacity = WorkingTime * ProductPerSecond;
        //Debug.Log("maxCapacity: " + maxCapacity);

        //if the amount of paw in the deposit is less than the max capacity, collect and move back
        if (CurrentProduct + _currentDeposit.CurrentPaw > maxCapacity)
        {
            amount = maxCapacity - CurrentProduct;
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
        _currentDeposit.RemovePaw(amount);
        MoveToNextShaft();
    }

    protected override async void Deposit()
    {
        await IEDeposit();
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
