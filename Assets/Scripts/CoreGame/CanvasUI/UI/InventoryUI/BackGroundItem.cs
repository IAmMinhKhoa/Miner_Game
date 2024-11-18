using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class BackGroundItem : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI nameBgItem;
		public SkeletonGraphic bg;
		public SkeletonGraphic secondBg;
		[SerializeField]
		Button click;
		[SerializeField]
		Image hideImg;
		public int index { private set; get;}
		public string desc { private set; get; }

		public string iName { private set; get; }
		public Sprite img { private set; get; }
		public event Action<int> OnBackGroundItemClick;

		public void OnPointerClick()
		{
			OnBackGroundItemClick?.Invoke(index);
		}
		public void Selected()
		{
			RectTransform _rectTransform = GetComponent<RectTransform>();
			_rectTransform.DOScale(1f, 0.2f)
				.SetEase(Ease.OutQuad);
		}
		public void UnSelected()
		{
			RectTransform _rectTransform = GetComponent<RectTransform>();
			_rectTransform.DOScale(0.9f, 0f)
				.SetEase(Ease.OutQuad);
		}
		public void SetItemInfor(int index, string itemName, string desc, InventoryItemType itType)
		{
			string titleKey = string.Empty;
			string titleKeyDesc = string.Empty;
			Debug.LogError(itType.ToString());
			switch(itType)
			{
				case InventoryItemType.ShaftBg:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftBg);
					titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftBg);
					break;
				case InventoryItemType.CounterBg:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallCouter);
					titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryWallCouter);
					break;
				case InventoryItemType.ElevatorBg:
					break;
				case InventoryItemType.CounterCart:
					break;
				case InventoryItemType.Elevator:
					break;
				case InventoryItemType.ShaftSecondBg:
					titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftSecondBg);
					titleKeyDesc = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaftSecondBg);
					break;
				case InventoryItemType.ShaftCart:
					break;
				case InventoryItemType.ShaftWaitTable:
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
			this.index = index;
			this.desc = titleKeyDesc;
			iName = titleKey;
			nameBgItem.text = titleKey;
			int idSkinInfo = SkinManager.Instance.ItemBought[itType].IndexOf((index).ToString());
			if(idSkinInfo == -1 && index != 1)
			{
				click.interactable = false;
				hideImg.gameObject.SetActive(true);
			}
		}
	}
}
