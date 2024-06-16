using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;

public class Transporter : BaseWorker
{
    public Couter Couter { get; set; }

    private TextMeshPro numberText;

    [SerializeField] private bool isWorking = false;

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
            Move(Couter.TransporterLocation.position);
        }
    }

    protected override async void Collect()
    {
        ChangeGoal();
        double maxCapacity = config.ProductPerSecond * config.WorkingTime * Couter.BoostScale;
        double amount = Couter.ElevatorDeposit.CaculateAmountPawCanCollect(maxCapacity);

        Couter.ElevatorDeposit.RemovePaw(amount);
        CurrentProduct += amount;
        await IECollect(amount ,config.WorkingTime);
    }

    protected override async void Deposit()
    {
        Debug.Log("Transporter Deposit");
        double amount = CurrentProduct;
        Couter.CouterDeposit.AddPaw(amount);
        CurrentProduct = 0;
        await IEDeposit(amount, 0);
    }

    protected override async UniTask IECollect(double amount, float time)
    {
        PlayTextAnimation(amount);
        await UniTask.Delay((int)config.WorkingTime * 1000);
        Move(Couter.CouterLocation.position);
    }

    protected override async UniTask IEDeposit(double amount = 0, float time = 0)
    {
        PlayTextAnimation(amount, true);
        await UniTask.Delay((int)config.WorkingTime * 1000);
        ChangeGoal();
        isWorking = false;
    }

    private async void PlayTextAnimation(double amount, bool reverse = false)
    {
        if (reverse)
        {
            double temp = amount;
            double firstValue = amount;
            double lastValue = 0;
            while (temp > lastValue)
            {
                await UniTask.Yield();
                temp -= firstValue * Time.deltaTime / config.WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(lastValue));
            return;
        }
        else
        {
            double temp = CurrentProduct;
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
