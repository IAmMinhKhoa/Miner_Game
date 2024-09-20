using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
	public class CharacterSkinUI : MonoBehaviour, IPointerClickHandler
	{
		public event Action<int> OnItemClicked;
		private int _index;
		public int Index
		{
			get => _index;
			set
			{
				_index = value;
			}
		}
		[SerializeField] Image BG;
		[SerializeField] Image border;
		[SerializeField] TextMeshProUGUI itemName;
		public void SetItemInfo(Sprite bg, string name)
		{
			//BG.sprite = bg;
			itemName.text = name;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			OnItemClicked?.Invoke(_index);
		}
		public void Active()
		{
			border.gameObject.SetActive(true);
		}
		public void Unactive()
		{
			border.gameObject.SetActive(false);
		}

	}

	
}
