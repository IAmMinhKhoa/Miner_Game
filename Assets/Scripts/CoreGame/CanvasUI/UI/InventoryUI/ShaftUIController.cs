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
		bool isUpdataDataAsset = false;
		public void UpdateShaftUI()
		{
			
			foreach (DecoratorItem item in items)
			{
				if(isUpdataDataAsset == false)
				{
					item.Spine.skeletonDataAsset = SkinManager.Instance.SkinGameDataAsset.SkinGameData[item.type];
					item.Spine.Initialize(true);
				}

				string id;
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
						item.Spine.AnimationState.SetAnimation(0, "Icon", false);
						item.ChangeSpineSkin("Icon_" + id);
						break;
				}
				
				if (item.type == InventoryItemType.ShaftCart)
				{
					int indexCart = int.Parse(ShaftManager.Instance.Shafts[index].shaftSkin.idCart);
					item.ChangeSpineSkin("Skin_" + (indexCart + 1));
					continue;
				}
				
			}
			isUpdataDataAsset = true;
		}
		public void SetShaftIndex(int i)
        {
            index = i;
			string titleKey = LocalizationManager.GetLocalizedString(LanguageKeys.TitleInventoryShaft);
            title.text = titleKey+" " + (i + 1).ToString();
			if (items == null) return;
			foreach (DecoratorItem item in items)
			{
				item.SetIndex(i);
			}
        }
    }
}
