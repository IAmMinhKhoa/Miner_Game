using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;

public class Transporter : BaseWorker
{
    public Counter Counter { get; set; }
    private TextMeshPro numberText;
    [SerializeField] private bool isWorking = false;
    public double ProductPerSecond
    {
        get => config.ProductPerSecond * Counter.BoostScale * Counter.EfficiencyBoost * Counter.SpeedBoost;
    }

    protected override float WorkingTime
    {
        get => config.WorkingTime / Counter.SpeedBoost;
    }

    private void Start()
    {
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(this.transform);
        numberText.transform.localPosition = new Vector3(0, 1.2f, 0);
    }

    private void Update()
    {
        if (!isWorking)
        {
            isWorking = true;
            Move(Counter.TransporterLocation.position);
        }
    }

    protected override async void Collect()
    {
        ChangeGoal();
        double maxCapacity = ProductPerSecond * config.WorkingTime;
        double amount = Counter.ElevatorDeposit.CalculateAmountPawCanCollect(maxCapacity);

        if (amount == 0)
        {
            await IECollect(amount , 0);
        }
        else
        {
            await IECollect(amount ,config.WorkingTime);
        }
    }
    protected override async UniTask IECollect(double amount, float time)
    {
        PlayTextAnimation(amount);
        await UniTask.Delay((int)time * 1000);
        CurrentProduct += Counter.ElevatorDeposit.TakePawn(amount);
        numberText.SetText(Currency.DisplayCurrency(CurrentProduct));
        Move(Counter.CounterLocation.position);
    }
        protected override async void Deposit()
    {
        double amount = CurrentProduct;
        if (amount == 0)
        {
            await IEDeposit(amount, 0);
        }
        else
        {
            await IEDeposit(amount, config.WorkingTime);
        }
    }

    protected override async UniTask IEDeposit(double amount = 0, float time = 0)
    {
        PlayTextAnimation(amount, true);
        await UniTask.Delay((int)time * 1000);
        PawManager.Instance.AddPaw(amount);
        CurrentProduct = 0;
        ChangeGoal();
        isWorking = false;
    }

    private async void PlayTextAnimation(double amount, bool reverse = false)
    {
        if (reverse)
        {
            double temp = amount;
            double firstValue = amount;
            while (temp > 0)
            {
                await UniTask.Yield();
                temp -= firstValue * Time.deltaTime / config.WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(0));
            return;
        }
        else
        {
            double temp = 0;
            double max = amount;
            while (temp < max)
            {
                await UniTask.Yield();
                temp += max * Time.deltaTime / config.WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(max));
        }
    }
}
