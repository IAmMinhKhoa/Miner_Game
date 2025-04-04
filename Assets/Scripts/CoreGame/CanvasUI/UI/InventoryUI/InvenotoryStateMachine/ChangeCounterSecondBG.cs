using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine.Unity;
using System.Linq;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
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
	int currenFloor;
	public override void Do()
	{

	}

	public override void Enter()
	{
		//Cap nhat bottom Skin list
		var sourceSkins = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
		bgItem.bg.skeletonDataAsset = sourceSkins[InventoryItemType.CounterBg];
		bgItem.secondBg.skeletonDataAsset = sourceSkins[InventoryItemType.ShaftSecondBg];
		bgItem.bg.Initialize(true);
		bgItem.secondBg.Initialize(true);

		bgList.ClearListItem();

		var listSkin = SkinManager.Instance.GetListDataSkinBases(InventoryItemType.ShaftSecondBg);

		bgList.Init(bgItem, listSkin.Count);
		currentSkinSelect = int.Parse(Counter.Instance.counterSkin.idBackGround);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var skinData = Counter.Instance.counterSkin;

		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;

			var itemInfo = listSkin[i];
			_item.SetItemInfor(int.Parse(itemInfo.id), itemInfo.name.GetContent(ManagersController.Instance.localSelected), itemInfo.desc.GetContent(ManagersController.Instance.localSelected), InventoryItemType.ShaftSecondBg);
			_item.bg.gameObject.SetActive(false);
			//ChangeSkin(_item.bg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));
			ChangeSkin(_item.secondBg, "Click_" + itemInfo.id);
		}

		SkeletonDataAsset skBgData = sourceSkins[InventoryItemType.CounterBg];
		SkeletonDataAsset skSecondBGData = sourceSkins[InventoryItemType.ShaftSecondBg];

		var imgSelectedBg = bgList.imgSelectedBg;
		//imgSelectedBg.skeletonDataAsset = skBgData;
		//imgSelectedBg.Initialize(true);
		//ChangeSkin(imgSelectedBg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));

		imgSelectedBg.gameObject.SetActive(false);

		var secondBg = bgList.imgSelectedSecondBg;
		secondBg.skeletonDataAsset = skSecondBGData;
		secondBg.Initialize(true);
		ChangeSkin(secondBg, "Click_" + (int.Parse(skinData.idSecondBg) + 1));

		bgList.descSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].desc;
		bgList.tileSelectedBg.text = bgList.listItem[int.Parse(skinData.idBackGround)].iName;

	}

	private void HandleConfirmButtonClick()
	{
		Counter.Instance.counterSkin.idSecondBg = currentSkinSelect.ToString();
		Counter.Instance.UpdateUI();
		Counter.Instance.OnUpdateCounterInventoryUI?.Invoke();
	}

	private void HandleItemClick(int index)
	{

		var imgSelectedBg = bgList.imgSelectedSecondBg;
		ChangeSkin(imgSelectedBg, "Click_" + (index ));
		bgList.descSelectedBg.text = bgList.listItem[index].desc;
		bgList.tileSelectedBg.text = bgList.listItem[index].iName;
		currentSkinSelect = index - 1;
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
