using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Inventory.PopupOtherItem;
using UnityEngine;

namespace UI.Inventory
{
    public class PopupOtherItemController : MonoBehaviour
    {
		public List<Item> itemsHandle;
		public int FloorIndex { set; get;}
		public GameObject content;
		public TextMeshProUGUI title;
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
		public List<Item> Init(Item item, int amount)
		{
			List<Item> items = new();
			for (int i = 0;	i < amount; ++i)
			{
				var _item = Instantiate(item, Vector3.zero, Quaternion.identity);
				_item.transform.SetParent(content.transform, false);
				items.Add(_item);
			}
			return items;
		}
 		public void UnactiveAll()
		{
			foreach (Item item in itemsHandle)
			{
	
					item.gameObject.SetActive(false);

			}
		}
		public void DestroyItem(GameObject obj)
		{
			Destroy(obj);
		}
		public void CloseUI()
		{
			gameObject.SetActive(false);
		}
    }
}
