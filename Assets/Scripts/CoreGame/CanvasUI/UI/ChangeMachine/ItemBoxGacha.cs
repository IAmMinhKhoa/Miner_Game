using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxGacha : MonoBehaviour
{
	[SerializeField] Image lowBg;
	[SerializeField] Image normalBg;
	[SerializeField] Image superBg;
	[SerializeField] Image ultraBg;
	[SerializeField] Image low;
	[SerializeField] Image normal;
	[SerializeField] Image super;
	[SerializeField] Image ultra;
	[SerializeField] Image type;
	[Header("Spine")]
	[SerializeField] SkeletonGraphic skeletonGraphic;
	[SerializeField] SkeletonGraphic subSkeletonGraphic;
	[SerializeField] SkeletonGraphic effectSpine1;
	[SerializeField] SkeletonGraphic effectSpine2;

	[Header("Sprite")]
	[SerializeField]
	Sprite lowType;
	[SerializeField]
	Sprite normalType;
	[SerializeField]
	Sprite superType;
	[SerializeField]
	Sprite ultraType;
	

	[Header("Button")]
	[SerializeField]
	Button closeUIButton;
	[SerializeField]
	Button skipButton;
	[Header("Text")]
	[SerializeField]
	TextMeshProUGUI titile;
	[SerializeField]
	TextMeshProUGUI description;

	[Header("Other")]
	[SerializeField]
	GameObject card;

	public event Action OnSkipButtonClick;
	[SerializeField] private TMP_Text title_quality;
	private void OnEnable()
	{
		RectTransform _rectTransform = card.GetComponent<RectTransform>();
		_rectTransform.DOScale(0, 0);
		_rectTransform.DOScale(1.2f, 0.3f)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				_rectTransform.DOScale(1, 0.2f).SetEase(Ease.InQuad);
			});
		effectSpine1.Initialize(true);
		effectSpine2.Initialize(true);
	}

	private void Start()
	{
		closeUIButton.onClick.AddListener(CloseUIOnClick);
		skipButton.onClick.AddListener(SkipOnClick);
	}

	private void SkipOnClick()
	{
		OnSkipButtonClick?.Invoke();
		gameObject.SetActive(false);
	}

	private void CloseUIOnClick()
	{
		gameObject.SetActive(false);
	}

	public void InitialData(GachaItemInfor itemInfo,string initialSkiName ,bool isUpdatePos)
	{
		gameObject.SetActive(true);
		var items = new[] { low, normal, super, ultra };
		var backgrounds = new[] { lowBg, normalBg, superBg, ultraBg };
		foreach (var item in items) item.gameObject.SetActive(false);
		foreach (var bg in backgrounds) bg.gameObject.SetActive(false);
		string titleKeyQuality = string.Empty;
		switch (itemInfo.skinGachaInfor.quality)
		{
			case MarketPlayItemQuality.low:
				low.gameObject.SetActive(true);
				lowBg.gameObject.SetActive(true);
				type.sprite = lowType;
				titleKeyQuality = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketMarketGoods);
				break;
			case MarketPlayItemQuality.normal:
				normal.gameObject.SetActive(true);
				normalBg.gameObject.SetActive(true);
				type.sprite = normalType;
				titleKeyQuality = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketImportedGoods);
				break;
			case MarketPlayItemQuality.super:
				super.gameObject.SetActive(true);
				superBg.gameObject.SetActive(true);
				type.sprite = superType;
				titleKeyQuality = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketCrafts);
				break;
			case MarketPlayItemQuality.ultra:
				ultra.gameObject.SetActive(true);
				ultraBg.gameObject.SetActive(true);
				type.sprite = ultraType;
				titleKeyQuality = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketCrafts);
				break;
		}
		title_quality.text = titleKeyQuality;
		skeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
		skeletonGraphic.initialSkinName = initialSkiName + itemInfo.skinGachaInfor.ID;
		skeletonGraphic.Initialize(true);
		if(subSkeletonGraphic != null)
		{
			subSkeletonGraphic.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[itemInfo.type];
			subSkeletonGraphic.initialSkinName = "Body/Skin_" + itemInfo.skinGachaInfor.ID;
			subSkeletonGraphic.Initialize(true);
			var spine = subSkeletonGraphic.GetComponent<RectTransform>();
			spine.localScale = itemInfo.skinGachaInfor.ScaleSingle;
			spine.anchoredPosition = itemInfo.skinGachaInfor.PositonSingle;
		}

		var iconAnimation = skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon");
		if (iconAnimation != null)
		{
			skeletonGraphic.AnimationState.SetAnimation(0, "Icon", false);
		}
		else
		{
			skeletonGraphic.AnimationState.ClearTrack(0);
		}

		string titleKey = string.Empty;
		string titleKeyDesc = string.Empty;
		string titleKeySubName = string.Empty;
		switch (itemInfo.type)
		{
			case InventoryItemType.ShaftBg:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftBg);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftBg);
				break;
			case InventoryItemType.CounterBg:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallCouter);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallCouter);
				break;
			case InventoryItemType.ElevatorBg:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallElevator);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallElevator);
				break;
			case InventoryItemType.CounterCart:
				break;
			case InventoryItemType.Elevator:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryElevator);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryElevator);
				break;
			case InventoryItemType.ShaftSecondBg:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftSecondBg);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftSecondBg);
				break;
			case InventoryItemType.ShaftCart:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryCart);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryCart);
				break;
			case InventoryItemType.ShaftWaitTable:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWaitTable);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWaitTable);
				break;
			case InventoryItemType.ShaftCharacter:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryHead);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryHead);
				titleKeySubName = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketCartStaff);
				break;
			case InventoryItemType.ElevatorCharacter:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryHead);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryHead);
				titleKeySubName = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketElevatorStaff);
				break;
			case InventoryItemType.CounterCharacter:
				break;
			case InventoryItemType.CounterSecondBg:
				break;
			case InventoryItemType.BackElevator:
				break;
			case InventoryItemType.ShaftCharacterBody:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryBody);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryBody);
				titleKeySubName = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketCartStaff);
				break;
			case InventoryItemType.ElevatorCharacterBody:
				titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryBody);
				titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryBody);
				titleKeySubName = LocalizationManager.GetLocalizedString(LanguageKeys.TitleMarketElevatorStaff);
				break;
		}


		//titile.text = itemInfo.skinGachaInfor.Name;
		//description.text = itemInfo.skinGachaInfor.Description;
		titile.text = titleKey;
		description.text = titleKeyDesc;
		if(isUpdatePos) {
			var spine = skeletonGraphic.GetComponent<RectTransform>();
			spine.localScale = itemInfo.skinGachaInfor.ScaleSingle;
			spine.anchoredPosition = itemInfo.skinGachaInfor.PositonSingle;
		}
	}
}
