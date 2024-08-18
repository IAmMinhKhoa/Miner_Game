using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NOOD.SerializableDictionary;
using UnityEditor;
using System.Linq;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Security.Cryptography;

public class UpgradeUI : MonoBehaviour
{
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

	[Header("Worker Info Name")]
	[SerializeField] private TextMeshProUGUI s_workerProduction;
	[SerializeField] private TextMeshProUGUI s_numberOrSpeed;
	[SerializeField] private TextMeshProUGUI s_totalProduction;

	private float currentLevel;
	private ManagerLocation managerLocation;

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
		this.transform.DOMove(showTrans.position, 0.5f).SetEase(Ease.Flash);
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
		this.transform.DOMove(hideTrans.position, 0.25f).SetEase(Ease.Flash);
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
		foreach (var btn in fastUpgradeButtons)
		{
			btn.image.sprite = btnNormalSprite;
		}
		fastUpgradeButtons[btnIndex].image.sprite = btnPressSprite;
		upgradeSlider.value = btnValue;
	}

	private void UpdateUpgradeAmount(float value)
	{
		upgradeAmountText.text = value.ToString();
		double cost = UpgradeManager.Instance.GetUpgradeCost((int)value);
		upgradeCostText.text = Currency.DisplayCurrency(cost);
		UpdateEvolutions(currentLevel + value);
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
		workerName.text = $"MỞ KHÓA QUẦY HÀNG Ở CẤP {levelToEvo}";
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

		upgradeAmountText.text = "1";
		upgradeCostText.text = Currency.DisplayCurrency(UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value));
		Debug.Log("Max value:" + max + " Upgrade cost:" + UpgradeManager.Instance.GetUpgradeCost((int)upgradeSlider.value) + " Upgrade initial cost:" + UpgradeManager.Instance.GetInitCost());
	}

	public void SetWorkerInfo(ManagerLocation locationType, string name, double production, string number, double total, int level)
	{
		managerLocation = locationType;
		switch (locationType)
		{
			case ManagerLocation.Shaft:
				currentLevel = level;
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][0] + level.ToString();
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Shaft][3];

				UpdateEvolutions(currentLevel);
				numberOrSpeed.text = number;
				break;
			case ManagerLocation.Elevator:
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][0] + level.ToString();
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Elevator][3];
				workerName.text = name;

				numberOrSpeed.text = number + " s";
				break;
			case ManagerLocation.Counter:
				titleText.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][0] + level.ToString();
				s_workerProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][1];
				s_numberOrSpeed.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][2];
				s_totalProduction.text = MainGameData.UpgradeDetailInfo[ManagerLocation.Counter][3];
				workerName.text = name;

				numberOrSpeed.text = number;
				break;
		}

		workerProduction.text = Currency.DisplayCurrency(production) + "/s";
		totalProduction.text = Currency.DisplayCurrency(total);
	}

	private void DeactivateButton(Button button)
	{
		button.interactable = false;
		ColorBlock colors = button.colors;
		ColorUtility.TryParseHtmlString("#C8C8C8", out Color disabledColor);
		colors.disabledColor = disabledColor;
		button.colors = colors;
	}

	private void ActivateButton(Button button)
	{
		button.interactable = true;
		ColorBlock colors = button.colors;
		colors.normalColor = Color.white;
		button.colors = colors;
	}
}
