using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
using Spine;
namespace UI.Inventory
{
	public class DecoratorItem : ItemInventoryUI
	{
		[SerializeField]
		private SkeletonGraphic spine;
		public ExposedList<Skin> SkinList
		{
			get
			{
				if (spine != null)
					return spine.Skeleton.Data.Skins;
				else
					return null;
			}
		}
		private void Awake()
		{
			if (spine != null)
			{
				spine.Initialize(false);
			}
			
		}
		public int Index { set; get;} = -1;
		public override void OnPointerClick(PointerEventData eventData)
		{
			OnItemClick?.Invoke(type, Index);
		}
		public void ChangeSpineSkin(string skinName)
		{
			if (skinName == null)
			{
				Debug.Log(this.name + " Skin not fond");
				return;
			}
		
			spine.Skeleton.SetSkin(skinName);
			spine.Skeleton.SetSlotsToSetupPose();
		}
	}
}
