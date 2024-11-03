using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NOOD.SerializableDictionary;
using System.Linq;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using NOOD;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
public class UpgradeUI : MonoBehaviour
{
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

	[Header("Upgrade Icons")]
	[SerializeField] private Image iconImage;
	[SerializeField] private SerializableDictionary<float, Sprite> upgradeIconDic = new SerializableDictionary<float, Sprite>();
	[SerializeField] private SerializableDictionary<float, Sprite> upgradeIconDic_Counter = new SerializableDictionary<float, Sprite>();

	[Header("Fast upgrade UI")]
	[SerializeField] private Slider upgradeSlider;
	[SerializeField] private List<Button> fastUpgradeButtons;
	[SerializeField] private Sprite btnNormalSprite, btnPressSprite;

	[Header("Evolution")]
	[SerializeField] private Slider currentEvolutionSlider, newEvolutionSlider;

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

	[SerializeField]private float currentLevel;
	private ManagerLocation managerLocation;
	string currentTitleText;
	void Start()
	{
		for (int i = 0; i < fastUpgradeButtons.Count; i++)
		{
			Button button = fastUpgradeButtons[i];
			button.image.sprite = btnNormalSprite;
			int index = i;
			switch (i)
			{
				case 0:
					fastUpgradeButtons[i].onClick.AddListener(() => OnFastUpgradeButtonPress(index, 1));
					break;
				case 1:
					fastUpgradeButtons[i].onClick.AddListener(() => OnFastUpgradeButtonPress(index, 10));
					break;
				case 2:
					fastUpgradeButtons[i].onClick.AddListener(() => OnFastUpgradeButtonPress(index, 50));
					break;
				case 3:
					fastUpgradeButtons[i].onClick.AddListener(() => OnFastUpgradeButtonPress(index, upgradeSlider.maxValue));
					break;
			}
		}
		// Deselect all button
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
		this.transform.DOMove(showTrans.position, 0.6f).SetEase(Ease.OutQuart);
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
		this.transform.DOMove(hideTrans.position, 0.6f).SetEase(Ease.InQuart);
		while (canvasGroup.alpha > 0)
		{
			await UniTask.Yield();
			canvasGroup.alpha -= Time.deltaTime * showHideSpeed;
		}
		this.transform.parent.gameObject.SetActive(false);
	}

	private void Upgrade()
	{
		int upgradeAmount = (int)upgradeSlider.value;
		UpgradeManager.Instance.OnUpgradeRequest?.Invoke(upgradeAmount);
	}

	private void OnFastUpgradeButtonPress(int btnIndex, float btnValue)
	{
		// foreach (var btn in fastUpgradeButtons)
		// {
		// 	btn.image.sprite = btnNormalSprite;
		// }
		for (int i = 0; i < fastUpgradeButtons.Count; i++)
		{
			Button button = fastUpgradeButtons[i];
			if (i == btnIndex)
			{
				button.image.sprite = btnPressSprite;
				button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#873C10");
				continue;
			}
			button.image.sprite = btnNormalSprite;
			if (button.interactable == false) continue;
			button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#B9987B");
		}
		upgradeSlider.value = btnValue;
	}

	private void UpdateUpgradeAmount(float value)
	{
		upgradeAmountText.text = "X" + value.ToString();
		titleText.text = currentTitleText + (value + currentLevel);
		double cost = UpgradeManager.Instance.GetUpgradeCost((int)value);
		upgradeCostText.text = Currency.DisplayCurrency(cost);
		UpdateEvolutions(currentLevel + value);

		DisplayNextUpgrade((int)value);

	}

