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
			itemController.title.text = "Đổi BackGround Phòng Chờ Trà Sữa";
			var bgElevatorSkeleton = SkinManager.Instance.SkinGameDataAsset.SkinGameData[InventoryItemType.ElevatorBg];
			//set data
			itemPrefab.spine.initialSkinName = "Icon_1";
			itemPrefab.spine.skeletonDataAsset = bgElevatorSkeleton;
			itemPrefab.spine.Initialize(true);
			int skinAmount = itemPrefab.spine.Skeleton.Data.Skins.Where(skin => skin.Name.StartsWith("Skin_")).Count();
			items = itemController.Init(itemPrefab, skinAmount);

			for (int i = 0; i < skinAmount; i++)
			{
				var _item = items[i].spine;
				
				var skinName = SkinManager.Instance.skinResource.skinBgElevator[i].name;
				items[i].ChangItemInfo(skinName);
				_item.Skeleton.SetSkin("Icon_" +(i+1));
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
