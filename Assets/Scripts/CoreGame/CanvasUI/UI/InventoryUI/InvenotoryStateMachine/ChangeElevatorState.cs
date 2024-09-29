using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Spine.Unity;
using UI.Inventory.PopupOtherItem;

namespace UI.Inventory
{
	public class ChangeElevatorState : BaseState<InventoryItemType>
	{
		readonly PopupOtherItemController itemController;
		public ChangeElevatorState(PopupOtherItemController itemController)
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
			itemController.title.text = "Chọn Skin Thang Máy";
			if (ElevatorSystem.Instance.ElevatorController.TryGetComponent<ElevatorControllerView>(out var elevatorControllerView))
			{
				var skins = elevatorControllerView.FontElevator.skeleton.Data.Skins;
				for (int i = 0;i < skins.Count; i++ ) 
				{
					itemController.itemsHandle[i].gameObject.SetActive(true);
					itemController.itemsHandle[i].ShowElevator(i);
					var skinName = SkinManager.Instance.skinResource.skinElevator[i].name;
					itemController.itemsHandle[i].ChangItemInfo(skinName);
					itemController.itemsHandle[i].ItemClicked += ChangeSkin;
				}
			}

		}
		private void ChangeSkin(Item item)
		{
			int index = itemController.itemsHandle.IndexOf(item);
			itemController.UnselectAllItem();
			itemController.itemsHandle[index].Selected();
			ElevatorSystem.Instance.elevatorSkin.idFrontElevator = index.ToString();
			ElevatorSystem.Instance.UpdateUI();
			ElevatorSystem.Instance.OnUpdateElevatorInventoryUI?.Invoke();
		}
		public override void Exit()
		{
			if (ElevatorSystem.Instance.ElevatorController.TryGetComponent<ElevatorControllerView>(out var elevatorControllerView))
			{
				var skins = elevatorControllerView.FontElevator.skeleton.Data.Skins;
				for (int i = 0; i < skins.Count; i++)
				{
					itemController.itemsHandle[i].ItemClicked -= ChangeSkin;
				}
			}
		}
	}
}
