using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NOOD.SerializableDictionary;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using NOOD;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using NOOD.Sound;
using Spine.Unity;

public class UpgradeUI : MonoBehaviour
{
	private Vector3 scale_tablet = new Vector3(0.87f, 0.87f, 0.87f);

	[Header("Localization")]
	[SerializeField] private LocalizedString workerNameLocalizedString;

	[Header("Show Hide Transform")]
	[SerializeField] private Transform showTrans;
	[SerializeField] private Transform hideTrans;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float showHideSpeed = 2f;

	[Header("Buttons UI")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button upgradeButton;

	[Header("Fast upgrade UI")]
	[SerializeField] private Slider upgradeSlider;
	[SerializeField] private List<Button> fastUpgradeButtons;
	[SerializeField] private Sprite btnNormalSprite, btnPressSprite;

	[Header("Text UI")]
	[SerializeField] private TextMeshProUGUI upgradeAmountText;
	[SerializeField] private TextMeshProUGUI upgradeCostText;
	[SerializeField] private TextMeshProUGUI titleText;

	[Header("Worker Info")]
	[SerializeField] private TextMeshProUGUI workerName;
	[SerializeField] private TextMeshProUGUI workerProduction;
	[SerializeField] private TextMeshProUGUI numberOrSpeed;
	[SerializeField] private TextMeshProUGUI totalProduction;
	[SerializeField] private GameObject numberOrSpeedPanel;

	[Header("Worker Increment")]
	[SerializeField] private TextMeshProUGUI productIncrement;
	[SerializeField] private TextMeshProUGUI speedIncrement;
	[SerializeField] private TextMeshProUGUI totalProductIncrement;

	[Header("Worker Info Name")]
	[SerializeField] private TextMeshProUGUI s_workerProduction;
	[SerializeField] private TextMeshProUGUI s_numberOrSpeed;
	[SerializeField] private TextMeshProUGUI s_totalProduction;

	[Header("Bar Counter Skin Display")]
	[SerializeField] private GameObject barCounterSkinPanel;
	[SerializeField] private SkeletonGraphic barCounterSkeleton;

	[Header("Counter Skin Display")]
	[SerializeField] private GameObject counterSkinPanel;
	[SerializeField] private SkeletonGraphic counterSkeletonGraphic;

	private float currentLevel;
	private ManagerLocation managerLocation;
	private string currentTitleText;

	private float soundCoolDown = 2f;
	private float lastPlayTime = 0f;
	private bool isClickBtnFast = false;

	void Start()
	{
		if (Common.IsTablet)
			gameObject.transform.localScale = scale_tablet;

		for (int i = 0; i < fastUpgradeButtons.Count; i++)
		{
			Button button = fastUpgradeButtons[i];
			button.image.sprite = btnNormalSprite;
			int index = i;
			switch (i)
			{
				case 0: button.onClick.AddListener(() => OnFastUpgradeButtonPress(index, 1)); break;
				case 1: button.onClick.AddListener(() => OnFastUpgradeButtonPress(index, 10)); break;
				case 2: button.onClick.AddListener(() => OnFastUpgradeButtonPress(index, 50)); break;
				case 3: button.onClick.AddListener(() => OnFastUpgradeButtonPress(index, upgradeSlider.maxValue)); break;
			}
		}
		OnFastUpgradeButtonPress(-1, -1);
	}

	private void OnEnable()
	{
		closeButton.onClick.AddListener(ClosePanel);
		upgradeButton.onClick.AddListener(Upgrade);
		upgradeSlider.onValueChanged.AddListener(UpdateUpgradeAmount);
	}

	private void OnDisable()
	{
		closeButton.onClick.RemoveListener(ClosePanel);
		upgradeButton.onClick.RemoveListener(Upgrade);
		upgradeSlider.onValueChanged.RemoveListener(UpdateUpgradeAmount);
	}

	public async void OpenPanel()
	{
		transform.DOMove(showTrans.position, 0.6f).SetEase(Ease.OutQuart);
		while (canvasGroup.alpha < 1)
		{
			await UniTask.Yield();
			canvasGroup.alpha += Time.deltaTime * showHideSpeed;
			canvasGroup.interactable = false;
		}
		canvasGroup.interactable = true;
	}

	private async void ClosePanel()
	{
		SoundManager.PlaySound(SoundEnum.mobileTexting2);
		transform.DOMove(hideTrans.position, 0.6f).SetEase(Ease.InQuart);
		while (canvasGroup.alpha > 0)
		{
			await UniTask.Yield();
			canvasGroup.alpha -= Time.deltaTime * showHideSpeed;
		}
		transform.parent.gameObject.SetActive(false);
	}

	private void Upgrade()
	{
		int upgradeAmount = (int)upgradeSlider.value;
		UpgradeManager.Instance.OnUpgradeRequest?.Invoke(upgradeAmount);
	}

	private void OnFastUpgradeButtonPress(int btnIndex, float btnValue)
	{
		isClickBtnFast = true;
		for (int i = 0; i < fastUpgradeButtons.Count; i++)
		{
			Button button = fastUpgradeButtons[i];
			if (i == btnIndex)
			{
				button.image.sprite = btnPressSprite;
				button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#873C10");
			}
			else
			{
				button.image.sprite = btnNormalSprite;
				if (button.interactable)
					button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#B9987B");
			}
		}
		upgradeSlider.value = btnValue;
		isClickBtnFast = false;
	}

	private void UpdateUpgradeAmount(float value)
	{
		if (!isClickBtnFast && Time.time - lastPlayTime >= soundCoolDown)
		{
			SoundManager.PlaySound(SoundEnum.heavyWoodDrag6);
			lastPlayTime = Time.time;
		}

		upgradeAmountText.text = "X" + value;
		titleText.text = currentTitleText + (value + currentLevel);
		double cost = UpgradeManager.Instance.GetUpgradeCost((int)value);
		upgradeCostText.text = Currency.DisplayCurrency(cost);

		DisplayNextUpgrade((int)value);
	}

	private void DisplayNextUpgrade(int value)
	{
		switch (managerLocation)
		{
			case ManagerLocation.Shaft:
				/*productIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetProductIncrement((int)value)) + "/s";
				speedIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetWorkerIncrement((int)value, managerLocation));
				totalProductIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetIncrementTotal((int)value, managerLocation));*/
				totalProductIncrement.text ="+"+Currency.DisplayCurrency(UpgradeManager.Instance.GetTotalCakeValue(value));
				productIncrement.text ="-"+UpgradeManager.Instance.GetTotalBakingTime(value).ToString("F2");

				break;
			case ManagerLocation.Elevator:
				productIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetProductIncrement(value)) + "/s";
				speedIncrement.text = UpgradeManager.Instance.GetDecreaseSpeed(value).ToString("F2");
				totalProductIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetIncrementTotal(value, managerLocation));
				break;
			case ManagerLocation.Counter:
				totalProductIncrement.text ="+"+Currency.DisplayCurrency(UpgradeManager.Instance.GetTotalCakeValue(value));
				productIncrement.text ="-"+UpgradeManager.Instance.GetTotalBakingTime(value).ToString("F2");
				break;
		}
	}

	public void SetUpPanel(int max)
	{
		upgradeSlider.maxValue = Mathf.Max(max, 1);
		upgradeSlider.value = 1;

		if (max < 10)
		{
			DeactivateButton(fastUpgradeButtons[1]);
			DeactivateButton(fastUpgradeButtons[2]);
		}
		else if (max < 50)
		{
			ActivateButton(fastUpgradeButtons[1]);
			DeactivateButton(fastUpgradeButtons[2]);
		}
		else
		{
			ActivateButton(fastUpgradeButtons[1]);
			ActivateButton(fastUpgradeButtons[2]);
		}

		upgradeAmountText.text = "X1";
		upgradeCostText.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetUpgradeCost(1));
		OnFastUpgradeButtonPress(0, 1f);
	}

	public void SetWorkerInfo(ManagerLocation locationType, string name, double production, string number, double total, int level)
	{
		managerLocation = locationType;
		string titleKey = string.Empty;
		string currentTitlekey = string.Empty;
		switch (locationType)
		{
			case ManagerLocation.Shaft: //show speed + value of cake
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeShaft);
				currentTitlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeShaft);
				currentLevel = level;
				numberOrSpeedPanel.SetActive(false);

				currentTitleText = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0];
				s_workerProduction.text = LanguageKeys.upgradeCakeTime.Text();
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][2];
				s_totalProduction.text = LanguageKeys.upgradeCakeValue.Text();
