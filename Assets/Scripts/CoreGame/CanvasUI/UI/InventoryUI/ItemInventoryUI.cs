using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public abstract class ItemInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        protected Image image;
		public InventoryItemType type;

		public Action<InventoryItemType, int> OnItemClick;
        public virtual void ChangeItem(Sprite sprite)
        {
			if(image != null) 
				image.sprite = sprite;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
