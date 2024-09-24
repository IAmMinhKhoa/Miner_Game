using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class BackGroundItem : ItemInventoryUI
    {
        [SerializeField]
        TextMeshProUGUI nameBgItem;
		public int index { private set; get;}
		public string desc { private set; get; }

		public string iName { private set; get; }
		public Sprite img { private set; get; }
		public event Action<int> OnBackGroundItemClick;
		public void ChangItemInfo(Sprite sprite, string itemName, int index, string desc)
        {
            nameBgItem.text = itemName;
			iName = itemName;
			image.sprite = sprite;
			img = sprite;
			this.index = index;
			this.desc = desc;
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
			OnBackGroundItemClick?.Invoke(index);
        }
		public void SetItemType(InventoryItemType type)
		{
			this.type = type;
		}
    }
}
