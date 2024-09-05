using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlayFabDemo
{
	public class Donate : MonoBehaviour
	{
		TMP_InputField inputField;
		public event Action<int> OnDonate;
		int currentAmount = 0;
		private void Start()
		{
			inputField = GetComponent<TMP_InputField>();
		}
		public void OnClickDonate()
		{
			OnDonate?.Invoke(currentAmount);
		}
		public void UpdateCurrentAmount()
		{
			currentAmount = int.Parse(inputField.text);
		}
	}
}
