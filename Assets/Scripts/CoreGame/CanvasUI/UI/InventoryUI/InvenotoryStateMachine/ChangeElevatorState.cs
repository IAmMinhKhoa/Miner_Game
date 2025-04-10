using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using UI.Inventory.PopupOtherItem;
using System.Linq;
using TMPro;

namespace UI.Inventory
{
	public class ChangeElevatorState : BaseState<InventoryItemType>
	{
		readonly PopupOtherItemController itemController;
		Item itemPrefab;

		List<Item> items;
		public ChangeElevatorState(PopupOtherItemController itemController, Item itemPrefab)
		{
			this.itemController = itemController;
			this.itemPrefab = itemPrefab;
		}
		public override void Do()
		{

		}

		public override void Enter()
		{
			string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryChooseSkinElevator);
			itemController.title.text = titleKey;

			itemPrefab.spine.initialSkinName = "Skin_1";
			//set data
			itemPrefab.spine.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InventoryItemType.Elevator];
			itemPrefab.spine.Initialize(true);

			var skinManager = SkinManager.Instance;
			List<(string ID, string Name)> listSkin = skinManager.GetListPopupOtherItem(InventoryItemType.Elevator);
			items = itemController.Init(itemPrefab, listSkin.Count);
			for (int i = 0; i < listSkin.Count; i++)
			{
				var _item = items[i].spine;
				_item.allowMultipleCanvasRenderers = true;
				_item.transform.localScale = new Vector3(0.17f, 0.17f, 1f);
				var skinName = SkinManager.Instance.skinResource.skinBgElevator[i].name;
				items[i].ChangItemInfo((i + 1).ToString(), int.Parse(listSkin[i].ID), InventoryItemType.Elevator);
				_item.Skeleton.SetSkin("Icon_" + listSkin[i].ID);
				_item.AnimationState.SetAnimation(0, "Icon", false);
				items[i].ItemClicked += ChangeSkin;
				_item.Skeleton.SetSlotsToSetupPose();
				_item.UpdateMesh();
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
			ElevatorSystem.Instance.elevatorSkin.idFrontElevator = index.ToString();
			ElevatorSystem.Instance.UpdateUI();
			ElevatorSystem.Instance.OnUpdateElevatorInventoryUI?.Invoke();
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
}
