using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;
using System.Linq;
using Spine.Unity;

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
				int headSkinAmount = controller.ElevatorHeadStaff.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Head/Skin_")).Count();
				int bodySkinAmount = controller.ElevatorBodyStaff.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Body/Skin_")).Count();

				SkeletonDataAsset headDataAsset = controller.ElevatorHeadStaff.skeletonDataAsset;
				SkeletonDataAsset bodyDataAsset = controller.ElevatorBodyStaff.skeletonDataAsset;

				staffSkinUI.CurrentItemTypeHandle = InventoryItemType.ElevatorCharacter;
				staffSkinUI.SetHeadIndex(headSkinAmount, headDataAsset, "Head/Skin_", new(0.2f, 0.2f, 0.2f), new(0, -84));
				staffSkinUI.SetBodyIndex(headSkinAmount, bodyDataAsset, "Body/Skin_", new(0.2f, 0.2f, 0.2f), new(0, -42));
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
			staffSkinUI.DestroyObject();
			staffSkinUI.OnConfirmButtonClick -= ChangeSkin;
		}
	}
}
