using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;
namespace UI.Inventory
{
	public class ChangeElevatorStaffState : BaseState<InventoryItemType>
	{
		StaffSkinUI staffSkinUI;
		public ChangeElevatorStaffState(StaffSkinUI staffSkinUI)
		{
			this.staffSkinUI = staffSkinUI;
		}

		public override void Do()
		{
			
		}

		public override void Enter()
		{
			staffSkinUI.selectFloor.SetActive(false);
			var elevator = ElevatorSystem.Instance;
			if (elevator.ElevatorController.TryGetComponent<ElevatorControllerView>(out var controller))
			{
				int headSkinAmount = controller.ElevatorHeadStaff.Skeleton.Data.Skins.Count;
				int bodySkinAmount = controller.ElevatorBodyStaff.Skeleton.Data.Skins.Count;
				staffSkinUI.CurrentItemTypeHandle = InventoryItemType.ElevatorCharacter;
				staffSkinUI.SetHeadIndex(headSkinAmount);
				staffSkinUI.SetBodyIndex(bodySkinAmount);
				int curHeadIndex = int.Parse(elevator.elevatorSkin.characterSkin.idHead);
				int curbodyIndex = int.Parse(elevator.elevatorSkin.characterSkin.idBody);
				staffSkinUI.SetCurentHeadBodyIndex(curHeadIndex, curbodyIndex);
				staffSkinUI.OnConfirmButtonClick += ChangeSkin;
			}
		}

		private void ChangeSkin(int headSkin, int bodySkin)
		{
			ElevatorSystem.Instance.elevatorSkin.characterSkin.idHead = headSkin.ToString();
			ElevatorSystem.Instance.elevatorSkin.characterSkin.idBody = bodySkin.ToString();
			ElevatorSystem.Instance.UpdateUI();
		}

		public override void Exit()
		{
			staffSkinUI.OnConfirmButtonClick -= ChangeSkin;
		}
	}
}
