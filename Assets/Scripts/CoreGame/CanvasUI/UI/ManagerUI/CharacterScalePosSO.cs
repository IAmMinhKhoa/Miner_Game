using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : ScriptableObject
{
	[SerializeField]
	List<ItemSize> listItemSize;
	public Dictionary<InventoryItemType, ItemSize> itemSizeDic
	{
		get
		{
			Dictionary<InventoryItemType, ItemSize> _tmpDic = new();
			foreach (var item in listItemSize)
			{
				_tmpDic[item.type] = item;
			}
			return _tmpDic;
		}
	}
}


