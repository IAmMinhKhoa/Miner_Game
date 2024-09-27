using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using UI.Inventory;
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

		
		int skinAmount = Counter.Instance.GetComponent<CounterUI>().m_secondBG.Skeleton.Data.Skins.Count;
		Debug.Log(skinAmount);
		bgList.ClearListItem();
		bgList.Init(bgItem, skinAmount / 3);
		currentSkinSelect = int.Parse(Counter.Instance.counterSkin.idSecondBg);
		bgList.OnConfirmButtonClick += HandleConfirmButtonClick;

		var counterSkin = Counter.Instance.counterSkin;
		for (int i = 0; i < bgList.listItem.Count; i++)
		{
			var _item = bgList.listItem[i];
			_item.OnBackGroundItemClick += HandleItemClick;

			var itemInfo = SkinManager.Instance.skinResource.skinSecondBgShaft[i];
			_item.SetItemInfor(i, itemInfo.desc, itemInfo.name);
			ChangeSkin(_item.bg, "Click_" + (int.Parse(counterSkin.idBackGround) + 1));
			ChangeSkin(_item.secondBg, "Click_" + (i + 1));
		}

		CounterUI Ui = Counter.Instance.GetComponent<CounterUI>();
		SkeletonDataAsset skBgData = Ui.m_bgCounter.skeletonDataAsset;
		SkeletonDataAsset skSecondBGData = Ui.m_secondBG.skeletonDataAsset;

		var imgSelectedBg = bgList.imgSelectedBg;
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
