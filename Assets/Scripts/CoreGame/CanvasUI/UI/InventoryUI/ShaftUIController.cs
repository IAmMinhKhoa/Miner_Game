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
				if (bgInfor.ContainsKey(item.type))
				{
					Sprite bgImage =  Resources.Load<Sprite>(bgInfor[item.type].path);
					item.ChangeItem(bgImage);

				}
				if (item.SkinList != null)
				{
					int indexCart = int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idCart);
					item.ChangeSpineSkin(item.SkinList.Items[indexCart]);
				}
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
