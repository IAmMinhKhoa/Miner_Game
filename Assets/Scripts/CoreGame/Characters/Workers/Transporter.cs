using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;
using Spine.Unity;
using System;

public class Transporter : BaseWorker
{
    public Counter Counter { get; set; }
    private TextMeshPro numberText;
    [SerializeField] private bool isWorking = false;
    public bool IsWorking => isWorking;
    [SerializeField] private GameObject transporterView;
    [SerializeField] private SkeletonAnimation transporterSkeletonAnimation, cartSkeletonAnimation, headSkeletonAnimation, tailSketonAnimation;
    public SkeletonAnimation CartSkeletonAnimation => cartSkeletonAnimation;
    public SkeletonAnimation HeadSkeletonAnimation => headSkeletonAnimation;
    public SkeletonAnimation BodySkeletonAnimation => transporterSkeletonAnimation;
    public SkeletonAnimation TailSkeletonAnimation => tailSketonAnimation;

    private bool isShowTextNumber = true;

	public override double ProductPerSecond
    {
        get => config.ProductPerSecond * Counter.BoostScale * Counter.EfficiencyBoost * Counter.SpeedBoost;
    }

    public override float WorkingTime
    {
        get => config.WorkingTime / Counter.SpeedBoost;
    }

    public override float MoveTime
    {
        get => config.MoveTime / Counter.SpeedBoost;
    }
	public event Action<bool> OnCashier;
    private void Start()
    {
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(transporterView.transform);
        numberText.transform.localPosition = new Vector3(-0.5f, 0.22f, 0);
        collectTransform = Counter.TransporterLocation;
        depositTransform = Counter.CounterLocation;

        PlayAnimation(WorkerState.Idle, true);
    }

    private void Update()
    {
        if (!isWorking)
        {
            if (Counter.ManagerLocation.Manager != null)
            {
                forceWorking = true;
            }

            if (forceWorking)
            {
                isWorking = true;
                forceWorking = false;
                Move(Counter.TransporterLocation.position);
            }
        }
    }
    private void LateUpdate()
    {
        if (isShowTextNumber == false)
        {
            numberText.text = "";
        }

    }
    public void HideNumberText()
    {
        isShowTextNumber = false;
    }
    protected override async void Collect()
    {
        ChangeGoal();
        double maxCapacity = ProductPerSecond * WorkingTime;
        double amount = Counter.ElevatorDeposit.CalculateAmountPawCanCollect(maxCapacity);

        if (amount == 0)
        {
            await IECollect(amount, 0);
        }
        else
        {
            await IECollect(amount, WorkingTime);
        }
    }
    protected override async UniTask IECollect(double amount, float time)
    {
        PlayTextAnimation(amount);
        PlayAnimation(WorkerState.Working, true);
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
            await IEDeposit(amount, WorkingTime);
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
        PlayAnimation(WorkerState.Idle, true);
    }

    protected override void PlayAnimation(WorkerState state, bool direction)
    {
        switch (state)
        {
            case WorkerState.Idle:
                if (direction)
                {
                    transporterView.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transporterView.transform.localScale = new Vector3(-1, 1, 1);
                }

                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Working:
                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
          
                break;
            case WorkerState.Moving:
				OnCashier?.Invoke(false);
                if (direction)
                {
                    transporterView.transform.localScale = new Vector3(1, 1, 1);
                    cartSkeletonAnimation.AnimationState.SetAnimation(0, "Active2", true);
                }
                else
                {
                    transporterView.transform.localScale = new Vector3(-1, 1, 1);
                    cartSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                }
                numberText.transform.localScale = new Vector3(transporterView.transform.localScale.x, 1f, 1f);
                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Active", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                break;
        }
    }
	
    private async void PlayTextAnimation(double amount, bool reverse = false)
    {
        if (reverse)
        {

			transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
			headSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
			tailSketonAnimation.AnimationState.SetAnimation(0, "Idle", true);
			cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
			double temp = amount;
            double firstValue = amount;
            while (temp > 0)
            {
                await UniTask.Yield();
                temp -= firstValue * Time.deltaTime / WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(0));

			OnCashier?.Invoke(true);
            return;
        }
        else
        {
            double temp = 0;
            double max = amount;
            while (temp < max)
            {
                await UniTask.Yield();
                temp += max * Time.deltaTime / WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(max));
        }
    }
}
