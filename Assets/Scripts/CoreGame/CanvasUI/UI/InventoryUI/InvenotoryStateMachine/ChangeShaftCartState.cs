using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
public class ChangeShaftCartState : BaseState<InventoryItemType>
{
	readonly PopupOtherItemController itemController;
	public ChangeShaftCartState(PopupOtherItemController itemController)
	{
		this.itemController = itemController;
	}
	public override void Do()
	{
		
	}

	public override void Enter()
	{
		itemController.UnselectAllItem();
		itemController.UnactiveAll();
		itemController.title.text = "CHỌN XE ĐẨY Ở MỖI TẦNG";
		var shaft = ShaftManager.Instance.Shafts[0];
		var brewer = shaft.Brewers[0];
		var skinAmount = brewer.CartSkeletonAnimation.skeleton.Data.Skins;
		for (int i = 0; i < skinAmount.Count; i++)
		{
			itemController.itemsHandle[i].gameObject.SetActive(true);
			itemController.itemsHandle[i].ShowCart(i);
			var skinName = SkinManager.Instance.skinResource.skinCartShaft[i].name;
			itemController.itemsHandle[i].ChangItemInfo(skinName);
			itemController.itemsHandle[i].ItemClicked += ChangeSkin;
		}
		

	}

	private void ChangeSkin(Item item)
	{
		int index = itemController.itemsHandle.IndexOf(item);
		itemController.UnselectAllItem();
		itemController.itemsHandle[index].Selected();
		ShaftManager.Instance.Shafts[itemController.FloorIndex].shaftSkin.idCart = index.ToString();
		ShaftManager.Instance.Shafts[itemController.FloorIndex].UpdateUI();
		ShaftManager.Instance.OnUpdateShaftInventoryUI(itemController.FloorIndex);
	}
	public override void Exit()
	{
		var shaft = ShaftManager.Instance.Shafts[0];
		var brewer = shaft.Brewers[0];
		var skinAmount = brewer.CartSkeletonAnimation.skeleton.Data.Skins;
		for (int i = 0; i < skinAmount.Count; i++)
		{
			itemController.itemsHandle[i].ItemClicked -= ChangeSkin;
		}
	}

}
