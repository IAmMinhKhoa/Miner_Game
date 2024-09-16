using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
	public class DecoratorItem : ItemInventoryUI
	{
		public int Index { set; get;} = -1;
		public override void OnPointerClick(PointerEventData eventData)
		{
			OnItemClick?.Invoke(type, Index);
		}
	}
}
