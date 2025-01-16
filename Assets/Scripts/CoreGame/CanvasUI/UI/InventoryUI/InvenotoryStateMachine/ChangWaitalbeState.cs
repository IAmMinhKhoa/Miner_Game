using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
using System.Linq;
public class ChangWaitalbeState : BaseState<InventoryItemType>
{
	readonly PopupOtherItemController itemController;
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
		string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryChangeTableMilkTea);
		itemController.title.text = titleKey;
		int currentFloor = itemController.FloorIndex;
		var cartSkeleton = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InventoryItemType.ShaftWaitTable];
		//set data
		itemPrefab.spine.initialSkinName = "Icon_1";
		itemPrefab.spine.skeletonDataAsset = cartSkeleton;
		itemPrefab.spine.Initialize(true);

		var skinManager = SkinManager.Instance;
		List<(string ID, string Name)> listSkin = skinManager.GetListPopupOtherItem(InventoryItemType.ShaftWaitTable);
	
		items = itemController.Init(itemPrefab, listSkin.Count);
		for (int i = 0; i < listSkin.Count; i++)
		{
			var _item = items[i].spine;
			items[i].ChangItemInfo((i+1).ToString(), int.Parse(listSkin[i].ID), InventoryItemType.ShaftWaitTable);
			_item.Skeleton.SetSkin("Icon_" + listSkin[i].ID);
			_item.AnimationState.SetAnimation(0, "Icon", false);
			_item.transform.localScale = new Vector3(0.17f, 0.17f, 1f);
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
		if (items == null) return;
		foreach (var item in items)
		{
			if (item != null)
				itemController.DestroyItem(item.gameObject);
		}
	}
}
