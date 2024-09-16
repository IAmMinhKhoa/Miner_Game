using Newtonsoft.Json;
using PlayFab.EconomyModels;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static UI.Inventory.ItemInventoryUI;

namespace UI.Inventory
{
    public class BackGroundItemController : MonoBehaviour
    {
		[SerializeField]
		GameObject content;
		[SerializeField]
		BackGroundItem item;
		[SerializeField]
		int amountItem;

		List<BackGroundItem> items;
		
		InventoryItemType currentItemHandle;
		int index;
		private void Awake()
		{
			items = new();
			Init();
		}
		private void Init()
		{
			for (int i = 0; i < amountItem; i++)
			{
				var _item = Instantiate(item, Vector3.zero, Quaternion.identity);
				_item.transform.SetParent(content.transform, false);
				_item.gameObject.SetActive(false);
				_item.OnBackGroundItemClick += HandleBgItemClick;
				items.Add(_item);
			}
		
		}

		private void HandleBgItemClick(int index)
		{
			switch (currentItemHandle)
			{
				case InventoryItemType.ShaftBg:
					ShaftManager.Instance.Shafts[this.index].shaftSkin.idBackGround = index.ToString();
					ShaftManager.Instance.OnUpdateShaftUI?.Invoke(this.index);
					string json = JsonConvert.SerializeObject(ShaftManager.Instance.Shafts[this.index].shaftSkin);
					Debug.Log(json);
					break;
			}
		}

		private void UnactiveAllItems()
		{
			for (int i = 0; i < items.Count; i++)
			{
				if(items[i].gameObject.activeInHierarchy)
					items[i].gameObject.SetActive(false);
			}
		}
		private void LoadShaftBg()
		{
			UnactiveAllItems();
			var shaftBg = SkinManager.Instance.skinResource.skinBgShaft;
			for(int i = 0; i < shaftBg.Count; i++)
			{
				items[i].gameObject.SetActive(true);
				Sprite sprite = Resources.Load<Sprite>(shaftBg[i].path);
				string name = shaftBg[i].name;
				items[i].ChangItemInfo(sprite, name, i);
			}
		}

		public void SetItemHandle(InventoryItemType itemHandle, int index)
		{
			currentItemHandle = itemHandle;
			this.index = index;
			switch (itemHandle)
			{
				case InventoryItemType.ShaftBg:
					LoadShaftBg();
					break;
			}	
		}
	}
}
