using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using UI.Inventory;
using System.Linq;
public class ChangeShaftBG : BaseState<InventoryItemType>
{
	BackGroundItemController bgList;
	BackGroundItem bgItem;

	public ChangeShaftBG(BackGroundItemController bgList, BackGroundItem bgItem)
	{
		this.bgList = bgList;
		this.bgItem = bgItem;

	}
	private int currentSkinSelect;
	int currenFloor;
	public override void Do()
	{

	}

	public override void Enter()
	{
		//Cap nhat bottom Skin list
		var sourceSkins = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
		bgItem.bg.skeletonDataAsset = sourceSkins[InventoryItemType.ShaftBg];
		bgItem.secondBg.skeletonDataAsset = sourceSkins[InventoryItemType.ShaftSecondBg];
		bgItem.bg.Initialize(true);
		bgItem.secondBg.Initialize(true);
		currenFloor = bgList.Index;
		int skinAmount = bgItem.bg.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();
		bgList.ClearListItem();

		bgList.Init(bgItem, skinAmount);
		currentSkinSelect = int.Parse(ShaftManager.Instance.Shafts[currenFloor].shaftSkin.idBackGround);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var skinData = ShaftManager.Instance.Shafts[currenFloor].shaftSkin;
		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;
			var itemInfo = SkinManager.Instance.skinResource.skinBgShaft[i];
			_item.SetItemInfor(i, itemInfo.desc, itemInfo.name, InventoryItemType.ShaftBg);
			_item.bg.gameObject.SetActive(true);
			ChangeSkin(_item.bg, "Click_" + (i + 1));
			ChangeSkin(_item.secondBg, "Click_" + (int.Parse(skinData.idSecondBg) + 1));
		}
		//Cap nhat top skinList
	
		SkeletonDataAsset skBgData = sourceSkins[InventoryItemType.ShaftBg];
		SkeletonDataAsset skSecondBGData = sourceSkins[InventoryItemType.ShaftSecondBg];

		var imgSelectedBg = bgList.imgSelectedBg;
		if (!imgSelectedBg.gameObject.activeInHierarchy)
		{
			imgSelectedBg.gameObject.SetActive(true);
		}
		imgSelectedBg.skeletonDataAsset = skBgData;
		imgSelectedBg.Initialize(true);
		ChangeSkin(imgSelectedBg, "Click_" + (int.Parse(skinData.idBackGround) + 1));

		var secondBg = bgList.imgSelectedSecondBg;
		secondBg.skeletonDataAsset = skSecondBGData;
		secondBg.Initialize(true);
		ChangeSkin(secondBg, "Click_" + (int.Parse(skinData.idSecondBg) + 1));

		bgList.descSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].desc;
		bgList.tileSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].iName;

	}

	private void HandleConfirmButtonClick()
	{

		ShaftManager.Instance.Shafts[currenFloor].shaftSkin.idBackGround = currentSkinSelect.ToString();
		ShaftManager.Instance.Shafts[currenFloor].UpdateUI();
		ShaftManager.Instance.OnUpdateShaftInventoryUI?.Invoke(currenFloor);
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
