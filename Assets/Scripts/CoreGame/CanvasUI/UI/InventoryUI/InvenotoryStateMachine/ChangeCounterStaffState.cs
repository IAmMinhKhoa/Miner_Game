using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System.Linq;

namespace UI.Inventory
{
	public class ChangeCounterStaffState : BaseState<InventoryItemType>
	{
		readonly StaffSkinUI staffSkinUI;
		public ChangeCounterStaffState(StaffSkinUI staffSkinUI)
		{
			this.staffSkinUI = staffSkinUI;
		}

		public override void Do()
		{

		}

		public override void Enter()
		{
			staffSkinUI.selectFloor.SetActive(false);
			var counter = Counter.Instance;
			
			int headSkinAmount = counter.Transporters[0].HeadSkeletonAnimation.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Head/Skin_")).Count();
			int bodySkinAmount = counter.Transporters[0].BodySkeletonAnimation.Skeleton.Data.Skins.Where(x => x.Name.StartsWith("Body/Skin_")).Count();
			staffSkinUI.CurrentItemTypeHandle = InventoryItemType.CounterCharacter;
			staffSkinUI.SetHeadIndex(headSkinAmount);
			staffSkinUI.SetBodyIndex(bodySkinAmount);

			int curHeadIndex = int.Parse(counter.counterSkin.character.idHead);
			int curbodyIndex = int.Parse(counter.counterSkin.character.idBody);
			staffSkinUI.SetCurentHeadBodyIndex(curHeadIndex, curbodyIndex);
			staffSkinUI.OnConfirmButtonClick += ChangeSkin;
			
		}

		private void ChangeSkin(int headSkin, int bodySkin)
		{
			Counter.Instance.counterSkin.character.idHead = headSkin.ToString();
			Counter.Instance.counterSkin.character.idBody = bodySkin.ToString();
			Counter.Instance.UpdateUI();
		}

		public override void Exit()
		{
			staffSkinUI.OnConfirmButtonClick -= ChangeSkin;
		}
	}
}
