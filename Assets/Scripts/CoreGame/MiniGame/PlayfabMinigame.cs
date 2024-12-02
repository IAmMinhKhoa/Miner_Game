
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayfabMinigame : MonoBehaviour
{
	public static PlayfabMinigame Instance;
	public TextMeshProUGUI scoreText, scoreChangeMachineText;

	private void Awake()
	{
		Instance = this;
	}
	private void OnEnable()
	{
		GetVirtualCurrencies();
	}

	public void GetVirtualCurrencies()
	{
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
	}

	public void GrantVirtualCurrency(int value)
	{
		var request = new AddUserVirtualCurrencyRequest
		{
			VirtualCurrency = "MC",
			Amount = value
		};
		PlayFabClientAPI.AddUserVirtualCurrency(request, OnGrandVirtualCurrencySuccess, OnError);
	}

	

	private void OnGrandVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
	{
		
	}

	private void OnGetUserInventorySuccess(GetUserInventoryResult result)
	{
		scoreText.text = "Xu: "+result.VirtualCurrency["MC"] + "";
		scoreChangeMachineText.text = result.VirtualCurrency["MC"] + "";
	}

	private void OnError(PlayFabError error)
	{
		Debug.Log(error);
	}
}
