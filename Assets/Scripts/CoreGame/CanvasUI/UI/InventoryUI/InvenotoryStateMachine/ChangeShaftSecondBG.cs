using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine.Unity;
using System.Linq;
public class ChangeShaftSecondBG : BaseState<InventoryItemType> 
{
	BackGroundItemController bgList;
	BackGroundItem bgItem;

	public ChangeShaftSecondBG(BackGroundItemController bgList, BackGroundItem bgItem)
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
		
		int skinAmount = bgItem.secondBg.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();

		currenFloor = bgList.Index;
		bgList.ClearListItem();
		bgList.Init(bgItem, skinAmount);
		currentSkinSelect = int.Parse(ShaftManager.Instance.Shafts[currenFloor].shaftSkin.idSecondBg);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var skinData = ShaftManager.Instance.Shafts[currenFloor].shaftSkin;
		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;
			var itemInfo = SkinManager.Instance.skinResource.skinBgShaft[i];
			_item.SetItemInfor(i, itemInfo.desc, itemInfo.name);
			_item.bg.gameObject.SetActive(false);
			//ChangeSkin(_item.bg, "Click_" + (int.Parse(skinData.idBackGround) + 1));
			ChangeSkin(_item.secondBg, "Click_" + (i + 1));
		}
		//Cap nhat top skinList
		ShaftUI Ui = ShaftManager.Instance.Shaft.GetComponent<ShaftUI>();
		SkeletonDataAsset skBgData = Ui.BG.skeletonDataAsset;
		SkeletonDataAsset skSecondBGData = Ui.SecondBG.skeletonDataAsset;

		var imgSelectedBg = bgList.imgSelectedBg;
	
		imgSelectedBg.gameObject.SetActive(false);
		

		//imgSelectedBg.skeletonDataAsset = skBgData;
		//imgSelectedBg.Initialize(true);
		//ChangeSkin(imgSelectedBg, "Click_" + (int.Parse(skinData.idBackGround) + 1));

		var secondBg = bgList.imgSelectedSecondBg;
		secondBg.skeletonDataAsset = skSecondBGData;
		secondBg.Initialize(true);
		ChangeSkin(secondBg, "Click_" + (int.Parse(skinData.idSecondBg) + 1));

		bgList.descSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].desc;
		bgList.tileSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].iName;

	}

	private void HandleConfirmButtonClick()
	{
		ShaftManager.Instance.Shafts[currenFloor].shaftSkin.idSecondBg = currentSkinSelect.ToString();
		ShaftManager.Instance.Shafts[currenFloor].UpdateUI();
		ShaftManager.Instance.OnUpdateShaftInventoryUI?.Invoke(currenFloor);
	}

	private void HandleItemClick(int index)
	{
		var imgSelectedBg = bgList.imgSelectedSecondBg;
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
