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
		private SkeletonAnimation cartSkeleton = null;
		public ExposedList<Skin> SkinList
		{
			get
			{
				Debug.Log(cartSkeleton);
				if (cartSkeleton != null)
					return cartSkeleton.skeleton.Data.Skins;
				else
					return null;
			}
		}
		private void Awake()
		{
			if (cartSkeleton != null)
			{
				cartSkeleton.Initialize(false);
			}
			
		}
		public int Index { set; get;} = -1;
		public override void OnPointerClick(PointerEventData eventData)
		{
			OnItemClick?.Invoke(type, Index);
		}
		public void ChangeSpineSkin(Skin skin)
		{
			if (skin == null)
			{
				Debug.Log(this.name + " Skin not fond");
				return;
			}
			
			cartSkeleton.skeleton.SetSkin(skin);
			cartSkeleton.skeleton.SetSlotsToSetupPose();
		}
	}
}
