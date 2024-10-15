using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class MarketStateMachine : StateManager<InventoryItemType>
{
	[SerializeField]
	GameObject lowContent;
	[SerializeField]
	GameObject normalContent;
	[SerializeField]
	GameObject superContent;
	[SerializeField]
	MarketPlayItem prefabItem;
	private void Start()
	{
		
	}
}
