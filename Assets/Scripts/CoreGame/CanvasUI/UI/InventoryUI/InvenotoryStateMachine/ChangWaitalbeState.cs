using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
public class ChangWaitalbeState : BaseState<InventoryItemType>
{
	readonly PopupOtherItemController itemController;
	readonly List<DataSkinImage> skins = SkinManager.Instance.skinResource.skinWaitTable;
	Item itemPrefab;

	List<Item> items;
	public ChangWaitalbeState(PopupOtherItemController itemController, Item itemPrefab)
	{
		this.itemController = itemController;
		this.itemPrefab = itemPrefab;
	}
	public override void Do()
	{
		
	}

	public override void Enter()
	{
		itemController.UnactiveAll();
		itemController.title.text = "Đổi Bàn Để Ly Trà Sữa";
		int currentFloor = itemController.FloorIndex;
		itemPrefab.cart.skeletonDataAsset = ShaftManager.Instance.Shaft.GetComponent<ShaftUI>().WaitTable.SkeletonDataAsset;
		itemPrefab.cart.Initialize(true);
		
		int skinAmount = itemPrefab.cart.Skeleton.Data.Skins.Count / 2;
		items = itemController.Init(itemPrefab, skinAmount);
		for (int i = 0; i < skinAmount; i++)
		{
			var _item = items[i].cart;
			
			_item.Skeleton.SetSkin("Icon_" + (i + 1));
			_item.AnimationState.SetAnimation(0, "icon", false);
			_item.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);
			_item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -29f); 
			items[i].ItemClicked += ChangeSkin;
			_item.Skeleton.SetSlotsToSetupPose();
		}
	}
	private void ChangeSkin(Item item) 
	{
		int index = items.IndexOf(item);
		foreach (var _item in items)
		{
			_item.Unselected();
		}
		items[index].Selected();
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
		if (items == null) return;
		foreach (var item in items)
		{
			if (item != null)
				itemController.DestroyItem(item.gameObject);
		}
	}
}
