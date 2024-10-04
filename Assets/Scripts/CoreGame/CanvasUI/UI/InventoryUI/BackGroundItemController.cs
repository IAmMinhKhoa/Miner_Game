
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace UI.Inventory
{
    public class BackGroundItemController : MonoBehaviour
    {
		[SerializeField]
		GameObject content;
		public GameObject Content => content;


		[SerializeField]
		GameObject invetorPanel;

		public SkeletonGraphic imgSelectedBg;
		public SkeletonGraphic imgSelectedSecondBg;
		public TextMeshProUGUI tileSelectedBg;
		public TextMeshProUGUI descSelectedBg;
		public event Action OnConfirmButtonClick;


		List<BackGroundItem> items = new();

		public List<BackGroundItem> listItem => items;
		InventoryItemType currentItemHandle;
		public int Index { private set; get; }
		int currentIndexImage;

		public void ClearListItem()
		{
			foreach (BackGroundItem item2 in items)
			{
				Destroy(item2.gameObject);
			}
			items.Clear();
		}
		public void Init(BackGroundItem item, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				var _item = Instantiate(item, Vector3.zero, Quaternion.identity);
				_item.transform.SetParent(content.transform, false);
				items.Add(_item);
				_item.OnBackGroundItemClick += ItemSelected;
			}
			ItemSelected(-1);
		}
		private void ItemSelected(int index)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if(i == index)
				{
					items[i].Selected();
				}
				else
				{
					items[i].UnSelected();
				}
			}
		}
		public void ConfirmButtonClick()
		{
			OnConfirmButtonClick?.Invoke();
			CloseUI();
		}
		public void SetIndex(int index)
		{
			this.Index = index;
		}
		public void CloseUI()
		{
			gameObject.SetActive(false);
			invetorPanel.SetActive(true);
		}
	}
}
