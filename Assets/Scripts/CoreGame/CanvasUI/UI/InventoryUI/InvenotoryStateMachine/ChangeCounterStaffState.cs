using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System.Linq;
using Spine.Unity;
using System.Security.Cryptography;

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
			var sourceSkins = SkinManager.Instance.SkinGameDataAsset.SkinGameData;
			SkeletonDataAsset headDataAsset = sourceSkins[InventoryItemType.CounterCharacter];
			SkeletonDataAsset bodyDataAsset = sourceSkins[InventoryItemType.CounterCharacter];

			int headSkinAmount = headDataAsset.GetSkeletonData(true).Skins.Where(x => x.Name.StartsWith("Head/Skin_")).Count();
			int bodySkinAmount = bodyDataAsset.GetSkeletonData(true).Skins.Where(x => x.Name.StartsWith("Body/Skin_")).Count();

			staffSkinUI.CurrentItemTypeHandle = InventoryItemType.CounterCharacter;
			staffSkinUI.SetHeadIndex(headSkinAmount, headDataAsset, "Head/Skin_", new(0.4f, 0.4f, 0.4f), new(0, -113));
			staffSkinUI.SetBodyIndex(headSkinAmount, bodyDataAsset, "Body/Skin_", new(0.4f, 0.4f, 0.4f), new(3, -45));


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
			staffSkinUI.DestroyObject();
			staffSkinUI.OnConfirmButtonClick -= ChangeSkin;
		}
	}
}
