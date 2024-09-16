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
		int index;
		public event Action<int> OnBackGroundItemClick;
		public void ChangItemInfo(Sprite sprite, string itemName, int index)
        {
            nameBgItem.text = itemName;
			image.sprite = sprite;
			this.index = index;
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
