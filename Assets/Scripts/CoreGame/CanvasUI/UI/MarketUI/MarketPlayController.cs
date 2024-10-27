using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using System.Linq;
using UnityEditor;
using System;

public class MarketPlayController : MonoBehaviour 
{

	[Header("Panel")]
	[SerializeField]
	MarketPlayItem item;
	[SerializeField]
	MarketContentBox pnNoiThat;
	[SerializeField]
	MarketContentBox pnNhanVien;

	[SerializeField]
	InfoMarketItemUIHandle nhanVien;
	[SerializeField]
	InfoMarketItemUIHandle noiThatLong;
	[SerializeField]
	InfoMarketItemUIHandle noiThatShort;

	[Header("Toggle")]
	[SerializeField]
	MarketToggleHandle[] listToggle;
	[SerializeField]
	MarketToggleHandle[] listHeadCharToggle;
	[SerializeField]
	MarketToggleHandle[] listBodyCharToggle;

	[Header("Size Item SO")]
	[SerializeField]
	ResizeMarketItem sizeItem;
	[SerializeField]
	CharacterScalePosSO shaftHeadScaleSO;
	[SerializeField]
	CharacterScalePosSO shaftBodyScaleSO;
	[SerializeField]
	CharacterScalePosSO elevatorHeadScaleSO;
	[SerializeField]
	CharacterScalePosSO elevatorBodyScaleSO;
	InfoMarketItemUIHandle curMarketItemHandle;

	GameObject lowContent;
	GameObject normalContent;
	GameObject superContent;

	InventoryItemType currentItemShowing;

	List<MarketPlayItem> listItem;

	public ContentFitterRefresh contentRefreshMainMenu;
	public ContentFitterRefresh contentRefreshStore;


	private void Start()
	{
		pnNoiThat.BoxIsEnable += SetContent;
		pnNhanVien.BoxIsEnable += SetContent;
		nhanVien.OnButtonBuyClick += BuyItem;
		noiThatLong.OnButtonBuyClick += BuyItem;
		nhanVien.OnButtonBuyBySuperMoneyClick += BuyBySuperMoneyItem;
		noiThatLong.OnButtonBuyBySuperMoneyClick += BuyBySuperMoneyItem;
		foreach (var item in listToggle)
		{
			item.OnTabulationClick += HandleTabItemClick;
			
		}
		foreach (var item in listHeadCharToggle)
		{
			item.OnTabulationClick += HandleListHead;
		
		}
		foreach (var item in listBodyCharToggle)
		{
			item.OnTabulationClick += HanldeListBody ;
	
		}
		contentRefreshMainMenu.RefreshContentFitters();
	}

	
	private void HanldeListBody(InventoryItemType type)
	{
		currentItemShowing = type;
		curMarketItemHandle = nhanVien;
		ClearItem();
		var skeletonData = SkinManager.Instance.SkinGameDataAsset.SkinGameData[type];
		var skinData = SkinManager.Instance.InfoSkinGame[type];
		Initial(skeletonData, skinData, "Body/Skin_", "Head/Skin_");
		if (type == InventoryItemType.ElevatorCharacterBody)
		{
			ResizeListItem(elevatorBodyScaleSO);
		}
		else
		{
			ResizeListItem(shaftBodyScaleSO);
		}


	}

