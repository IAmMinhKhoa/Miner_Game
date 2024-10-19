using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using UI.Inventory;
using System.Linq;
public class ChangeCounterSecondBG : BaseState<InventoryItemType>
{
	BackGroundItemController bgList;
	BackGroundItem bgItem;
	
	public ChangeCounterSecondBG(BackGroundItemController bgList, BackGroundItem bgItem)
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
		bgItem.bg.Initialize(true);
		bgItem.secondBg.Initialize(true);

		int skinAmount = bgItem.secondBg.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();
		
		bgList.ClearListItem();
		bgList.Init(bgItem, skinAmount);
		currentSkinSelect = int.Parse(Counter.Instance.counterSkin.idSecondBg);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var counterSkin = Counter.Instance.counterSkin;
		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;

			var itemInfo = SkinManager.Instance.skinResource.skinSecondBgShaft[i];
			_item.SetItemInfor(i, itemInfo.desc, itemInfo.name, InventoryItemType.ShaftSecondBg);
			_item.bg.gameObject.SetActive(false);
			//ChangeSkin(_item.bg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));
			ChangeSkin(_item.secondBg, "Click_" + (i + 1));
		}

		SkeletonDataAsset skBgData = sourceSkins[InventoryItemType.CounterBg];
		SkeletonDataAsset skSecondBGData = sourceSkins[InventoryItemType.CounterSecondBg];

		var imgSelectedBg = bgList.imgSelectedBg;
		//imgSelectedBg.skeletonDataAsset = skBgData;
		//imgSelectedBg.Initialize(true);
		//ChangeSkin(imgSelectedBg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));
		
		imgSelectedBg.gameObject.SetActive(false);
		

		var secondBg = bgList.imgSelectedSecondBg;
		secondBg.skeletonDataAsset = skSecondBGData;
		secondBg.Initialize(true);
		ChangeSkin(secondBg, "Click_" + (int.Parse(counterSkin.idSecondBg) + 1));

		bgList.descSelectedBg.text = bgList.listItem[int.Parse(counterSkin.idBackGround)].desc;
		bgList.tileSelectedBg.text = bgList.listItem[int.Parse(counterSkin.idBackGround)].iName;

	}

	private void HandleConfirmButtonClick()
	{
		Counter.Instance.counterSkin.idSecondBg = currentSkinSelect.ToString();
		Counter.Instance.UpdateUI();
		Counter.Instance.OnUpdateCounterInventoryUI?.Invoke();
	}

	private void HandleItemClick(int index)
	{
		var secondBg = bgList.imgSelectedSecondBg;
		ChangeSkin(secondBg, "Click_" + (index + 1));
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
