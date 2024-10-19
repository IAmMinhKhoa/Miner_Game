using Spine;
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
	public class CharacterSkinUI : MonoBehaviour
	{
		public event Action<int> OnItemClicked;
		[SerializeField] SkeletonGraphic spine;
		[SerializeField]
		Image hideImg;
		[SerializeField]
		Button clickButton;
		public SkeletonGraphic Spine => spine;
		private int _index;
		public int Index => _index;

		[SerializeField] Image border;
		[SerializeField] TextMeshProUGUI itemName;
		public void SetItemInfo(int indexSkin, InventoryItemType itType)
		{
			_index = indexSkin;
			
			itemName.text = SkinManager.Instance.InfoSkinGame[itType][indexSkin].name;
			int idInInfo = SkinManager.Instance.ItemBought[itType].IndexOf((indexSkin+1).ToString());

			if(idInInfo == -1 && indexSkin != 0)
			{
				clickButton.interactable = false;
				hideImg.gameObject.SetActive(true);
			}

		}
		public void OnPointerClick()
		{
			OnItemClicked?.Invoke(_index);
		}
		public void Select()
		{
			border.gameObject.SetActive(true);
		}
		public void Unselect()
		{
			border.gameObject.SetActive(false);
		}

	}

	
}
