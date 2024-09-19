using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine;
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
		
	}

	public override void Exit()
	{
		
	}

}
