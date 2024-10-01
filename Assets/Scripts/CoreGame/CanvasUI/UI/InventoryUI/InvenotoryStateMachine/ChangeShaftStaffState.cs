using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;
using System.Linq;

namespace UI.Inventory
{
	public class ChangeShaftStaffState : BaseState<InventoryItemType>
	{
		readonly StaffSkinUI staffSkinUI;
		public ChangeShaftStaffState(StaffSkinUI staffSkinUI)
		{
			this.staffSkinUI = staffSkinUI;
		}

		public override void Do()
		{

		}

		public override void Enter()
		{
			staffSkinUI.selectFloor.SetActive(true);
			var shaft = ShaftManager.Instance;
			staffSkinUI.SelectFloorHandle.OnChangeFloorSeleted += HandleChangListSkin;
			int curFloor = staffSkinUI.CurrentFloor;
			int headSkinAmount = shaft.Shafts[curFloor].Brewers[0].HeadSkeletonAnimation.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Head/Skin_")).Count();
			int bodySkinAmount = shaft.Shafts[curFloor].Brewers[0].BodySkeletonAnimation.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Body/Skin_")).Count();
			staffSkinUI.CurrentItemTypeHandle = InventoryItemType.ShaftCharacter;
			staffSkinUI.SetHeadIndex(headSkinAmount);
			staffSkinUI.SetBodyIndex(bodySkinAmount);

			int curHeadIndex = int.Parse(shaft.Shafts[curFloor].shaftSkin.characterSkin.idHead);
			int curbodyIndex = int.Parse(shaft.Shafts[curFloor].shaftSkin.characterSkin.idBody);
			staffSkinUI.SetCurentHeadBodyIndex(curHeadIndex, curbodyIndex);
			staffSkinUI.OnConfirmButtonClick += ChangeSkin;

		}

		private void HandleChangListSkin(List<int> list)
		{
			foreach (int i in list)
			{
				var shaft = ShaftManager.Instance.Shafts[i];
				shaft.shaftSkin.characterSkin.idBody = staffSkinUI.CurrentBodyIndex.ToString();
				shaft.shaftSkin.characterSkin.idHead = staffSkinUI.CurrentHeadIndex.ToString();
				shaft.UpdateUI();
			}
		}

		private void ChangeSkin(int headSkin, int bodySkin)
		{
			
			int curFloor = staffSkinUI.CurrentFloor;
			ShaftManager.Instance.Shafts[curFloor].shaftSkin.characterSkin.idHead = headSkin.ToString();
			ShaftManager.Instance.Shafts[curFloor].shaftSkin.characterSkin.idBody = bodySkin.ToString();
			ShaftManager.Instance.Shafts[curFloor].UpdateUI();
			
		}

		public override void Exit()
		{
			staffSkinUI.OnConfirmButtonClick -= ChangeSkin;
			staffSkinUI.SelectFloorHandle.OnChangeFloorSeleted -= HandleChangListSkin;
		}
	}
}
