using Cysharp.Threading.Tasks;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using Patterns;
using Spine;

namespace PlayFabManager.Data
{
	public class PlayFabDataManager : Singleton<PlayFabDataManager>
	{
		private Dictionary<string, string> DataDictionary = new()
			{
				{ "ShaftManager", "" }, 
				{ "Elevator", "" },
				{ "Counter", "" },
				{ "ManagersController", "" },
				{ "PawVolume", "" },
				{ "SkinManager", "" },
				{ "LastTimeQuit", "" }
			};

		public static event Action LoadingIsDone;
		public async UniTask LoadData()
		{
			await Login();
			await GetDataFromPlayFab();
			LoadingIsDone?.Invoke();
		}
		private static async UniTask Login()
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
			}
			catch (Exception ex)
			{
				Debug.LogError($"Login Error: {ex.Message}");
			}

		}
		private async UniTask GetDataFromPlayFab()
		{
			var request = new GetUserDataRequest();
			var taskCompletionSource = new UniTaskCompletionSource<GetUserDataResult>();
			PlayFabClientAPI.GetUserData(request, result =>
			{
				taskCompletionSource.TrySetResult(result);

			}, error => {
				taskCompletionSource.TrySetException(new Exception(error.GenerateErrorReport()));
			});
			try
			{
				var result = await taskCompletionSource.Task;

				var keys = new List<string>(DataDictionary.Keys);

				foreach (var s in keys)
				{
					if (result.Data.ContainsKey(s))
					{
						string json = result.Data[s].Value;
						DataDictionary[s] = json;
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
			
			var request = new UpdateUserDataRequest { Data = DataDictionary };
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
		public void SaveData(string key, string value)
		{
			DataDictionary[key] = value;
		}
		public bool ContainsKey(string key) => DataDictionary.ContainsKey(key) && DataDictionary[key] != "";
		public string GetData(string key) => DataDictionary.ContainsKey(key) ? DataDictionary[key] : null;
	}
}
