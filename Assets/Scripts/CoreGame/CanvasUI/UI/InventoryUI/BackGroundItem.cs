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
			this.index = index;
			this.desc = desc;
			iName = itemName;
			nameBgItem.text = itemName;
			int idSkinInfo = SkinManager.Instance.ItemBought[itType].IndexOf((index+1).ToString());
			if(idSkinInfo == -1 && index != 0)
			{
				click.interactable = false;
				hideImg.gameObject.SetActive(true);
			}
		}
	}
}
