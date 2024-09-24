using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


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
				States.Add(InventoryItemType.ElevatorCharacter, new ChangeElevatorStaffState(inventoryUI.StaffSkin));
				States.Add(InventoryItemType.CounterCharacter, new ChangeCounterStaffState(inventoryUI.StaffSkin));
				States.Add(InventoryItemType.ShaftCharacter, new ChangeShaftStaffState(inventoryUI.StaffSkin));
				CurrentState = States[InventoryItemType.ShaftWaitTable];
			}
		}
	}
}
