using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class TabStaffItem : SelecFloorItem, IPointerClickHandler
	{
		[SerializeField] SkeletonGraphic tail;
		public InventoryItemType type;
		public int Index {  get; set; }
		public event Action<InventoryItemType, int> OnItemClick;

		public override void SetInfoItem(int headIndex, int bodyIndex, int floor)
		{
			string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleShaft);
			if (type == InventoryItemType.ElevatorCharacter)
			{
				base.SetInfoItem(headIndex, bodyIndex, floor);
				return;
			}
			if (floor != -1)
				floorText.text = titleKey +" " + floor;
			
			head.Skeleton.SetSkin("Head/Skin_"+(headIndex+1));
			body.Skeleton.SetSkin("Body/Skin_" + (bodyIndex + 1));
			if(tail.Skeleton.Data.FindSkin("Tail/Skin_" + (headIndex + 1)) != null)
			{
				tail.gameObject.SetActive(true);
				tail.Skeleton.SetSkin("Tail/Skin_" + (headIndex + 1));
				tail.Skeleton.SetSlotsToSetupPose();
			}
			else
			{
				tail.gameObject.SetActive(false);
			}
			

			head.Skeleton.SetSlotsToSetupPose();
			body.Skeleton.SetSlotsToSetupPose();
			
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnItemClick?.Invoke(type, Index);
		}
	}
}
