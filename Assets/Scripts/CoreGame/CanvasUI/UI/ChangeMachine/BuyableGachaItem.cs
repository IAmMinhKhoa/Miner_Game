
using NOOD.Sound;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyableGachaItem : MonoBehaviour
{
	[SerializeField] GameObject bought;
	[SerializeField] GameObject buyableItem;
	[SerializeField] SkeletonGraphic skeletonGraphic;
	[SerializeField] SkeletonGraphic head;
	[SerializeField] SkeletonGraphic body;
	[SerializeField] Button buyItemClick;

	public event Action<BuyableGachaItem> buyItemClicked;
	public GachaItemInfor InfoItem {get; set;}
	public float Itemprice { get; } = 1000;

	private void Awake()
	{
		buyItemClick.onClick.AddListener(BuyThisItem);
	}

	private void BuyThisItem()
	{
		SoundManager.PlaySound(SoundEnum.retroreward1);
		buyItemClicked?.Invoke(this);
	}

	public void ItemBougth(bool isBoght)
	{
		buyItemClick.interactable = !isBoght;
		bought.SetActive(isBoght);
		buyableItem.SetActive(!isBoght);
	}
	public void Init(string skinName)
	{
		head.gameObject.SetActive(false);
		body.gameObject.SetActive(false);
		SkeletonDataAsset skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InfoItem.type];
		skeletonGraphic.skeletonDataAsset = skeletonDataAsset;

		if (skeletonGraphic.Skeleton.Data.FindSkin(skinName) == null) return;

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

		ItemBougth(SkinManager.Instance.ItemBought[InfoItem.type].IndexOf(InfoItem.skinGachaInfor.ID) != -1) ;

		var spine = skeletonGraphic.GetComponent<RectTransform>();
		spine.localScale = InfoItem.skinGachaInfor.Scale;
		spine.anchoredPosition = InfoItem.skinGachaInfor.Positon;
	}

	public void InitStaff()
	{
		skeletonGraphic.gameObject.SetActive(false);
		head.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InfoItem.type];
		head.initialSkinName = "Head/Skin_" + InfoItem.skinGachaInfor.ID;
		head.Initialize(true);

		var spineHead = head.GetComponent<RectTransform>();
		spineHead.localScale = InfoItem.skinGachaInfor.Scale;
		spineHead.anchoredPosition = InfoItem.skinGachaInfor.Positon;

		body.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InfoItem.type];
		body.initialSkinName = "Body/Skin_" + InfoItem.skinGachaInfor.ID;
		body.Initialize(true);

		ItemBougth(SkinManager.Instance.ItemBought[InfoItem.type].IndexOf(InfoItem.skinGachaInfor.ID) != -1);

		var spineBody = body.GetComponent<RectTransform>();
		spineBody.localScale = InfoItem.skinGachaInfor.Scale;
		spineBody.anchoredPosition = InfoItem.skinGachaInfor.Positon;
	}
}
