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
		AdsManager.Instance.ShowRewardedAd(BoostManager.Instance.BonusAdsBoost);
	}
}