	private void HandleListHead(InventoryItemType type)
	{
		currentItemShowing = type;
		curMarketItemHandle = nhanVien;
		ClearItem();
		var skeletonData = SkinManager.Instance.SkinGameDataAsset.SkinGameData[type];
		var skinData = SkinManager.Instance.InfoSkinGame[type];
		Initial(skeletonData, skinData, "Head/Skin_", "Body/Skin_");
		if(type == InventoryItemType.ElevatorCharacter)
		{
			ResizeListItem(elevatorHeadScaleSO, true);
		}
		else
		{
			ResizeListItem(shaftHeadScaleSO, true);
		}


	}
	void ResizeListItem(CharacterScalePosSO sO, bool isHead = false)
	{
		foreach(var item in listItem)
		{
		

			int indexA = item.SpineHandling.transform.GetSiblingIndex();
			int indexB = item.SecondSpine.transform.GetSiblingIndex();

			if (indexA > indexB && !isHead)
			{
				item.SpineHandling.transform.SetSiblingIndex(indexB);
				item.SecondSpine.transform.SetSiblingIndex(indexB + 1);
			}

			if (indexA < indexB && isHead)
			{
				item.SpineHandling.transform.SetSiblingIndex(indexA);
				item.SecondSpine.transform.SetSiblingIndex(indexA + 1);
			}
			item.ActiveSecondSpine();
			foreach (var it in sO.ListCharScaleAndPos)
			{
				if(item.ID == it.ID)
				{
					var transform = item.SpineHandling.GetComponent<RectTransform>();
					transform.localScale = it.scale;
					transform.anchoredPosition = it.pos;
					var transformSecondSpine = item.SecondSpine.GetComponent<RectTransform>();
					transformSecondSpine.localScale = it.scale;
					transformSecondSpine.anchoredPosition = it.pos;
				}
			}
		}
	}
	private void HandleTabItemClick(InventoryItemType type)
	{
		currentItemShowing = type;
		switch (type)
		{
			case InventoryItemType.CounterBg:
			case InventoryItemType.ShaftBg:
			case InventoryItemType.ShaftSecondBg:
			case InventoryItemType.CounterSecondBg:
				curMarketItemHandle = noiThatLong;
				break;
			default:
				curMarketItemHandle = noiThatShort;
				break;
		}
		ClearItem();
		var skeletonData = SkinManager.Instance.SkinGameDataAsset.SkinGameData[type];
		var skinData = SkinManager.Instance.InfoSkinGame[type];

		Initial(skeletonData, skinData, "Icon_");
		//Debug.Log("Market Item Click:" + type);
		contentRefreshStore.RefreshContentFitters();
	}


	public void SetContent(MarketContentBox box)
	{
		lowContent = box.LowContent;
		normalContent = box.NormalContent;
		superContent = box.SuperContent;
		
	}

	void SetDataItem(SkeletonDataAsset data)
	{
		item.SpineHandling.startingAnimation =
			data.GetSkeletonData(true).FindAnimation("Icon") != null
			? "Icon"
			: "";
		item.SpineHandling.skeletonDataAsset = data;
		item.SpineHandling.initialSkinName = data.GetSkeletonData(true).Skins.Items[0].Name;
		item.SecondSpine.skeletonDataAsset = data;
		item.SecondSpine.initialSkinName = data.GetSkeletonData(true).Skins.Items[0].Name;
		item.SpineHandling.Initialize(true);
		item.SecondSpine.Initialize(true);
	}

