using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEditorInternal;

namespace UI.Inventory
{
	public class InventoryUIStateMachine : StateManager<InventoryItemType>
	{
		private void Awake()
		{
			if (TryGetComponent<InventoryUIManager>(out var inventoryUI))
			{
				States.Add(InventoryItemType.ShaftWaitTable, new ChangWaitalbeState(inventoryUI.pOIController));
				States.Add(InventoryItemType.Elevator, new ChangeElevatorState(inventoryUI.pOIController));
				States.Add(InventoryItemType.ShaftCart, new ChangeShaftCartState(inventoryUI.pOIController));
				States.Add(InventoryItemType.CounterCart, new ChangeCounterCartState(inventoryUI.pOIController));
				CurrentState = States[InventoryItemType.ShaftWaitTable];
			}
		}
	}
}
