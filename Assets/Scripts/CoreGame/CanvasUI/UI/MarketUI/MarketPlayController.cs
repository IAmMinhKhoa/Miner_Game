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
	
	
	[SerializeField]
	MarketPlayItem item;
	[SerializeField]
	MarketContentBox pnNoiThat;
	[SerializeField]
	MarketContentBox pnNhanVien;
	[SerializeField]
	MarketToggleHandle[] listToggle;
	[SerializeField]
	MarketToggleHandle[] listHeadCharToggle;
	[SerializeField]
	MarketToggleHandle[] listBodyCharToggle;

	[SerializeField]
	InfoMarketItemUIHandle nhanVien;
	[SerializeField]
	InfoMarketItemUIHandle noiThat;

	[SerializeField]
	ResizeMarketItem sizeItem;

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
		noiThat.OnButtonBuyClick += BuyItem;
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
		Initial(skeletonData, skinData, "Body/Skin_");
		if (type == InventoryItemType.ElevatorCharacterBody)
		{
			ResizeListItem(new Vector3(0.3f, 0.3f, 1f), new Vector2(0f, -46f));
		}
		else
		{
			ResizeListItem(new Vector3(0.45f, 0.45f, 1f), new Vector2(10f, -51f));
		}


	}

	private void HandleListHead(InventoryItemType type)
	{
		currentItemShowing = type;
		curMarketItemHandle = nhanVien;
		ClearItem();
		var skeletonData = SkinManager.Instance.SkinGameDataAsset.SkinGameData[type];
		var skinData = SkinManager.Instance.InfoSkinGame[type];
		Initial(skeletonData, skinData, "Head/Skin_");
		if(type == InventoryItemType.ElevatorCharacter)
		{
			ResizeListItem(new Vector3(0.3f, 0.3f, 1f), new Vector2(0f, -127f));
		}
		else
		{
			ResizeListItem(new Vector3(0.45f, 0.45f, 1f), new Vector2(0f, -130f));
		}


	}
	void ResizeListItem(Vector3 scale, Vector2 pos)
	{
		foreach(var item in listItem)
		{
			var transform = item.SpineHandling.GetComponent<RectTransform>();
			transform.localScale = scale;
			transform.anchoredPosition = pos;
		}
	}
	private void HandleTabItemClick(InventoryItemType type)
	{
		currentItemShowing = type;
		curMarketItemHandle = noiThat;
		ClearItem();
		var skeletonData = SkinManager.Instance.SkinGameDataAsset.SkinGameData[type];
		var skinData = SkinManager.Instance.InfoSkinGame[type];

		item.SpineHandling.startingAnimation = type == InventoryItemType.ShaftWaitTable ? "Icon" : "";
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
		item.SpineHandling.skeletonDataAsset = data;
		item.SpineHandling.initialSkinName = data.GetSkeletonData(true).Skins.Items[0].Name;
		item.SpineHandling.Initialize(true);
	}

	public void Initial(SkeletonDataAsset data, List<DataSkinBase> skinData, string startStringSkinName)
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
				instanceItem.ID = superSkinList[i].id;
				listItem.Add(instanceItem);
			}
		}
		foreach(var it in listItem)
		{
			it.OnItemIsBought += ShowItemInfo;
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

	protected void ClearItem()
	{
		if (listItem == null) return;
		foreach(var item in listItem)
		{
			GameObject.Destroy(item.gameObject);
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