	private void DisplayNextUpgrade(int value)
	{
		switch (managerLocation)
		{
			case ManagerLocation.Shaft:
				productIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetProductIncrement((int)value)) + "/s";
				speedIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetWorkerIncrement((int)value, managerLocation));
				totalProductIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetIncrementTotal((int)value, managerLocation));
				break;
			case ManagerLocation.Elevator:
				productIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetProductIncrement((int)value)) + "/s"; ;
				speedIncrement.text = UpgradeManager.Instance.GetDecreaseSpeed((int)value).ToString("F2");
				totalProductIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetIncrementTotal((int)value, managerLocation));
				break;
			case ManagerLocation.Counter:
				productIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetProductIncrement((int)value)) + "/s";
				speedIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetWorkerIncrement((int)value, managerLocation));
				totalProductIncrement.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetIncrementTotal((int)value, managerLocation));
				break;
		}
	}

	private void UpdateEvolutions(float value)
	{
		switch (managerLocation)
		{
			case ManagerLocation.Shaft:
				for (int i = 0; i < upgradeIconDic.Dictionary.Count; i++)
				{
					if (value >= upgradeIconDic.Dictionary.ElementAt(i).Key)
					{
						Sprite newIcon = upgradeIconDic.Dictionary.ElementAt(i).Value;
						iconImage.sprite = newIcon;
						if (i + 1 < upgradeIconDic.Dictionary.Count)
						{
							UpdateEvolutionSlider(currentLevel, value, upgradeIconDic.Dictionary.ElementAt(i + 1).Key);
							UpdateEvolutionText(upgradeIconDic.Dictionary.ElementAt(i + 1).Key);
						}
					}
				}
				break;
			case ManagerLocation.Elevator:
				break;
			case ManagerLocation.Counter:
				for (int i = 0; i < upgradeIconDic_Counter.Dictionary.Count; i++)
				{
					if (value >= upgradeIconDic_Counter.Dictionary.ElementAt(i).Key)
					{
						Sprite newIcon = upgradeIconDic_Counter.Dictionary.ElementAt(i).Value;
						iconImage.sprite = newIcon;
						if (i + 1 < upgradeIconDic_Counter.Dictionary.Count)
						{
							UpdateEvolutionSlider(currentLevel, value, upgradeIconDic_Counter.Dictionary.ElementAt(i + 1).Key);
							UpdateEvolutionText(upgradeIconDic_Counter.Dictionary.ElementAt(i + 1).Key);
						}
					}
				}
				break;
		}
	}

	private void UpdateEvolutionSlider(float currentLevel, float updateLevel, float levelToEvo)
	{
		currentEvolutionSlider.maxValue = levelToEvo;
		newEvolutionSlider.maxValue = levelToEvo;
		currentEvolutionSlider.value = currentLevel;
		newEvolutionSlider.value = updateLevel;
	}

	private void UpdateEvolutionText(float levelToEvo)
	{
		workerName.text = "mở khóa quầy hàng ở cấp : " + levelToEvo.ToString();
		workerNameLocalizedString.Arguments = new object[] { levelToEvo };
		workerNameLocalizedString.StringChanged -= OnWokerNameStringChange;
		workerNameLocalizedString.StringChanged += OnWokerNameStringChange;
	}

	private void OnWokerNameStringChange(string value)
	{
		workerName.text = value;
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
			DeactivateButton(fastUpgradeButtons[2]);
		}
		else
		{
			ActivateButton(fastUpgradeButtons[1]);
			ActivateButton(fastUpgradeButtons[2]);
		}

		upgradeAmountText.text = "X1";
		upgradeCostText.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value));
		Debug.Log("Max value:" + max + " Upgrade cost:" + UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value) + " Upgrade initial cost:" + UpgradeManager.Instance.GetInitCost());
		OnFastUpgradeButtonPress(0, 1f);
	}

	public void SetWorkerInfo(ManagerLocation locationType, string name, double production, string number, double total, int level)
	{
		managerLocation = locationType;
		string titleKey = string.Empty;
		string currentTitlekey = string.Empty;
		switch (locationType)
		{
			case ManagerLocation.Shaft:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeShaft, parameters: new object[] { level + 1 });
				currentTitlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeShaft);
				currentLevel = level;
				numberOrSpeedPanel.SetActive(true);
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0] + (level + 1).ToString();
				currentTitleText = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0];
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][3];

				UpdateEvolutions(currentLevel);
				numberOrSpeed.text = number + "NV";
				break;
			case ManagerLocation.Elevator:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeElevator);
				currentTitlekey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleUpgradeElevator);
				numberOrSpeedPanel.SetActive(false);
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][0] + (level + 1).ToString();
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
				numberOrSpeedPanel.SetActive(true);
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][0] + (level+1).ToString();
				currentTitleText = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][0];
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][3];
				workerName.text = name;

				numberOrSpeed.text = number + "NV";
				break;
		}
		titleText.text = titleKey;
		currentTitleText = currentTitlekey;
		workerProduction.text = Currency.DisplayCurrency(production) + "/s";
		totalProduction.text = Currency.DisplayCurrency(total);

		DisplayNextUpgrade(1);
		UpdateEvolutions(currentLevel);
	}

	private void DeactivateButton(Button button)
	{
		button.interactable = false;
		Color disabledColor = NoodyCustomCode.HexToColor("#C8C8C8");
		button.image.color = disabledColor;
		button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#735E4C");
	}

	private void ActivateButton(Button button)
	{
		button.interactable = true;
		ColorBlock colors = button.colors;
		colors.normalColor = Color.white;
		button.colors = colors;
		button.image.color = Color.white;
		button.GetComponentInChildren<TextMeshProUGUI>().color = NoodyCustomCode.HexToColor("#873C10");
	}
}
