using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
using System.Linq;
public class ChangeCounterCartState : BaseState<InventoryItemType> 
{
	readonly PopupOtherItemController itemController;
	Item itemPrefab;

	List<Item> items;
	public ChangeCounterCartState(PopupOtherItemController itemController, Item itemPrefab)
	{
		this.itemController = itemController;
		this.itemPrefab = itemPrefab;
	}
	public override void Do()
	{

	}

	public override void Enter()
	{
		string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryChangeCartCouter);
		itemController.title.text = titleKey;
		int currentFloor = itemController.FloorIndex;
		var cartSkeleton = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InventoryItemType.ShaftCart];
		itemPrefab.spine.initialSkinName = cartSkeleton.GetSkeletonData(true).Skins.Items[0].Name;
		itemPrefab.spine.skeletonDataAsset = cartSkeleton;
		itemPrefab.spine.Initialize(true);

		int skinAmount = itemPrefab.spine.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();

		items = itemController.Init(itemPrefab, skinAmount);

		for (int i = 0; i < skinAmount; i++)
		{
			var _item = items[i].spine;
			var skinName = SkinManager.Instance.skinResource.skinWaitTable[i].name;
			items[i].ChangItemInfo(skinName, i, InventoryItemType.ShaftCart);
			_item.Skeleton.SetSkin("Skin_" + (i + 1));
			_item.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
			_item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -65f);
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
		Counter.Instance.counterSkin.idCart = index.ToString();
		Counter.Instance.UpdateUI();
		Counter.Instance.OnUpdateCounterInventoryUI?.Invoke();
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
