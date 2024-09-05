using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAdButton : MonoBehaviour
{
	private AdsManager AdsManager { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        AdsManager = this.GetComponent<AdsManager>();
		this.GetComponent<Button>().onClick.AddListener(ShowAd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void ShowAd()
	{
		AdsManager.ShowRewardedAd();
	}
}
