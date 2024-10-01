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
			
			foreach (DecoratorItem item in items)
			{
				string id = "";
				switch (item.type)
				{
					case InventoryItemType.ShaftBg:
						id = (int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idBackGround) + 1) + "";
						item.ChangeSpineSkin("Icon_" + id);
						break;
					case InventoryItemType.ShaftSecondBg:
						id = (int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idSecondBg) + 1) + "";
						item.ChangeSpineSkin("Icon_" + id);
						break;
					case InventoryItemType.ShaftWaitTable:
						id = (int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idWaitTable) + 1) + "";
						item.ChangeSpineSkin("Icon_" + id);
						break;
				}
				
				if (item.type == InventoryItemType.ShaftCart)
				{
					int indexCart = int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idCart);
					item.ChangeSpineSkin(item.SkinList.Items[indexCart].Name);
					continue;
				}
				
			}
		}

		public void SetShaftIndex(int i)
        {
            index = i;
            title.text = "Tầng " + (i + 1).ToString();
			if (items == null) return;
			foreach (DecoratorItem item in items)
			{
				item.Index = i;
			}
        }
    }
}
