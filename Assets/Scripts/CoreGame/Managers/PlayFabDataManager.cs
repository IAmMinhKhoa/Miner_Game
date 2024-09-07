using Cysharp.Threading.Tasks;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace PlayFabManager.Data
{
	public class PlayFabDataManager : MonoBehaviour
	{
		private readonly HashSet<string> listData = new()
				{
					"ShaftManager",
					"Elevator",
					"Counter",
					"ManagersController",
					"PawVolume",
					"SkinManager",
					"LastTimeQuit"
				};
		public async UniTask LoadData()
		{

			await Login();
			await GetData();
		}
		private async UniTask Login()
		{
			var request = new LoginWithCustomIDRequest
			{
				CustomId = SystemInfo.deviceUniqueIdentifier,
				CreateAccount = true,
			};

			var taskCompletionSource = new UniTaskCompletionSource<LoginResult>();

			PlayFabClientAPI.LoginWithCustomID(request, result =>
			{
				taskCompletionSource.TrySetResult(result);
			}, error =>
			{
				taskCompletionSource.TrySetException(new Exception(error.GenerateErrorReport()));
			});

			try
			{
				var result = await taskCompletionSource.Task;
				//Debug.Log("Login Successful");
			}
			catch (Exception ex)
			{
				Debug.LogError($"Login Error: {ex.Message}");
			}

		}
		private async UniTask GetData()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
			var request = new GetUserDataRequest();
			var taskCompletionSource = new UniTaskCompletionSource<GetUserDataResult>();
			PlayFabClientAPI.GetUserData(request, result =>
			{
				taskCompletionSource.TrySetResult(result);

			}, error => {
			});
			try
			{
				var result = await taskCompletionSource.Task;
				foreach (var item in listData)
				{
					string s = item.ToString();
					if (result.Data.ContainsKey(s))
					{
						string json = result.Data[s].Value;
						PlayerPrefs.SetString(s, json);
						PlayerPrefs.Save();
					}
				}

			}
			catch (Exception ex)
			{
				Debug.LogError($"Login Error: {ex.Message}");
			}
			
		}
		public async UniTask SendDataBeforeExit()
		{
			// Sending the data to PlayFab
			Dictionary<string, string> data = new();
			foreach (var item in listData)
			{
				string s = item.ToString();
				if (PlayerPrefs.HasKey(s))
				{
					string json = PlayerPrefs.GetString(s);
					data[s] = json;
				}
			}

			var request = new UpdateUserDataRequest { Data = data };
			var taskCompletionSource = new UniTaskCompletionSource<bool>();

			PlayFabClientAPI.UpdateUserData(request, result =>
			{
				Debug.Log("Data successfully sent before exit!");
				taskCompletionSource.TrySetResult(true);
			},
			error =>
			{
				Debug.LogError("Error sending data: " + error.GenerateErrorReport());
				taskCompletionSource.TrySetResult(false);
			});
			await taskCompletionSource.Task;

		}

	}
}
