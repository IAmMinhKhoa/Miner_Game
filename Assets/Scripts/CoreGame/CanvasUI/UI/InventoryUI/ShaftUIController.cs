using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UI.Inventory.ItemInventoryUI;

namespace UI.Inventory
{
    public class ShaftUIController : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI title;
        int index;
		public List<DecoratorItem> items;

		public void UpdateShaftUI()
		{
			var bgInfor = ShaftManager.Instance.Shafts[index].shaftSkin.GetDataSkin();
			Sprite bgImage = Resources.Load<Sprite>(bgInfor["skinBgShaft"].path);
			foreach (DecoratorItem item in items)
			{
				if(item.type == InventoryItemType.ShaftBg) item.ChangeItem(bgImage);
			}
		}

		public void SetShaftIndex(int i)
        {
            index = i;
            title.text = "Tầng " + (i + 1).ToString();
			foreach (DecoratorItem item in items)
			{
				item.Index = i;
				
			}
        }
    }
}
