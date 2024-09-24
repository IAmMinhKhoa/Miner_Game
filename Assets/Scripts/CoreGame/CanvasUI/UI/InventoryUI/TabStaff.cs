using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
	
    public class TabStaff : MonoBehaviour
    {
		public event Action OnTabStaffEnable, OnTabStaffDisable;
		private void OnEnable()
		{
			OnTabStaffEnable?.Invoke();
		}
		private void OnDisable()
		{
			OnTabStaffDisable?.Invoke();
		}
	}
}
