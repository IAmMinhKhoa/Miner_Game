
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabManager.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
	private async void OnEnable()
	{
		await FetchVirtualCurrencies();
		RewardCoin();
	}
	public async UniTask FetchVirtualCurrencies()
	{
		try
		{
			var inventoryResult = await GetUserInventoryAsync();
			scoreText.text = "Xu: " + inventoryResult.VirtualCurrency["MC"] + "";
			scoreChangeMachineText.text = inventoryResult.VirtualCurrency["MC"] + "";

		}
		catch (Exception ex)
		{
			Debug.LogError($"Error fetching inventory: {ex.Message}");
		}
	}

	public async Task<GetUserInventoryResult> GetUserInventoryAsync()
	{
		var taskCompletionSource = new TaskCompletionSource<GetUserInventoryResult>();

		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
			result => taskCompletionSource.SetResult(result),
			error => taskCompletionSource.SetException(new Exception(error.GenerateErrorReport()))
		);

		return await taskCompletionSource.Task;
	}

	private void RewardCoin()
	{
		const int coinReward = 200;
		const int rewardCooldownMinutes = 30;

		string lastTimeCoinReward = PlayFabDataManager.Instance.GetData("LastTimeCoinReward");
		var now = DateTime.Now;

		if (now.Hour is 3 or 11 or 19 && now.Minute <= 30 && ShouldGrantReward(lastTimeCoinReward, now, rewardCooldownMinutes))
		{
			GrantVirtualCurrency(coinReward);
			PlayFabDataManager.Instance.SaveData("LastTimeCoinReward", now.ToString());
			GetVirtualCurrencies();
		}
	}

	private bool ShouldGrantReward(string lastTime, DateTime now, int cooldownMinutes)
	{
		if (string.IsNullOrEmpty(lastTime))
			return true;

		TimeSpan timeOffset = now - DateTime.Parse(lastTime);
		return timeOffset.TotalMinutes > cooldownMinutes;
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
		Debug.Log("ShowData");
		scoreText.text = "Xu: "+result.VirtualCurrency["MC"] + "";
		scoreChangeMachineText.text = result.VirtualCurrency["MC"] + "";
	}

	private void OnError(PlayFabError error)
	{
		Debug.Log(error);
	}
}
