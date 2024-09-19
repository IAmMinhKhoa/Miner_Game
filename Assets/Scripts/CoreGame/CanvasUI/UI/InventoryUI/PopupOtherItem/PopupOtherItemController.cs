using System.Collections;
using System.Collections.Generic;
using UI.Inventory.PopupOtherItem;
using UnityEngine;

namespace UI.Inventory
{
    public class PopupOtherItemController : MonoBehaviour
    {
		public List<Item> itemsHandle;
		public int FloorIndex { set; get;}
		public void UnselectAllItem()
		{
			foreach (Item item in itemsHandle)
			{
				if(item.gameObject.activeInHierarchy)
				{
					item.Unselected();
				}	
			}
		}
		public void UnactiveAll()
		{
			foreach (Item item in itemsHandle)
			{
				if (item.gameObject.activeInHierarchy)
				{
					item.gameObject.SetActive(false);
				}
			}
		}
		public void CloseUI()
		{
			gameObject.SetActive(false);
		}
    }
}
