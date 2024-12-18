using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySpecialItemUI : MonoBehaviour
{
	[SerializeField] Button closeUI;
	[SerializeField] Image buyButtonIMG;
	[SerializeField] Button buyButton;
	[SerializeField] TextMeshProUGUI currentCoin;
	[SerializeField] SkeletonGraphic skeletonGraphic;
	[SerializeField] SkeletonGraphic head;
	[SerializeField] SkeletonGraphic body;
	[SerializeField] TextMeshProUGUI nameIT;
	[SerializeField] TextMeshProUGUI descIT;
	[SerializeField] RectTransform bgR;
	public event Action<BuyableGachaItem> OnButtonBuyClick;

	BuyableGachaItem currentItem;

	private void Awake()
	{
		closeUI.onClick.AddListener(CloseUI);
		buyButton.onClick.AddListener(BuyItem);
	}
	public void Initiallize(BuyableGachaItem item, bool isBuyable, float coin)
	{

		gameObject.SetActive(true);

		bgR.DOScale(1.2f, 0.2f / 2)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				bgR.DOScale(Vector3.one, 0.2f / 2).SetEase(Ease.InQuad);
			});

		currentItem = item;
		Color color = buyButtonIMG.color;
		color.a = isBuyable ? 0 : 0.75f;
		buyButton.interactable = isBuyable;
		buyButtonIMG.color = color;
		currentCoin.text = coin + "";

		var itemInfoJson = SkinManager.Instance.InfoSkinGame[item.InfoItem.type].Where(i => i.id == item.InfoItem.skinGachaInfor.ID).First();


		var lg = ManagersController.Instance.localSelected;

		nameIT.text = itemInfoJson.name.GetContent(lg);
		descIT.text = itemInfoJson.desc.GetContent(lg);

		switch(item.InfoItem.type)
		{
			case(InventoryItemType.ShaftCharacterBody):
			case(InventoryItemType.ElevatorCharacter):
			case(InventoryItemType.ShaftCharacter):
			case(InventoryItemType.ElevatorCharacterBody):
				break;
			default:
				Initinterior(item.InfoItem);
				break;
		}
	}
	void BuyItem()
	{
		OnButtonBuyClick?.Invoke(currentItem);
		CloseUI();
	}
	private void Initinterior( GachaItemInfor InfoItem)
	{
		
		head.gameObject.SetActive(false);
		skeletonGraphic.gameObject.SetActive(true);
		body.gameObject.SetActive(false);
		SkeletonDataAsset skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InfoItem.type];

		skeletonGraphic.skeletonDataAsset = skeletonDataAsset;

		string skinName = "Icon_" + InfoItem.skinGachaInfor.ID;
		skeletonGraphic.initialSkinName = skinName;

		skeletonGraphic.Initialize(true);

		var iconAnimation = skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation("Icon");

		if (iconAnimation != null)
		{
			skeletonGraphic.AnimationState.SetAnimation(0, "Icon", false);
		}
		else
		{
			skeletonGraphic.AnimationState.ClearTrack(0);
		}

		var spine = skeletonGraphic.GetComponent<RectTransform>();
		spine.localScale = InfoItem.skinGachaInfor.Scale;
		spine.anchoredPosition = InfoItem.skinGachaInfor.Positon;
	}
	void CloseUI()
	{
		gameObject.SetActive(false);
	}

	//#region AnimateUI
	//[Button]
	//public void FadeInContainer()
	//{
		
	//	Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
	//	gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
	//	gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);


	//}
	//[Button]
	//public void FadeOutContainer()
	//{
	//	Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
	//	gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
	//	{
			
	//	});

	//}
	//#endregion
}
