using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject/MartketItemSize")]
public class ResizeMarketItem : ScriptableObject
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
[System.Serializable]
public struct ItemSize
{
	public InventoryItemType type;
	public string skinName;
	public Vector3 scale;
	public Vector2 pos;
}

