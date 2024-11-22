using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;
using NOOD.SerializableDictionary;
using Newtonsoft.Json;
using Spine;
using Cysharp.Threading.Tasks;

public class CounterUI : MonoBehaviour
{
	public static Action OnUpgradeRequest;

	[Header("UI Button")]
	[SerializeField] private Button m_upgradeButton;
	[SerializeField] private Button m_managerButton;
	[SerializeField] private Button m_boostButton;
	[SerializeField] private Button m_workerButton;


	[Header("UI Text")]
	[SerializeField] private TextMeshProUGUI m_pawText;
	[SerializeField] private TextMeshProUGUI m_levelText;
	[SerializeField] private TextMeshProUGUI m_costText;

	public SkeletonAnimation m_bgCounter;
	public SkeletonAnimation m_secondBG;
	// [Header("Visual object")]
	// [SerializeField] private GameObject m_quayGiaoNuocHolder;
	[SerializeField] private SkeletonAnimation m_cashierCounter;
	[SerializeField] private SerializableDictionary<int, SkeletonDataAsset> skeletonDataAssetDic;
	private Counter m_counter;
	private CounterUpgrade m_counterUpgrade;


	void Awake()
	{
		m_counter = GetComponent<Counter>();
		m_counterUpgrade = GetComponent<CounterUpgrade>();
	}

	void Start()
	{
		m_levelText.text = m_counterUpgrade.CurrentLevel.ToString();
		m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
		//UpdateFrameButtonUpgrade(m_counterUpgrade.CurrentLevel);
	}

	void Update()
	{
		m_pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
		m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
		m_levelText.text ="Lv. " + m_counterUpgrade.CurrentLevel.ToString();
	}

	void OnEnable()
	{
		m_upgradeButton.onClick.AddListener(UpgradeRequest);
		m_managerButton.onClick.AddListener(OpenManagerPanel);
		m_boostButton.onClick.AddListener(ActiveBoost);
		BaseUpgrade.OnUpgrade += UpdateUpgradeButton;
		m_counter.OnUpgrade += Counter_OnUpgradeHandler;
		m_workerButton.onClick.AddListener(AwakeWorker);
	}

	void OnDisable()
	{
		m_upgradeButton.onClick.RemoveListener(UpgradeRequest);
		m_managerButton.onClick.RemoveListener(OpenManagerPanel);
		m_boostButton.onClick.RemoveListener(ActiveBoost);
		BaseUpgrade.OnUpgrade -= UpdateUpgradeButton;
		m_counter.OnUpgrade -= Counter_OnUpgradeHandler;
		m_workerButton.onClick.RemoveListener(AwakeWorker);
	}
	private void Counter_OnUpgradeHandler(int currentLevel)
	{
		foreach (var item in skeletonDataAssetDic.Dictionary)
		{
			if (currentLevel >= item.Key)
			{
				UpgradeCounter(item.Value);
			}
		}
	}
	private void UpgradeCounter(SkeletonDataAsset tableDataAsset)
	{
		m_cashierCounter.skeletonDataAsset = tableDataAsset;
		m_cashierCounter.Initialize(true, true);
		//tableAnimation.skeletonDataAsset = tableDataAsset;
		//tableAnimation.Initialize(true, true);
	}

	public void AddManagerInteract(bool isShowing) => m_managerButton.gameObject.SetActive(isShowing);
	
	void CallUpgrade()
	{
		if (PawManager.Instance.CurrentPaw >= m_counterUpgrade.CurrentCost)
		{
			m_counterUpgrade.Upgrade(1);
		}
	}

