using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketToggleHandle : MonoBehaviour
{
	Toggle toggleHandling;
	public InventoryItemType itemType;
	public event Action<InventoryItemType> OnTabulationClick;
	void Start()
    {
        toggleHandling = GetComponent<Toggle>();
		toggleHandling.onValueChanged.AddListener(isOn => OnToggleValueChanged(isOn, itemType));
    }

	private void OnToggleValueChanged(bool isOn, InventoryItemType itemType)
	{
		if (isOn)
		{

			OnTabulationClick?.Invoke(itemType);
		}
	}
}
