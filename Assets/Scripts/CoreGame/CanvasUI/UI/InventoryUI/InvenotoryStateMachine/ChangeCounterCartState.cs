using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using UI.Inventory.PopupOtherItem;
public class ChangeCounterCartState : BaseState<InventoryItemType> 
{
	readonly PopupOtherItemController itemController;
	public ChangeCounterCartState(PopupOtherItemController itemController)
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
		var skinAmount = Counter.Instance.Transporters[^1].CartSkeletonAnimation.skeleton.Data.Skins;
		for (int i = 0; i < skinAmount.Count; i++)
		{
			itemController.itemsHandle[i].gameObject.SetActive(true);
			itemController.itemsHandle[i].ItemClicked += ChangeSkin;
		}
	}
	public override void Exit()
	{
		var skinAmount = Counter.Instance.Transporters[^1].CartSkeletonAnimation.skeleton.Data.Skins;
		for (int i = 0; i < skinAmount.Count; i++)
		{
			itemController.itemsHandle[i].ItemClicked -= ChangeSkin;
		}
	}
	private void ChangeSkin(Item item)
	{
		int index = itemController.itemsHandle.IndexOf(item);
		itemController.UnselectAllItem();
		itemController.itemsHandle[index].Selected();
		Counter.Instance.counterSkin.idCart = index.ToString();
		Counter.Instance.UpdateUI();
	}
	
}
