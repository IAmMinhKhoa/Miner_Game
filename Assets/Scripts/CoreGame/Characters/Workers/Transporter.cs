using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;
using Spine.Unity;

public class Transporter : BaseWorker
{
    public Counter Counter { get; set; }
    private TextMeshPro numberText;
    [SerializeField] private bool isWorking = false;
    [SerializeField] private GameObject transporterView;
    [SerializeField] private SkeletonAnimation transporterSkeletonAnimation, cartSkeletonAnimation, headSkeletonAnimation;
	public SkeletonAnimation CartSkeletonAnimation => cartSkeletonAnimation;
	public SkeletonAnimation HeadSkeletonAnimation => headSkeletonAnimation;
	public SkeletonAnimation BodySkeletonAnimation => transporterSkeletonAnimation;
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

    private void Start()
    {
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(transporterView.transform);
        numberText.transform.localPosition = new Vector3(-0.5f, 0.22f, 0);
        collectTransform = Counter.TransporterLocation;
        depositTransform = Counter.CounterLocation;
    }

    private void Update()
    {
        if (!isWorking)
        {
            isWorking = true;
            Move(Counter.TransporterLocation.position);
        }
    }
	private void LateUpdate()
	{
		if(isShowTextNumber == false)
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
            await IECollect(amount , 0);
        }
        else
        {
            await IECollect(amount ,WorkingTime);
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
        PlayAnimation(WorkerState.Idle, false);
        await UniTask.Delay((int)time * 1000);
        PawManager.Instance.AddPaw(amount);
        CurrentProduct = 0;
        ChangeGoal();
        isWorking = false;
    }

    protected override void PlayAnimation(WorkerState state, bool direction)
    {
        switch (state)
        {
            case WorkerState.Idle:
                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Working:
                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Moving:
                if (direction)
                {
                    transporterView.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transporterView.transform.localScale = new Vector3(-1, 1, 1);
                }
				numberText.transform.localScale = new Vector3(transporterView.transform.localScale.x, 1f, 1f);
				cartSkeletonAnimation.AnimationState.SetAnimation(0,(CurrentProduct == 0)? "Active" : "Active2", true);
                transporterSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                break;
        }
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
                temp -= firstValue * Time.deltaTime / WorkingTime;
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
                temp += max * Time.deltaTime / WorkingTime;
                numberText.SetText(Currency.DisplayCurrency(temp));
            }
            numberText.SetText(Currency.DisplayCurrency(max));
        }
    }
}
