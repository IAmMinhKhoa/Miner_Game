using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMarketItemUIHandle : MonoBehaviour
{
	[SerializeField]
	GameObject backDrop;
	[SerializeField]
	GameObject notEnoughMoneyNotification;
	[SerializeField]
	SkeletonGraphic Spine;
	[SerializeField]
	TextMeshProUGUI normalCost;
	[SerializeField]
	TextMeshProUGUI superMoneyCost;
	[SerializeField]
	Sprite lowQuality;
	[SerializeField]
	Sprite normalQuality;
	[SerializeField]
	Sprite superQuality;
	[SerializeField]
	UnityEngine.UI.Image quality;
	[SerializeField]
	Button normalBuyButton;
	[SerializeField]
	Button superBuyButton;
	[SerializeField]
	Image hideNormalBuyIMG;
	[SerializeField]
	Image hideSuperBuyIMG;
	[SerializeField]
	TextMeshProUGUI title;
	[SerializeField]
	TextMeshProUGUI description;
	[SerializeField]
	TextMeshProUGUI subNameText;
	[SerializeField]
	SkeletonGraphic subHead;
	[SerializeField]
	SkeletonGraphic subBody;
	[Header("Character Skin SO")]
	[SerializeField]
	CharacterScalePosSO headShaftScale;
	[SerializeField]
	CharacterScalePosSO bodyShaftScale;
	[SerializeField]
	CharacterScalePosSO headElevatorScale;
	[SerializeField]
	CharacterScalePosSO bodyElevatorScale;
	public event Action<MarketPlayItem> OnButtonBuyClick;
	public event Action<MarketPlayItem> OnButtonBuyBySuperMoneyClick;

	MarketPlayItem curItemHandling;
	bool CurState
	{
		set
		{
			if (value)
			{
				FadeInContainer();
			}
			else
			{
				FadeOutContainer();
			}
		}
	}

	private void Start()
	{
		normalBuyButton.GetComponent<ButtonBehavior>().onClickEvent.AddListener(Buy);
		superBuyButton.GetComponent<ButtonBehavior>().onClickEvent.AddListener(BuyBySuperMoney);
	}
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		backDrop.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);
	}
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
			backDrop.SetActive(false);
			curItemHandling = null;
		});
	}
	public void Close()
	{
		if (!gameObject.activeInHierarchy) return;
		CurState = false;
	}
	public void Open()
	{
		CurState = true;
	}

	public void Init(ItemSize itemSize, MarketPlayItem it)
	{

		switch(it.ItemQuality)
		{
			case MarketPlayItemQuality.low:
				quality.sprite = lowQuality;
				break;
			case MarketPlayItemQuality.normal:
				quality.sprite = normalQuality;
				break;
			case MarketPlayItemQuality.super:
				quality.sprite = superQuality;
				break;
		}

		Dictionary<InventoryItemType, string> keyValuePairs = new()
		{
			{ InventoryItemType.ShaftBg, "Back Ground Tầng"},
			{ InventoryItemType.CounterBg, "Back Ground Quầy"},
			{ InventoryItemType.ElevatorBg, "Back Ground Phòng Trà Sữa"},
			{ InventoryItemType.ShaftCart, "Xe Đẩy"},
			{ InventoryItemType.Elevator, "Thang Máy"},
			{ InventoryItemType.ShaftSecondBg, "Nền"},
			{ InventoryItemType.ShaftWaitTable, "Bàn Chờ"},
			{ InventoryItemType.ShaftCharacter, "Nhân Viên Đẩy Xe"},
			{ InventoryItemType.ElevatorCharacter, "Nhân Viên Thang Máy"},
			{ InventoryItemType.ShaftCharacterBody, "Nhân Viên Đẩy Xe"},
			{ InventoryItemType.ElevatorCharacterBody, "Nhân Viên Thang Máy"},
		};
		Debug.LogError(itemSize.type.ToString());
		string titleKey = string.Empty;
		string titleKeyDesc = string.Empty;
		string titleKeySubName = string.Empty;
		switch (itemSize.type)
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
		if (subNameText != null)
		{
			//subNameText.text = keyValuePairs[itemSize.type];
			subNameText.text = titleKeySubName;
		}
		curItemHandling = it;
		if(it.SpineHandling.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon") != null) {
			Spine.startingAnimation = "Icon";
		}
		Spine.initialSkinName = itemSize.skinName + it.ID;
		Spine.skeletonDataAsset = it.SpineHandling.skeletonDataAsset;
		Spine.Initialize(true);
		//
		
		var transform = Spine.GetComponent<RectTransform>();
		transform.localScale = itemSize.scale;
		transform.anchoredPosition = itemSize.pos;
		Spine.Skeleton.SetSkin(itemSize.skinName + it.ID);
		Spine.Skeleton.SetSlotsToSetupPose();

		var skinInfo = SkinManager.Instance.InfoSkinGame[itemSize.type].Where(x => x.id == it.ID).First();
		//title.text = skinInfo.name;
		//description.text = skinInfo.desc;
		title.text = titleKey +" " + it.ID.ToString();
		description.text = titleKeyDesc;
		normalCost.text =  Currency.DisplayCurrency(it.Cost);
		superMoneyCost.text = it.SuperCost.ToString();
		//if (it.IsItemBougth)
		//{
		//	normalCost.text = "Đã mua";
		//	superMoneyCost.text = "Đã mua";
		//	normalBuyButton.interactable = false;
		//	superBuyButton.interactable = false;
		//	hideNormalBuyIMG.gameObject.SetActive(true);
		//	hideSuperBuyIMG.gameObject.SetActive(true);
		//	LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
		//}
		//else
		//{
		//	normalBuyButton.interactable = true;
		//	superBuyButton.interactable = true;
		//	hideNormalBuyIMG.gameObject.SetActive(false);
		//	hideSuperBuyIMG.gameObject.SetActive(false);
		//}
		switch (itemSize.type)
		{
			case InventoryItemType.ShaftCharacter:
				ActiveSubSpine(true, headShaftScale);
				break;
			case InventoryItemType.ShaftCharacterBody:
				ActiveSubSpine(false, bodyShaftScale);
				break;
			case InventoryItemType.ElevatorCharacter:
				ActiveSubSpine(true, headElevatorScale);
				break;
			case InventoryItemType.ElevatorCharacterBody:
				ActiveSubSpine(false, bodyElevatorScale);
				break;
		}
	}

	public void Buy()
	{
	
		if(curItemHandling == null) return;
		if (curItemHandling.Cost > PawManager.Instance.CurrentPaw)
		{
			notEnoughMoneyNotification.SetActive(true);
			return;
		}
		OnButtonBuyClick?.Invoke(curItemHandling);
		Close();
	}
	void ActiveSubSpine( bool isHead, CharacterScalePosSO listData)
	{
		CharScaleAndPos it = listData.ListCharScaleAndPos.Where(data => data.ID == curItemHandling.ID).First();

		subHead.skeletonDataAsset = Spine.SkeletonDataAsset;
		subHead.initialSkinName = "Head/Skin_1";
		subBody.skeletonDataAsset = Spine.SkeletonDataAsset;
		subBody.initialSkinName = "Body/Skin_1";
	
		subHead.Initialize(true);
		subBody.Initialize(true);

		subBody.gameObject.SetActive(isHead);
		subHead.gameObject.SetActive(!isHead);

		var transform = Spine.GetComponent<RectTransform>();
		transform.localScale = it.scale;
		transform.anchoredPosition = it.pos;
		var transformStaticHeadSpine = subHead.GetComponent<RectTransform>();
		transformStaticHeadSpine.localScale = it.scale;
		transformStaticHeadSpine.anchoredPosition = it.pos;
		var transformBodySpine = subBody.GetComponent<RectTransform>();
		transformBodySpine.localScale = it.scale;
		transformBodySpine.anchoredPosition = it.pos;
	}
	public void BuyBySuperMoney()
	{

		if (curItemHandling == null) return;
		if (curItemHandling.SuperCost > SuperMoneyManager.Instance.SuperMoney)
		{
			notEnoughMoneyNotification.SetActive(true);
			return;
		}
		OnButtonBuyBySuperMoneyClick?.Invoke(curItemHandling);
		Close();
	}
}
