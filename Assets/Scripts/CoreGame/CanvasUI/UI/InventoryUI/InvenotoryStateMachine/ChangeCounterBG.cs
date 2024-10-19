using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine.Unity;
using System;
using System.Linq;
public class ChangeCounterBG : BaseState<InventoryItemType>
{
	BackGroundItemController bgList;
	BackGroundItem bgItem;

	public ChangeCounterBG(BackGroundItemController bgList, BackGroundItem bgItem)
	{
		this.bgList = bgList;
		this.bgItem = bgItem;
	}
	private int currentSkinSelect;
	public override void Do()
	{

	}

	public override void Enter()
	{
		var sourceSkins = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
		bgItem.bg.skeletonDataAsset = sourceSkins[InventoryItemType.CounterBg];
		bgItem.secondBg.skeletonDataAsset = sourceSkins[InventoryItemType.CounterSecondBg];
		int skinAmount = bgItem.bg.skeletonDataAsset.GetSkeletonData(true).Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();
		bgItem.bg.Initialize(true);
		bgItem.secondBg.Initialize(true);
		bgList.ClearListItem();
		bgList.Init(bgItem, skinAmount);
		currentSkinSelect = int.Parse(Counter.Instance.counterSkin.idBackGround);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var counterSkin = Counter.Instance.counterSkin;

		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;
			

			var itemInfo = SkinManager.Instance.skinResource.skinBgCounter[i];
			_item.SetItemInfor(i, itemInfo.desc, itemInfo.name, InventoryItemType.CounterBg);
			_item.bg.gameObject.SetActive(true);
			ChangeSkin(_item.bg, "Click_" + (i + 1));
			ChangeSkin(_item.secondBg, "Click_" + (int.Parse(counterSkin.idSecondBg) + 1));
		}

		SkeletonDataAsset skBgData = sourceSkins[InventoryItemType.CounterBg];
		SkeletonDataAsset skSecondBGData = sourceSkins[InventoryItemType.CounterSecondBg];

		var imgSelectedBg = bgList.imgSelectedBg;
		if (!imgSelectedBg.gameObject.activeInHierarchy)
		{
			imgSelectedBg.gameObject.SetActive(true);
		}

		imgSelectedBg.skeletonDataAsset = skBgData;
		imgSelectedBg.Initialize(true);
		ChangeSkin(imgSelectedBg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));

		var secondBg = bgList.imgSelectedSecondBg;
		secondBg.skeletonDataAsset = skSecondBGData;
		secondBg.Initialize(true);
		ChangeSkin(secondBg, "Click_" + (int.Parse(counterSkin.idSecondBg) + 1));

		bgList.descSelectedBg.text = bgList.listItem[int.Parse(counterSkin.idBackGround)].desc;
		bgList.tileSelectedBg.text = bgList.listItem[int.Parse(counterSkin.idBackGround)].iName;

	}

	private void HandleConfirmButtonClick()
	{
		Counter.Instance.counterSkin.idBackGround = currentSkinSelect.ToString();
		Counter.Instance.UpdateUI();
		Counter.Instance.OnUpdateCounterInventoryUI?.Invoke();
	}

	private void HandleItemClick(int index)
	{
		var imgSelectedBg = bgList.imgSelectedBg;
		ChangeSkin(imgSelectedBg, "Click_" + (index + 1));
		bgList.descSelectedBg.text = bgList.listItem[index].desc;
		bgList.tileSelectedBg.text = bgList.listItem[index].iName;
		currentSkinSelect = index;
	}

	private void ChangeSkin(SkeletonGraphic target, string skinName)
	{
		target.Skeleton.SetSkin(skinName);
		target.Skeleton.SetSlotsToSetupPose();
		target.UpdateMesh();
	}

	public override void Exit()
	{
		bgList.OnConfirmButtonClick -= HandleConfirmButtonClick;
	}
}