	public void Initial(SkeletonDataAsset data, List<DataSkinBase> skinData, string startStringSkinName, string startStringSeconSpineSkinName = "")
	{
		SetDataItem(data);
		listItem ??= new();
		var lowSkinList = skinData.Where(it => it.quality == "low").ToList();
		var normalSkinList = skinData.Where(it => it.quality == "normal").ToList();
		var superSkinList = skinData.Where(it => it.quality == "super").ToList();
		

		for (int i = 0; i < lowSkinList.Count(); i++)
		{
			if (lowSkinList[i].id == "1") continue;
			string skinName = startStringSkinName + lowSkinList[i].id;
			var skin = data.GetSkeletonData(true).FindSkin(skinName);
			if (skin != null)
			{
				var instanceItem = Instantiate(item, lowContent.transform);
				instanceItem.ItemQuality = MarketPlayItemQuality.low;
				instanceItem.SpineHandling.Skeleton.SetSkin(skinName);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();

				if(startStringSeconSpineSkinName != "")
				{
					instanceItem.SecondSpine.Skeleton.SetSkin(startStringSeconSpineSkinName + 1);
					instanceItem.SecondSpine.Skeleton.SetSlotsToSetupPose();
				}

				instanceItem.ID = lowSkinList[i].id;
				listItem.Add(instanceItem);
			}
		}

		for (int i = 0;i < normalSkinList.Count(); i++)
		{
			string skinName = startStringSkinName + normalSkinList[i].id;
			var skin = data.GetSkeletonData(true).FindSkin(skinName);

			if (skin != null)
			{
				var instanceItem = Instantiate(item, normalContent.transform);
				instanceItem.ItemQuality = MarketPlayItemQuality.normal;
				instanceItem.SpineHandling.Skeleton.SetSkin(skinName);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();


				if (startStringSeconSpineSkinName != "")
				{
					instanceItem.SecondSpine.Skeleton.SetSkin(startStringSeconSpineSkinName + 1);
					instanceItem.SecondSpine.Skeleton.SetSlotsToSetupPose();
				}


				instanceItem.ID = normalSkinList[i].id;
				listItem.Add(instanceItem);
			}
		}

		for (int i = 0;i < superSkinList.Count(); i++)
		{
			
			string skinName = startStringSkinName + superSkinList[i].id;
			var skin = data.GetSkeletonData(true).FindSkin(skinName);

			if (skin != null)
			{
				var instanceItem = Instantiate(item, superContent.transform);
				instanceItem.ItemQuality = MarketPlayItemQuality.super;
				instanceItem.SpineHandling.Skeleton.SetSkin(skinName);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();

				if (startStringSeconSpineSkinName != "")
				{
					instanceItem.SecondSpine.Skeleton.SetSkin(startStringSeconSpineSkinName + 1);
					instanceItem.SecondSpine.Skeleton.SetSlotsToSetupPose();
				}
				instanceItem.ID = superSkinList[i].id;
				listItem.Add(instanceItem);
			}
		}
		foreach(var it in listItem)
		{
			it.OnItemIsBought += ShowItemInfo;
			if(currentItemShowing == InventoryItemType.ShaftWaitTable)
			{
				it.SpineHandling.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -17);
			}
		}
		UpdateCost();

	}

	public void UpdateCost()
	{
		var skinManager = SkinManager.Instance;


		Dictionary<MarketPlayItemQuality, int> amountSkinBought = new();
		HashSet<string> skingBought = new(skinManager.ItemBought[currentItemShowing]);

		foreach (var it in listItem)
		{
			amountSkinBought[it.ItemQuality] = 0;
		}

		foreach (var item in listItem.Where(it => skingBought.Contains(it.ID)))
		{
			amountSkinBought[item.ItemQuality] += 1;
			item.ItemIsBought();
			item.OnItemIsBought -= ShowItemInfo;
		}
		foreach (var it in listItem)
		{
			string cost = skinManager.InfoSkinGame[currentItemShowing].Where(item => item.quality == it.ItemQuality.ToString()).First().cost;
			it.Cost = double.Parse(cost, System.Globalization.CultureInfo.InvariantCulture);
		}

		

		foreach(var skinQuantity in amountSkinBought)
		{
			string cost = skinManager.InfoSkinGame[currentItemShowing].Where(item => item.quality == skinQuantity.Key.ToString()).First().cost;
			
			foreach (var it in listItem)
			{
				if(it.ItemQuality == skinQuantity.Key)
				{
					it.Cost = double.Parse(cost, System.Globalization.CultureInfo.InvariantCulture) * Math.Pow(100, skinQuantity.Value);
				}
			}
		}
		
	}
	private void ShowItemInfo(MarketPlayItem it)
	{
		curMarketItemHandle.Open();
		curMarketItemHandle.Init(sizeItem.itemSizeDic[currentItemShowing], it);
	}
	private void BuyItem(MarketPlayItem it)
	{
		SkinManager.Instance.BuyNewSkin(currentItemShowing ,it.ID);
		PawManager.Instance.RemovePaw(it.Cost); 
		UpdateCost();
	}
	private void BuyBySuperMoneyItem(MarketPlayItem item)
	{
		SkinManager.Instance.BuyNewSkin(currentItemShowing, item.ID);
		SuperMoneyManager.Instance.RemoveMoney((float)(item.SuperCost));
		UpdateCost();
	}

	protected void ClearItem()
	{
		if (listItem == null) return;
		foreach(var item in listItem)
		{
			Destroy(item.gameObject);
		}
		listItem.Clear();

	}
	public static decimal DecimalPow(decimal baseValue, int exponent)
	{
		decimal result = 1;
		for (int i = 0; i < exponent; i++)
		{
			result *= baseValue;
		}
		return result;
	}
}
