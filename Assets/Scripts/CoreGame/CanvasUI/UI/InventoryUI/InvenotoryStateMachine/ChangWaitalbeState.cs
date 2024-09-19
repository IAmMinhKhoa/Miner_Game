using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using UI.Inventory.PopupOtherItem;
public class ChangWaitalbeState : BaseState<InventoryItemType>
{
	readonly PopupOtherItemController itemController;
	readonly List<DataSkinImage> skins = SkinManager.Instance.skinResource.skinWaitTable;

	public ChangWaitalbeState(PopupOtherItemController itemController)
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
		for (int i = 0; i < skins.Count; i++)
		{
			var item = itemController.itemsHandle[i];
			item.gameObject.SetActive(true);
			item.ChangItemInfo(skins[i].sprite, skins[i].name);
			item.ItemClicked += ChangeSkin;
		}
	}
	private void ChangeSkin(Item item) 
	{
		int index = itemController.itemsHandle.IndexOf(item);
		itemController.UnselectAllItem();
		itemController.itemsHandle[index].Selected();
		ShaftManager.Instance.Shafts[itemController.FloorIndex].shaftSkin.idWaitTable = index.ToString();
		ShaftManager.Instance.Shafts[itemController.FloorIndex].UpdateUI();
		ShaftManager.Instance.OnUpdateShaftInventoryUI?.Invoke(itemController.FloorIndex);
	}

	public override void Exit()
	{
		for (int i = 0; i < skins.Count; i++)
		{
			var item = itemController.itemsHandle[i];
			item.ItemClicked -= ChangeSkin;
		}
	}
}
