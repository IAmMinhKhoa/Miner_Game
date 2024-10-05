using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
//Google Modile Ad Service
public class AdsManager : Patterns.Singleton<AdsManager>
{
	[Header("Rewarded Ad")]
	[SerializeField]
	private string RewardedAdUnitIdAndroid = string.Empty;
	[SerializeField]
	private string RewardedAdUnitIdIos = string.Empty;
	[Header("Banner Ad")]
	[SerializeField]
	private string BannerAdUnitIdAndroid = string.Empty;
	[SerializeField]
	private string BannerAdUnitIdIos = string.Empty;

	private RewardedAd _RewardedAd { get; set; }

	private BannerView _BannerView { get; set; }

	// Start is called before the first frame update
	void Start()
	{
		MobileAds.Initialize(initStatus =>
		{

		});
		//RequestConfiguration request = new RequestConfiguration();
		//request.TestDeviceIds.Add("4E03DC2B53D57237BC3ACF0AEFB9BB32");
		//MobileAds.SetRequestConfiguration(request);
		LoadBannerAd();
		LoadRewardedAd();
	}

	#region RewardedAd
	private void LoadRewardedAd()
	{
		string adUnitId = string.Empty;
#if UNITY_ANDROID
		adUnitId = RewardedAdUnitIdAndroid;
#elif UNITY_IPHONE
		adUnitId = RewardedAdUnitIdIos;
#endif
		if (adUnitId == string.Empty)
		{
			//Debug.Log("Rewarded Ad: Empty Ad Unit Id");
			return;
		}
		if (_RewardedAd is not null)
		{
			_RewardedAd.Destroy();
			_RewardedAd = null;
		}
		AdRequest adRequest = new AdRequest();
		RewardedAd.Load(adUnitId, adRequest,
			(RewardedAd ad, LoadAdError error) =>
			{
				if (error is not null || ad is null)
				{
					Debug.Log($"Rewarded ad failed to load an ad: {ad} with error: {error}");
					return;
				}
				_RewardedAd = ad;
				RegisterRewardedAdEventHandler(_RewardedAd);
			}
		);
	}
	private void RegisterRewardedAdEventHandler(RewardedAd rewardedAd)
	{
		rewardedAd.OnAdPaid += (AdValue adValue) =>
		{
			//Debug.Log($"Rewarded ad has just earned: {adValue.Value} {adValue.CurrencyCode}");
		};
		rewardedAd.OnAdImpressionRecorded += () =>
		{
			//Debug.Log($"Rewarded ad recorded an impression");
		};
		rewardedAd.OnAdClicked += () =>
		{
			//Debug.Log($"Rewarded ad was clicked");
		};
		rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
		{
			Debug.LogWarning($"Rewared ad failed to open with error: {adError}");
		};
		rewardedAd.OnAdFullScreenContentOpened += () =>
		{
			//Debug.Log("Reward ad opened");
		};
		rewardedAd.OnAdFullScreenContentClosed += () =>
		{
			Debug.LogWarning("Rewarded ad closed");
			PawManager.Instance.AddPaw(1000000000);
			LoadRewardedAd();
		};
	}
	public void ShowRewardedAd()
	{
		if (_RewardedAd is not null && _RewardedAd.CanShowAd())
		{
			_RewardedAd.Show((Reward reward) =>
			{
				//Debug.Log($"Rewarded ad rewarded the user. Type: {reward.Type}, amount: {reward.Amount}");
			});
		}
	}
	#endregion
	#region BannerAd
	public void LoadBannerAd()
	{
		string adUnitId = string.Empty;
#if UNITY_ANDROID
		adUnitId = BannerAdUnitIdAndroid;
#elif UNITY_IPHONE
		adUnitId = BannerAdUnitIdIos;
#endif
		if (adUnitId == string.Empty)
		{
			//Debug.LogWarning("Banner Ad: Empty Ad Unit Id");
			return;
		}
		if (_BannerView is not null)
		{
			_BannerView.Destroy();
			_BannerView = null;
		}
		_BannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
		RegisterBannerAdEventHandler(_BannerView);
		AdRequest adRequest = new AdRequest();
		_BannerView.LoadAd(adRequest);
	}
	private void RegisterBannerAdEventHandler(BannerView bannerView)
	{
		bannerView.OnAdPaid += (AdValue adValue) =>
		{

		};
		bannerView.OnAdImpressionRecorded += () =>
		{

		};
		bannerView.OnAdClicked += () =>
		{

		};
		bannerView.OnBannerAdLoaded += () =>
		{
			
		};
		bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
		{
			Debug.LogWarning($"Banner view failed to load an ad with error : {error}");
		};
		bannerView.OnAdFullScreenContentOpened += () =>
		{

		};
		bannerView.OnAdFullScreenContentClosed += () =>
		{
			LoadBannerAd();
		};
	}
	
#endregion
}
