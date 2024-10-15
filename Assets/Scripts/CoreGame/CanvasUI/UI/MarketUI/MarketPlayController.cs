using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using System.Linq;
using UnityEditor;

public class MarketPlayController : MonoBehaviour 
{
	
	GameObject lowContent;
	GameObject normalContent;
	GameObject superContent;
	[SerializeField]
	MarketPlayItem item;
	[SerializeField]
	MarketContentBox pnNoiThat;
	[SerializeField]
	MarketContentBox pnNhanVien;


	private void Start()
	{
		pnNoiThat.BoxIsEnable += SetContent;
		pnNhanVien.BoxIsEnable += SetContent;

	}
	List<MarketPlayItem> listItem;

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
	public void Initial(SkeletonDataAsset data, List<DataSkinBase> skinData)
	{
		SetDataItem(data);
		listItem ??= new();
		var lowSkinList = skinData.Where(it => it.quality == "low").ToList();
		var normalSkinList = skinData.Where(it => it.quality == "normal").ToList();
		var superSkinList = skinData.Where(it => it.quality == "super").ToList();
		

		for (int i = 0; i < lowSkinList.Count(); i++)
		{
			var instanceItem = Instantiate(item, lowContent.transform);
			instanceItem.ItemQuality =  MarketPlayItemQuality.Low;

			string skinName = "Icon_" + lowSkinList[i].id;
			var skin = instanceItem.SpineHandling.Skeleton.Data.FindSkin(skinName);
			if(skin != null)
			{
				instanceItem.SpineHandling.Skeleton.SetSkin(skin);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();
			}

			listItem.Add(instanceItem);
		}

		for (int i = 0;i < normalSkinList.Count(); i++)
		{
			var instanceItem = Instantiate(item, normalContent.transform);
			instanceItem.ItemQuality = MarketPlayItemQuality.Normal;

			string skinName = "Icon_" + lowSkinList[i].id;
			var skin = instanceItem.SpineHandling.Skeleton.Data.FindSkin(skinName);
			if (skin != null)
			{
				instanceItem.SpineHandling.Skeleton.SetSkin(skin);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();
			}

			listItem.Add(instanceItem);
		}

		for (int i = 0;i < superSkinList.Count(); i++)
		{
			var instanceItem = Instantiate(item, superContent.transform);
			instanceItem.ItemQuality = MarketPlayItemQuality.Super;

			string skinName = "Icon_" + lowSkinList[i].id;
			var skin = instanceItem.SpineHandling.Skeleton.Data.FindSkin(skinName);
			if (skin != null)
			{
				instanceItem.SpineHandling.Skeleton.SetSkin(skin);
				instanceItem.SpineHandling.Skeleton.SetSlotsToSetupPose();
			}

			listItem.Add(instanceItem);
		}
		UpdateCost();
	}

	public void UpdateCost()
	{

	}

	protected void ClearItem()
	{
		foreach(var item in listItem)
		{
			GameObject.Destroy(item.gameObject);
		}
		listItem.Clear();
	}

}
