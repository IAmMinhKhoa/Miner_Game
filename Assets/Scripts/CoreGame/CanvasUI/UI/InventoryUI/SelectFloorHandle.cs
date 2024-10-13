using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class SelectFloorHandle : MonoBehaviour
    {
        public StaffSkinUI staffSkinUI;
		[SerializeField] GameObject content;
		[SerializeField] SelecFloorItem item;
		List<SelecFloorItem> listItem;
		[SerializeField] Toggle selectAll;
		public List<SelecFloorItem> ListItem => listItem;

		public event Action<List<int>> OnChangeFloorSeleted;
		private void OnEnable()
		{
			listItem ??= new();
			int curFloorAmount = ShaftManager.Instance.Shafts.Count;
			while(listItem.Count < curFloorAmount)
			{
				SelecFloorItem newItem = Instantiate(item, content.transform);
				newItem.OnItemSlect += CheckSelectAll;
				listItem.Add(newItem); 
			}
			selectAll.onValueChanged.AddListener(SelectAllOnClick);
			UpdateUI();

		}

		private void CheckSelectAll()
		{
			selectAll.SetIsOnWithoutNotify(CheckListItem());
		}

		private bool CheckListItem()
		{
			foreach (SelecFloorItem item in listItem)
			{
				if (item.TryGetComponent<Toggle>(out Toggle toggle))
				{
					if (toggle.isOn == false) return false;
				}
			}
			return true;
		}
		public void UpdateUI()
		{
			for (int i = 0; i < ShaftManager.Instance.Shafts.Count; i++)
			{
				int headIndex = int.Parse(ShaftManager.Instance.Shafts[i].shaftSkin.characterSkin.idHead);
				int bodyIndex = int.Parse(ShaftManager.Instance.Shafts[i].shaftSkin.characterSkin.idBody);
				listItem[i].SetInfoItem(headIndex, bodyIndex, i + 1);
				var item = listItem[i];
				if (item.TryGetComponent<Toggle>(out Toggle toggle))
				{
					toggle.SetIsOnWithoutNotify(false);
				}
				CheckSelectAll();
			}
		}
		public void SelectAllOnClick(bool isOn)
		{
			foreach (SelecFloorItem item in listItem)
			{
				if (item.TryGetComponent<Toggle>(out Toggle toggle))
				{
					
					toggle.SetIsOnWithoutNotify(isOn);
				}
			}
		}
		public void ChangeFloorSelected()
		{
			List<int> itemsSelected = new();
			for(int i = 0; i < listItem.Count; i++)
			{
				var item = listItem[i];	
				if (item.TryGetComponent<Toggle>(out Toggle toggle))
				{
					if(toggle.isOn) itemsSelected.Add(i);
				}
			}
			OnChangeFloorSeleted?.Invoke(itemsSelected);
			UpdateUI();
			CloseUI();
		}

		public void CloseUI()
		{
			staffSkinUI.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
    }
}
