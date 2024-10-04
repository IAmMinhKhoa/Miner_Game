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
	public class CharacterSkinUI : MonoBehaviour, IPointerClickHandler
	{
		public event Action<int> OnItemClicked;
		[SerializeField] SkeletonGraphic spine;
		public SkeletonGraphic Spine => spine;
		private int _index;
		public int Index => _index;

		[SerializeField] Image border;
		[SerializeField] TextMeshProUGUI itemName;
		public void SetItemInfo(int indexSkin)
		{
			_index = indexSkin;
			
			itemName.text = indexSkin.ToString();
		}
		public void OnPointerClick(PointerEventData eventData)
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
