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
		[SerializeField] Image itemImg;
		[SerializeField] Image boder;
		[SerializeField] TextMeshProUGUI itemName;
		public SkeletonGraphic elevator;
		public SkeletonGraphic cart;
		public event Action<Item> ItemClicked;
		public void Selected()
		{
			boder.gameObject.SetActive(true);
		}
		public void ChangItemInfo(Sprite sprite, string iName)
		{
			itemImg.sprite = sprite;
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
		public void ShowElevator(int index)
		{
			elevator.gameObject.SetActive(true);
			cart.gameObject.SetActive(false);
			itemImg.gameObject.SetActive(false);
			var skin = elevator.Skeleton.Data.Skins.Items[index];
			elevator.Skeleton.SetSkin(skin);
			elevator.Skeleton.SetSlotsToSetupPose();
		}
		public void ShowCart(int index)
		{
			elevator.gameObject.SetActive(false);
			cart.gameObject.SetActive(true);
			itemImg.gameObject.SetActive(false);
			var skin = cart.Skeleton.Data.Skins.Items[index];
			cart.Skeleton.SetSkin(skin);
			cart.Skeleton.SetSlotsToSetupPose();
		}
		public void ShowNothing()
		{
			elevator.gameObject.SetActive(false);
			cart.gameObject.SetActive(false);
			itemImg.gameObject.SetActive(true);

		}
	}
}