	void UpdateUpgradeButton(BaseUpgrade upgrade, int level)
	{
		if (upgrade == m_counterUpgrade)
		{
			m_levelText.text = "Level " + level;
			m_costText.text = Currency.DisplayCurrency(m_counterUpgrade.CurrentCost);
			UpdateFrameButtonUpgrade(level);
		}
	}
	void UpdateFrameButtonUpgrade(int currentLevel)
	{

		Image imgButtonUpgrade = m_upgradeButton.GetComponent<Image>();
		if (currentLevel <= 600)
		{
			imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][0]);
		}
		else if (currentLevel > 600 && currentLevel <= 1200)
		{
			imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][1]);
		}
		else if (currentLevel > 1200 && currentLevel <= 1800)
		{
			imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][2]);
		}
		else if (currentLevel > 1800 && currentLevel <= 2400)
		{
			imgButtonUpgrade.sprite = Resources.Load<Sprite>(MainGameData.FrameLevelButton[ManagerLocation.Counter][3]);
		}
	}
	void ActiveBoost()
	{
		m_counter.RunBoost();
	}
	void OpenManagerPanel()
	{
		ManagersController.Instance.OpenManagerPanel(m_counter.ManagerLocation);
	}

	public void UpgradeRequest()
	{
		OnUpgradeRequest?.Invoke();
	}

	public void PlayCollectAnimation(bool isBrewing)
	{
		Debug.Log("khoa PlayCollectAnimation:"+ isBrewing);
		if (isBrewing == false && m_cashierCounter.AnimationState.GetCurrent(0).Animation.Name != "Idle")
		{
			Debug.Log("khoa idel");
			m_cashierCounter.AnimationState.ClearTrack(0);
			m_cashierCounter.AnimationState.SetAnimation(0, "Idle", true);
			//m_managerCounter.AnimationState.SetAnimation(0, "Idle", true);
			return;
		}
		if (isBrewing && m_cashierCounter.AnimationState.GetCurrent(0).Animation.Name != "Active")
		{
			Debug.Log("khoa acticve");
			m_cashierCounter.AnimationState.ClearTrack(0);
			m_cashierCounter.AnimationState.SetAnimation(0, "Active", true);
		//	m_managerCounter.AnimationState.SetAnimation(0, "Active", true);
			return;
		}
	}

	public void UpdateSkeletonData()
	{
		var skinGameData = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
		m_bgCounter.skeletonDataAsset = skinGameData[InventoryItemType.CounterBg];
		m_secondBG.skeletonDataAsset = skinGameData[InventoryItemType.CounterSecondBg];

		m_bgCounter.Initialize(true);
		m_secondBG.Initialize(true);

		var counter = GetComponent<Counter>();

		foreach (var item in counter.Transporters)
		{
			var cartSkeleton = item.CartSkeletonAnimation;

			cartSkeleton.skeletonDataAsset = skinGameData[InventoryItemType.CounterCart];
			cartSkeleton.Initialize(true);

			var headSkeleton = item.HeadSkeletonAnimation;
			var bodySkeleton = item.BodySkeletonAnimation;
			headSkeleton.skeletonDataAsset = skinGameData[InventoryItemType.CounterCharacter];
			bodySkeleton.skeletonDataAsset = skinGameData[InventoryItemType.CounterCharacter];

			headSkeleton.Initialize(true);
			bodySkeleton.Initialize(true);
		}
	
	}
	public void ChangeSkin(CounterSkin data)
	{
		if (data == null) return;


		string skinBGName = "Skin_" + (int.Parse(data.idBackGround) + 1);
		m_bgCounter.Skeleton.SetSkin(skinBGName);
		m_bgCounter.Skeleton.SetSlotsToSetupPose();

		string skinSecondBGName = "Skin_" + (int.Parse(data.idSecondBg) + 1);
		m_secondBG.Skeleton.SetSkin(skinSecondBGName);
		m_secondBG.Skeleton.SetSlotsToSetupPose();

		if (TryGetComponent<Counter>(out var counter))
		{

			int cartIndex = int.Parse(data.idCart);
			int headIndex = int.Parse(data.character.idHead);
			int bodyIndex = int.Parse(data.character.idBody);
			foreach (var item in counter.Transporters)
			{
				var skeleton = item.CartSkeletonAnimation.skeleton;

				skeleton.SetSkin("Skin_" + (cartIndex + 1));
				skeleton.SetSlotsToSetupPose();


				var headSkeleton = item.HeadSkeletonAnimation.skeleton;
				var bodySkeleton = item.BodySkeletonAnimation.skeleton;

				headSkeleton.SetSkin("Head/Skin_" + (headIndex + 1));
				bodySkeleton.SetSkin("Body/Skin_" + (bodyIndex + 1));
				headSkeleton.SetSlotsToSetupPose();
				bodySkeleton.SetSlotsToSetupPose();
				item.TailSkeletonAnimation.gameObject.SetActive(true);
				if (item.TailSkeletonAnimation.skeleton.Data.FindSkin("Tail/Skin_" + (headIndex + 1)) != null)
				{
					item.TailSkeletonAnimation.skeleton.SetSkin("Tail/Skin_" + (headIndex + 1));
					item.TailSkeletonAnimation.skeleton.SetSlotsToSetupPose();
				}
				else
				{
					item.TailSkeletonAnimation.gameObject.SetActive(false);
				}

			}
		}
	}

	private void AwakeWorker()
	{
		m_counter.AwakeWorker();
	}
}
