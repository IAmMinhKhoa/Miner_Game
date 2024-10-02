using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using Spine;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
using System.Linq;
public class ChangeShaftCartState : BaseState<InventoryItemType>
{
	readonly PopupOtherItemController itemController;
	readonly List<DataSkinImage> skins = SkinManager.Instance.skinResource.skinWaitTable;
	Item itemPrefab;

	List<Item> items;
	public ChangeShaftCartState(PopupOtherItemController itemController, Item itemPrefab)
	{
		this.itemController = itemController;
		this.itemPrefab = itemPrefab;
	}
	public override void Do()
	{
		
	}

	public override void Enter()
	{
		itemController.title.text = "Đổi Xe Đẩy Nhân Viên";
		int currentFloor = itemController.FloorIndex;
		var cartSkeleton = ShaftManager.Instance.Shafts[currentFloor].Brewers[0].CartSkeletonAnimation;
		itemPrefab.spine.initialSkinName = cartSkeleton.Skeleton.Data.Skins.Items[1].Name;
		itemPrefab.spine.skeletonDataAsset = cartSkeleton.SkeletonDataAsset;
		itemPrefab.spine.Initialize(true);

		int skinAmount = cartSkeleton.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Xe day ")).Count();
		
		items = itemController.Init(itemPrefab, skinAmount);
		
		for (int i = 0; i < skinAmount; i++)
		{	
			var _item = items[i].spine;
			var skinName = SkinManager.Instance.skinResource.skinWaitTable[i].name;
			items[i].ChangItemInfo(skinName);
			_item.Skeleton.SetSkin("Xe day " + (i + 1));
			_item.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			_item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -35f);
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
		ShaftManager.Instance.Shafts[itemController.FloorIndex].shaftSkin.idCart = index.ToString();
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
