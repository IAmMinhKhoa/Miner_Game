using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;
using Spine.Unity;

public class Brewer : BaseWorker
{
    [SerializeField] private GameObject spineData;
    private SkeletonAnimation skeletonAnimation;

    public Shaft CurrentShaft { get; set; }
    private TextMeshPro numberText;
    private Vector3 targetPos;
    private bool isArrive;

    [SerializeField] private bool isWorking = false;
    public double ProductPerSecond
    {
        get => config.ProductPerSecond * CurrentShaft.LevelBoost * CurrentShaft.IndexBoost;
    }

    void Start()
    {
        skeletonAnimation = spineData.GetComponent<SkeletonAnimation>();
        numberText = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadText).GetComponent<TextMeshPro>();
        numberText.transform.SetParent(this.transform);
        numberText.transform.localPosition = new Vector3(0, 1.2f, 0);
    }

    private void Update()
    {
        if (!isWorking)
        {
            isWorking = true;
            // PlayAnimation();
            Move(CurrentShaft.BrewLocation.position);
            //targetPos = CurrentShaft.BrewLocation.position;       
        }
        // if(Vector3.Distance(this.transform.position, targetPos) < 0.1f)
        // {
        //     if(isArrive == false)
        //     {
        //         if(IsCollecting)
        //         {
        //             Collect();
        //         }
        //         else
        //         {
        //             Deposit();
        //         }
        //         isArrive = true;
        //     }
        // }
        // else
        // {
        //     isArrive = false;
        //     Vector3 dir = (targetPos - transform.position).normalized;
        //     this.transform.position += dir * config.MoveTime * Time.deltaTime;
        // }

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
    }

    protected override async UniTask IECollect()
    {
        PlayTextAnimation();
        // skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        // CurrentShaft.gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(true);
        await UniTask.Delay((int)config.WorkingTime * 1000);
        CurrentProduct = ProductPerSecond * config.WorkingTime;
        // PlayAnimation();
        // CurrentShaft.gameObject.GetComponent<ShaftUI>().PlayCollectAnimation(false);
        Move(CurrentShaft.BrewerLocation.position);
    }

    private async void PlayTextAnimation()
    {
        double max = ProductPerSecond * config.WorkingTime;
        double temp = 0;
        while (temp < max)
        {
            await UniTask.Yield();
            temp += ProductPerSecond * Time.deltaTime;
            numberText.SetText(Currency.DisplayCurrency(temp));
        }
        numberText.SetText(Currency.DisplayCurrency(max));
    }

    private void PlayAnimation()
    {
        if (IsCollecting)
        {
            skeletonAnimation.skeleton.ScaleX = -1;
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }
        else
        {
            skeletonAnimation.skeleton.ScaleX = 1;
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
        }
    }

    protected override void PlayAnimation(WorkerState state, bool direction)
    {
        switch (state)
        {
            case WorkerState.Idle:
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Working:
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case WorkerState.Moving:
                if (direction)
                {
                    skeletonAnimation.skeleton.ScaleX = 1;
                }
                else
                {
                    skeletonAnimation.skeleton.ScaleX = -1;
                }
                skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
                break;
        }
    }
}
