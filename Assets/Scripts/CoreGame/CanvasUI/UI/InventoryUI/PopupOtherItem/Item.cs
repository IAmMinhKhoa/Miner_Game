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
    public class Item : MonoBehaviour
    {
		[SerializeField] Image boder;
		[SerializeField] TextMeshProUGUI itemName;
		[SerializeField]
		Image hideImg;
		[SerializeField]
		Button clickButton;
		public SkeletonGraphic spine;
		public event Action<Item> ItemClicked;
		public void Selected()
		{
			boder.gameObject.SetActive(true);
		}
		public void ChangItemInfo(string iName, int index, InventoryItemType itType)
		{
			string titleKey = string.Empty;
			switch (itType)
			{
				case InventoryItemType.ShaftBg:
					break;
				case InventoryItemType.CounterBg:
					break;
				case InventoryItemType.ElevatorBg:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallElevator);
					break;
				case InventoryItemType.CounterCart:
					break;
				case InventoryItemType.Elevator:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryElevator);
					break;
				case InventoryItemType.ShaftSecondBg:
					break;
				case InventoryItemType.ShaftCart:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryCart);
					break;
				case InventoryItemType.ShaftWaitTable:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWaitTable);
					break;
				case InventoryItemType.ShaftCharacter:
					break;
				case InventoryItemType.ElevatorCharacter:
					break;
				case InventoryItemType.CounterCharacter:
					break;
				case InventoryItemType.CounterSecondBg:
					break;
				case InventoryItemType.BackElevator:
					break;
				case InventoryItemType.ShaftCharacterBody:
					break;
				case InventoryItemType.ElevatorCharacterBody:
					break;
			}
			itemName.text = titleKey + " " + index.ToString();
			int idInInfor = SkinManager.Instance.ItemBought[itType].IndexOf((index + 1).ToString());
			if(idInInfor == -1 && index != 0)
			{
				clickButton.interactable = false;
				hideImg.gameObject.SetActive(true);
			}
		}
		public void Unselected()
		{
			boder.gameObject.SetActive(false);
		}
		public void OnPointerClick()
		{
			ItemClicked?.Invoke(this);
		}
	}
}
