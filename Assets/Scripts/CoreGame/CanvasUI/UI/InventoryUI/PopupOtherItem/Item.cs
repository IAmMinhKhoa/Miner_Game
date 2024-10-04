using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Spine.Unity;
using PlayFab.ClientModels;
using Spine;

namespace UI.Inventory.PopupOtherItem
{
    public class Item : MonoBehaviour, IPointerClickHandler
    {
		[SerializeField] Image boder;
		[SerializeField] TextMeshProUGUI itemName;
		public SkeletonGraphic spine;
		public event Action<Item> ItemClicked;
		public void Selected()
		{
			boder.gameObject.SetActive(true);
		}
		public void ChangItemInfo(string iName)
		{
			itemName.text = iName;
		}
		public void Unselected()
		{
			boder.gameObject.SetActive(false);
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			ItemClicked?.Invoke(this);
		}
	}
}
