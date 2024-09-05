using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using Newtonsoft.Json;
using static PlayFabDemo.UpdatePlayerStatButton;
namespace PlayFabDemo
{
	public class PlayFabManager : MonoBehaviour
	{
		public Donate donate;
		public TextMeshProUGUI donateList;
		public BuyButton buyButton;
		public UpdatePlayerStatButton updatePlayerStatButton;
		private void Start()
		{
			Login();
			donate.OnDonate += DonateToPublisher;
		}


		//dang nhap hoac dang ki tai khoang CreateAccount = true se tao ID moi neu khong tim thay custiomID
		void Login()
		{
			var request = new LoginWithCustomIDRequest
			{
				CustomId = SystemInfo.deviceUniqueIdentifier,
				CreateAccount = true,
			};
			PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
		}

		private void OnError(PlayFabError error)
		{
			Debug.Log("Error while logging in / Creating account!");
			Debug.Log(error.GenerateErrorReport());
		}

		private void OnSuccess(LoginResult result)
		{
			Debug.Log("Successful login/Acount create!");
			GetLeaderboard("Donate Leaderboard");
		}
		//Gui thong tin cap nhat toi leader board
		public void SendToLeaderboard(int amount)
		{
			var request = new UpdatePlayerStatisticsRequest
			{
				Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName = "Donate Leaderboard",
					Value = amount,
				}
			}
			};
			PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
		}

		private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
		{
			Debug.Log("Suseccfull Update!");
			StartCoroutine(DelayedLeaderboardFetch());
			GetLeaderboard("Donate Leaderboard");
		}
		private IEnumerator DelayedLeaderboardFetch()
		{
			yield return new WaitForSeconds(2);
			GetLeaderboard("Donate Leaderboard");
		}

		private void DonateToPublisher(int amount)
		{
			SendToLeaderboard(amount);
		}
		//Lay thong tin tu leader board
		public void GetLeaderboard(string nameLeaderboard)
		{
			var request = new GetLeaderboardRequest
			{
				StatisticName = nameLeaderboard,
				StartPosition = 0,
				MaxResultsCount = 10
			};
			PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
		}

		private void OnLeaderboardGet(GetLeaderboardResult result)
		{
			donateList.text = "";
			foreach (var item in result.Leaderboard)
			{
				donateList.text += $"{item.Position} {item.PlayFabId} {item.StatValue} \n";
			}
		}
		
		//lay du lieu tu User data du lieu rieng cua tung player
		public void GetAmountItem()
		{
			PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
		}

		private void OnDataRecieved(GetUserDataResult result)
		{
			Debug.Log("Recieved user data!");
			Dictionary<string, TextMeshProUGUI> itemDictionary = new();

			foreach (var item in buyButton.listItemBuy)
			{
				itemDictionary.Add(item.itemName.ToString(), item.currentAmount);
			}
			foreach (var item in result.Data)
			{
				string key = item.Key.ToString();
				if(itemDictionary.ContainsKey(key))
					itemDictionary[key].text = item.Value.Value;
				if(key == "Player")
				{
					PlayerStat player = JsonConvert.DeserializeObject<PlayerStat>(result.Data["Player"].Value);
					updatePlayerStatButton.SetUI(player);
				}
			}
			Debug.Log("All user data Recieved");
		}

		//Gui du lieu can luu tru toi User data du lieu danh rieng cho 1 player
		public void UpdateAmountItem()
		{
			Dictionary<string, string> itemDictionary = new();
			
			foreach (var item in buyButton.listItemBuy)
			{
				itemDictionary.Add(item.itemName.ToString(), item.amountBuy.text);
			}

			var request = new UpdateUserDataRequest
			{
				Data = itemDictionary,
			};
			PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
		}

		private void OnDataSend(UpdateUserDataResult result)
		{
			Debug.Log("Save data Successfull!");
		}
		//Cap nhat du lieu bang Json
		public void SavePlayerStat()
		{
			var request = new UpdateUserDataRequest
			{
				Data = new Dictionary<string, string>
				{
					{"Player", JsonConvert.SerializeObject(updatePlayerStatButton.ReturnClass())}
				}
			};
			PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
		}
	}
}
