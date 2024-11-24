using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;
using Spine.Unity;

public class Brewer : BaseWorker
{
    [SerializeField] private GameObject brewerView;

    [SerializeField] private SkeletonAnimation brewerSkeletonAnimation, cartSkeletonAnimation, headSkeletonAnimation, tailSketonAnimation;


    public Shaft CurrentShaft { get; set; }
    public SkeletonAnimation CartSkeletonAnimation => cartSkeletonAnimation;
    public SkeletonAnimation HeadSkeletonAnimation => headSkeletonAnimation;
    public SkeletonAnimation BodySkeletonAnimation => brewerSkeletonAnimation;
    public SkeletonAnimation TailSkeletonAnimation => tailSketonAnimation;
    [SerializeField] private TextMeshPro numberText;

    [SerializeField] private bool isWorking = false;
    public bool IsWorking => isWorking;

    public bool isBrewing = false;
    public override double ProductPerSecond
    {
        get => config.ProductPerSecond * CurrentShaft.EfficiencyBoost * CurrentShaft.SpeedBoost * CurrentShaft.GetGlobalBoost();
    }

    public override float WorkingTime
    {
        get => config.WorkingTime / CurrentShaft.SpeedBoost;
    }

    public override float MoveTime
    {
        get => config.MoveTime / CurrentShaft.SpeedBoost;
    }

    void Start()
    {
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(brewerView.transform);
        numberText.transform.localPosition = new Vector3(-0.75f, 0.3f, 0);
        collectTransform = CurrentShaft.BrewLocation;
        depositTransform = CurrentShaft.BrewerLocation;

        PlayAnimation(WorkerState.Idle, false);
    }


    private void Update()
    {
        if (!isWorking)
        {
            if (CurrentShaft.ManagerLocation.Manager != null)
            {
                forceWorking = true;
            }

            if (forceWorking)
            {
                isWorking = true;
                forceWorking = false;
                Move(CurrentShaft.BrewLocation.position);
            }
        }
    }
    private void LateUpdate()
    {
        numberText.text = "";
    }

    protected override async void Collect()
    {
        base.Collect();
        await IECollect();
    }

    protected override void Deposit()
    {
        base.Deposit();
        CurrentShaft.CurrentDeposit.AddPaw(CurrentProduct);
        CurrentProduct = 0;
        numberText.text = "0";

        isWorking = false;
        PlayAnimation(WorkerState.Idle, false);
    }

    protected override async UniTask IECollect()
    {
        PlayTextAnimation();
        // skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        // CurrentShaft.gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(true);
        await UniTask.Delay((int)WorkingTime * 1000);
        CurrentProduct = ProductPerSecond * WorkingTime;
        //Debug.Log("Collect: " + ProductPerSecond + " Time:" + WorkingTime);
        // PlayAnimation();
        // CurrentShaft.gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(false);
        Move(CurrentShaft.BrewerLocation.position);
    }

    private async void PlayTextAnimation()
    {
        double max = ProductPerSecond * WorkingTime;
        double temp = 0;
        while (temp < max)
        {
            await UniTask.Yield();
            temp += ProductPerSecond * Time.deltaTime;
            numberText.SetText(Currency.DisplayCurrency(temp));
        }
        numberText.SetText(Currency.DisplayCurrency(max));
    }

    protected override void PlayAnimation(WorkerState state, bool direction)
    {
        switch (state)
        {
            case WorkerState.Idle:
                if (direction)
                {
                    brewerView.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    brewerView.transform.localScale = new Vector3(-1, 1, 1);
                }

                brewerSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Working:
                brewerSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                cartSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                isBrewing = true;
                break;
            case WorkerState.Moving:
                isBrewing = false;
                if (direction)
                {
                    brewerView.transform.localScale = new Vector3(1, 1, 1);
                    cartSkeletonAnimation.AnimationState.SetAnimation(0, "Active2", true);
                }
                else
                {
                    brewerView.transform.localScale = new Vector3(-1, 1, 1);
                    cartSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                }
                numberText.transform.localScale = new Vector3(brewerView.transform.localScale.x, 1f, 1f);
                brewerSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Active", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                break;
        }
    }
}
