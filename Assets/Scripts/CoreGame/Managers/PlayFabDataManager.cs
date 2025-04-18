using Cysharp.Threading.Tasks;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using Patterns;
using Spine;
using NOOD;

namespace PlayFabManager.Data
{
	public class PlayFabDataManager : Singleton<PlayFabDataManager>
	{
		[SerializeField]
		private GameObject loadingPrefab => GameData.Instance.InstantiatePrefab(PrefabEnum.ModelLoading);
		private LoaddingScreenManager loadingScene;
		[SerializeField] private Transform gameUI;
		private bool isDataLoaded = false;
		public string accountID;
		private void Start()
		{
			
			//Camera.main.cullingMask = LayerMask.GetMask("LoadingScene");
		}
		private Dictionary<string, string> DataDictionary;
		public async UniTask LoadData()
		{

			DataDictionary = new()
			{
				{ "ShaftManager", "" },
				{ "Elevator", "" },
				{ "Counter", "" },
				{ "ManagersController", "" },
				{ "PawVolume", "" },
				{ "SkinManager", "" },
				{ "LastTimeQuit", "" },
				{ "LastTimeCoinReward", "" },
				{ "TutorialState", "1" },
			};
			if (loadingScene == null)
			{
				var go = Instantiate(GameData.Instance.InstantiatePrefab(PrefabEnum.ModelLoading), gameUI);
				loadingScene = go.GetComponent<LoaddingScreenManager>();
			}
			await Login();
			await GetDataFromPlayFab();
			await loadingScene.FullLoadingBar();
			GameUI.Instance.OpenModalShowEvent();
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
				accountID=result.PlayFabId;
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

				if (result.Data.Count > 0)
				{
					if(result.Data.ContainsKey("TutorialState") && result.Data["TutorialState"].Value != "2")
					{
						foreach (var s in keys)
						{
							if (result.Data.ContainsKey(s))
							{
								string json = result.Data[s].Value;
								DataDictionary[s] = json;
							}
						}
					} else
					{
						DataDictionary["TutorialState"] = "2";
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

			if(isDataLoaded == true)
			{
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
		public void SaveData(string key, string value)
		{
			if(!DataDictionary.ContainsKey(key))
			{
				Debug.Log(key + " Not found");
			}
			DataDictionary[key] = value;
		}
		public bool ContainsKey(string key) => DataDictionary.ContainsKey(key) && DataDictionary[key] != "";
		public string GetData(string key) => DataDictionary.ContainsKey(key) ? DataDictionary[key] : "";
		//Su dung de but data lay ve tu playfab
		/*public void FectchData()
		{
			Debug.Log("Fectch data called -------------------------------------");
			foreach (var key in DataDictionary.Keys)
			{
				Debug.Log(key + "-------------------------------------" + DataDictionary[key]);
			}
		}*/
		public void GoToMainGame()
		{
			isDataLoaded = true;
			Camera.main.cullingMask = -1;
			loadingScene.gameObject.SetActive(false);
		}
	}
}
