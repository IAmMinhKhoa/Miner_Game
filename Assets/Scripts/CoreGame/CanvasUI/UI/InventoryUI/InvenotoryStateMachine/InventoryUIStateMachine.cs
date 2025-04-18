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
				States.Add(InventoryItemType.ShaftWaitTable, new ChangWaitalbeState(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.BarCounter, new ChangeShaftBarCounter(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.Elevator, new ChangeElevatorState(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.ElevatorBg, new ChangeElevatorBG(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.ShaftCart, new ChangeShaftCartState(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.CounterCart, new ChangeCounterCartState(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.CashierCounter, new ChangeCashierCounter(inventoryUI.pOIController, inventoryUI.popupOtherItemPrefab));
				States.Add(InventoryItemType.ElevatorCharacter, new ChangeElevatorStaffState(inventoryUI.StaffSkin));
				States.Add(InventoryItemType.CounterCharacter, new ChangeCounterStaffState(inventoryUI.StaffSkin));
				States.Add(InventoryItemType.ShaftCharacter, new ChangeShaftStaffState(inventoryUI.StaffSkin));
				States.Add(InventoryItemType.CounterBg, new ChangeCounterBG(inventoryUI.BGList, inventoryUI.bgCounterPrefab));
				States.Add(InventoryItemType.CounterSecondBg, new ChangeCounterSecondBG(inventoryUI.BGList, inventoryUI.bgCounterPrefab));
				States.Add(InventoryItemType.ShaftBg, new ChangeShaftBG(inventoryUI.BGList, inventoryUI.bgCounterPrefab));
				States.Add(InventoryItemType.ShaftSecondBg, new ChangeShaftSecondBG(inventoryUI.BGList, inventoryUI.bgCounterPrefab));
				
				CurrentState = States[InventoryItemType.ShaftWaitTable];
			}
		}
	}
}