//				Debug.LogError("khoa:"+s_workerProduction.text+"/"+s_totalProduction.text+"/"+s_numberOrSpeed.text);
				//các text ở đây đang bị đè bởi component localize, chờ final design rồi sửa

				//Debug.LogError("khoa:"+s_workerProduction.text+"/"+s_totalProduction.text);
				break;
			case ManagerLocation.Elevator:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeElevator);
				currentTitlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeElevator);
				numberOrSpeedPanel.SetActive(false);

				currentTitleText = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][0];
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][3];
				workerName.text = name;
				numberOrSpeed.text = number + " s";
				break;
			case ManagerLocation.Counter:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeCounter);
				currentTitlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeCounter);
				currentLevel = level;
				numberOrSpeedPanel.SetActive(false);
				currentTitleText = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0];
				s_workerProduction.text = LanguageKeys.upgradeCakeTime.Text();
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][2];
				s_totalProduction.text = LanguageKeys.upgradeCakeValue.Text();
				break;
		}
		titleText.text = $"{titleKey} {level + 1}";
		currentTitleText = currentTitlekey;
		if (locationType == ManagerLocation.Shaft || locationType == ManagerLocation.Counter)
		{
			workerProduction.text=production.ToString("F2")+ " s";
			totalProduction.text = Currency.DisplayCurrency(number)+" Paw";
		}
		else
		{
			workerProduction.text = Currency.DisplayCurrency(production) + "/s";
			totalProduction.text = Currency.DisplayCurrency(total);
		}
		// Ensure only the correct panel is shown
		if (barCounterSkinPanel != null) barCounterSkinPanel.SetActive(false);
		if (counterSkinPanel != null) counterSkinPanel.SetActive(false);

		if (locationType == ManagerLocation.Shaft && barCounterSkinPanel != null)
			barCounterSkinPanel.SetActive(true);
		else if (locationType == ManagerLocation.Counter && counterSkinPanel != null)
			counterSkinPanel.SetActive(true);

		DisplayNextUpgrade(1);
	}

	private void DeactivateButton(Button button)
	{
		button.interactable = false;
		button.image.color = NoodyCustomCode.HexToColor("#C8C8C8");
		button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#735E4C");
	}

	private void ActivateButton(Button button)
	{
		button.interactable = true;
		button.image.color = Color.white;
		button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#873C10");
	}

	public void SetBarCounterData(SkeletonDataAsset dataAsset, string skinName)
	{
		if (barCounterSkeleton == null)
		{
			Debug.LogWarning("⚠️ barCounterSkeleton chưa được gán!");
			return;
		}

		var skeletonData = dataAsset.GetSkeletonData(true);
		var foundSkin = skeletonData.FindSkin(skinName);
		if (foundSkin == null)
		{
			Debug.LogError($"❌ Không tìm thấy skin '{skinName}' trong {dataAsset.name}");
			foreach (var skin in skeletonData.Skins)
				Debug.Log($"➡️ Skin có sẵn: {skin.Name}");
			return;
		}

		barCounterSkeleton.skeletonDataAsset = dataAsset;
		barCounterSkeleton.Initialize(true);
		barCounterSkeleton.Skeleton.SetSkin(foundSkin);
		barCounterSkeleton.Skeleton.SetSlotsToSetupPose();
		barCounterSkeleton.AnimationState.SetAnimation(0, "Idle", true);
	}

	public void SetCashierCounterData(SkeletonDataAsset dataAsset, string skinName)
	{
		if (counterSkeletonGraphic == null)
		{
			Debug.LogWarning("⚠️ counterSkeletonGraphic chưa được gán!");
			return;
		}

		var skeletonData = dataAsset.GetSkeletonData(true);
		var foundSkin = skeletonData.FindSkin(skinName);
		if (foundSkin == null)
		{
			Debug.LogError($"❌ Không tìm thấy skin '{skinName}' trong {dataAsset.name}");
			foreach (var skin in skeletonData.Skins)
				Debug.Log($"➡️ Skin có sẵn: {skin.Name}");
			return;
		}

		counterSkeletonGraphic.skeletonDataAsset = dataAsset;
		counterSkeletonGraphic.Initialize(true);
		counterSkeletonGraphic.Skeleton.SetSkin(foundSkin);
		counterSkeletonGraphic.Skeleton.SetSlotsToSetupPose();
		counterSkeletonGraphic.AnimationState.SetAnimation(0, "Idle", true);
	}
}
