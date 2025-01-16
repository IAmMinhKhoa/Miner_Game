using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UI.Inventory.PopupOtherItem;
using Spine.Unity;
using System.Linq;
namespace UI.Inventory
{
	public class ChangeElevatorBG : BaseState<InventoryItemType>
	{
		readonly PopupOtherItemController itemController;
		Item itemPrefab;

		List<Item> items;
		public ChangeElevatorBG(PopupOtherItemController itemController, Item itemPrefab)
		{
			this.itemController = itemController;
			this.itemPrefab = itemPrefab;
		}


		public override void Do()
		{

		}

		public override void Enter()
		{
			string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryChangeBackGround);
			itemController.title.text = titleKey;
			var bgElevatorSkeleton = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InventoryItemType.ElevatorBg];
			//set data
			itemPrefab.spine.initialSkinName = "Icon_1";
			itemPrefab.spine.skeletonDataAsset = bgElevatorSkeleton;
			itemPrefab.spine.Initialize(true);


			var skinManager = SkinManager.Instance;
			List<(string ID, string Name)> listSkin = skinManager.GetListPopupOtherItem(InventoryItemType.ElevatorBg);


			items = itemController.Init(itemPrefab, listSkin.Count);

			for (int i = 0; i < listSkin.Count; i++)
			{
				var _item = items[i].spine;
				
				var skinName = SkinManager.Instance.skinResource.skinBgElevator[i].name;
				items[i].ChangItemInfo((i + 1).ToString(), int.Parse(listSkin[i].ID), InventoryItemType.ElevatorBg);

				_item.transform.localScale = new Vector3(0.17f, 0.17f, 1f);
				//_item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -29f);
				_item.Skeleton.SetSkin("Icon_" + listSkin[i].ID);
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
			ElevatorSystem.Instance.elevatorSkin.idBackGround = index.ToString();
			ElevatorSystem.Instance.UpdateUI();
			ElevatorSystem.Instance.OnUpdateElevatorInventoryUI?.Invoke();
		}
		public override void Exit()
		{
			foreach (var item in items)
			{
				if(item != null)
					itemController.DestroyItem(item.gameObject);
			}

		}
	}
}
