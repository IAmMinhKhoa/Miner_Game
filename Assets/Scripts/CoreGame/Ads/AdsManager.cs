
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Google Modile Ad Service
public class AdsManager : Patterns.Singleton<AdsManager>
{
	// //Now available for android
	// [SerializeField]
	// private string RewardedAdUnitId = string.Empty;
	// private RewardedAd RewardedAd { get; set; }

	// // Start is called before the first frame update
	// void Start()
	// {
	// 	MobileAds.Initialize(initStatus =>
	// 	{

	// 	});
	// 	LoadRewardedAd();
	// 	RegisterRewardedAdEventHandler(RewardedAd);
	// }

	// #region RewardedAd
	// private void LoadRewardedAd()
	// {
	// 	if (RewardedAdUnitId == string.Empty)
	// 	{
	// 		Debug.Log("Ad unit id is empty");
	// 		return; 
	// 	}
	// 	AdRequest adRequest = new AdRequest();
	// 	RewardedAd.Load(RewardedAdUnitId, adRequest,
	// 		(RewardedAd ad, LoadAdError error) =>
	// 		{
	// 			if (error is not null || ad is null)
	// 			{
	// 				Debug.Log($"Rewarded ad failed to load an ad: {ad} with error: {error}");
	// 				return;
	// 			}
	// 			RewardedAd = ad;
	// 		}
	// 	);
	// }
	// public void ShowRewardedAd()
	// {
	// 	if (RewardedAd is not null && RewardedAd.CanShowAd())
	// 	{
	// 		RewardedAd.Show((Reward reward) =>
	// 		{
	// 			//Debug.Log($"Rewarded ad rewarded the user. Type: {reward.Type}, amount: {reward.Amount}");
	// 		});
	// 	}
	// }
	// private void RegisterRewardedAdEventHandler(RewardedAd rewardedAd)
	// {
	// 	rewardedAd.OnAdPaid += (AdValue adValue) =>
	// 	{
	// 		Debug.Log($"Rewarded ad has just earned: {adValue.Value} {adValue.CurrencyCode}");
	// 	};
	// 	rewardedAd.OnAdImpressionRecorded += () =>
	// 	{
	// 		//Debug.Log($"Rewarded ad recorded an impression");
	// 	};
	// 	rewardedAd.OnAdClicked += () =>
	// 	{
	// 		//Debug.Log($"Rewarded ad was clicked");
	// 	};
	// 	rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
	// 	{
	// 		Debug.Log($"Rewared ad failed to open with error: {adError}");
	// 	};
	// 	rewardedAd.OnAdFullScreenContentOpened += () =>
	// 	{
	// 		//Debug.Log("Reward ad opened");
	// 	};
	// 	rewardedAd.OnAdFullScreenContentClosed += () =>
	// 	{
	// 		Debug.Log("Reward ad closed");
	// 		RewardedAd.Destroy();
	// 		LoadRewardedAd();
	// 		RegisterRewardedAdEventHandler(RewardedAd);
	// 		PawManager.Instance.AddPaw(1000000000);
	// 	};
	// }
	// #endregion 
}
