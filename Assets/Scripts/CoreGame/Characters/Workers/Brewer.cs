using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using NOOD;
using Spine.Unity;
using UnityEngine.UI;

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
	    [SerializeField] private Image imageLoading; //parent loading
	    [SerializeField] private ViewLoadingBrewer imageContentLoading; //content loading
    [SerializeField] private bool isWorking = false;
    public bool IsWorking => isWorking;

    public bool isBrewing = false;
    public override double ProductPerSecond
    {
        get => config.ProductPerSecond * CurrentShaft.EfficiencyBoost * CurrentShaft.SpeedBoost * CurrentShaft.GetGlobalBoost();
    }
	bool? isRequireCallToTutorial = null;

	public void SetValueParameterIsRequireCallToTutorial() => isRequireCallToTutorial = true;


	public override float WorkingTime
    {
        get => config.WorkingTime / CurrentShaft.SpeedBoost;
    }

    public override float MoveTime
    {
        get => config.MoveTime / CurrentShaft.SpeedBoost;
    }

	[SerializeField] private ParticleSystem dustFx;
	private bool isBoostingFX;
	[Header("GHOST")]
	public SkeletonGhost ghostBody;
	public SkeletonGhost ghostHead;
    void Start()
    {
	    imageLoading = GameData.Instance.InstantiatePrefab(PrefabEnum.HeadLoading).GetComponent<Image>();
	    imageContentLoading = imageLoading.GetComponent<ViewLoadingBrewer>();
	    imageLoading.transform.SetParent(brewerView.transform);
	    imageLoading.transform.localPosition = new Vector3(-0.032785f, 0.693f, 0);
        collectTransform = CurrentShaft.BrewLocation;
        depositTransform = CurrentShaft.BrewerLocation;
        imageLoading.gameObject.SetActive(false);
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
       //d numberText.text = "";
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
        imageLoading.gameObject.SetActive(false);
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
		if(isRequireCallToTutorial != null)
		{

			await UniTask.Delay(2000);
			TutorialManager.Instance.TutorialStateMachine.TriggerClickableStates(1);
			isRequireCallToTutorial = null;

		}
    }

    private async void PlayTextAnimation() // Loading bar process
    {
	    imageLoading.gameObject.SetActive(true);
	    float max = WorkingTime;  // Tổng thời gian cần loading
	    float progress = 0f;      // Khởi tạo giá trị loading
	    float elapsedTime = 0f;   // Thời gian đã trôi qua

	    if (imageContentLoading != null)
	    {
		    while (elapsedTime < max)
		    {
			    await UniTask.Yield();  // Chờ một frame

			    elapsedTime += Time.deltaTime; // Tăng thời gian đã trôi qua
			    progress = Mathf.Clamp01(elapsedTime / max); // Tính toán tỷ lệ loading (0 đến 1)

			    imageContentLoading.loadingImage.fillAmount = progress; // Cập nhật fillAmount của loading bar

			    // Bạn có thể làm gì đó với `progress` hoặc `elapsedTime` nếu cần
		    }

		    // Đảm bảo rằng fillAmount được đặt thành 1 khi kết thúc
		    imageContentLoading.loadingImage.fillAmount = 1f;
	    }
    }


	public void BoostFx(bool value)
	{
		isBoostingFX = value;
		ghostHead.enabled = value;
		ghostBody.enabled = value;

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
              //  numberText.transform.localScale = new Vector3(brewerView.transform.localScale.x, 1f, 1f);
                brewerSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
                tailSketonAnimation.AnimationState.SetAnimation(0, "Active", true);
                headSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);

				if(isBoostingFX)
				{
					dustFx.Play();
				}

                break;
        }
    }
}
