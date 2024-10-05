using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAdButton : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		this.GetComponent<Button>().onClick.AddListener(ShowAd);
	}
	private void ShowAd()
	{
		Debug.LogWarning("Ad button clicked");
		AdsManager.Instance.ShowRewardedAd();
		//AdsManager.Instance.LoadBannerAd();
	}
}
