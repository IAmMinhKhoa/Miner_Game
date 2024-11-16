using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerController : Patterns.Singleton<BannerController>
{
	[Header("UI Element")]
	[SerializeField] TMP_Text textTitle;
	[SerializeField] Image backBackground;
	[SerializeField] Image frontBackground;
	[SerializeField] GameObject modalUI;
	[Header("Data")]
	public BannerSO bannerData;

	public BannerEntity curBannerEntity { set; get; }
	private void Start()
	{
		LoadBannerData();

	}

	public void ApplyToWorldUI(BannerEntity banner)
	{
		textTitle.text = banner.title;
		textTitle.color = banner.color;
		textTitle.font = bannerData.dataFontText[banner.indexFont];
		backBackground.sprite = bannerData.dataSprite_2[banner.IDBack].templateDetails[banner.indexBrBack].sprite;
		frontBackground.sprite = bannerData.dataSprite_1[banner.IDFront].templateDetails[banner.indexBrFront].sprite;
	}
	#region Load Method

	public void LoadBannerData()
	{
		// Check if banner data exists in PlayerPrefs
		if (PlayerPrefs.HasKey("BannerData"))
		{
			string json = PlayerPrefs.GetString("BannerData");
			BannerEntity banner = JsonUtility.FromJson<BannerEntity>(json);
			curBannerEntity = banner;
			ApplyToWorldUI(banner);
			Debug.Log("Data loaded from PlayerPrefs.");
		}
		else
		{
			Debug.Log("No saved data found in PlayerPrefs. Creating initial data.");

			// Create default initial data
			BannerEntity initialBanner = new BannerEntity(
				title: "Animal Bubble Tea",
				color: Color.white,
				indexFont: 0,
				indexBrBack: 0,
				IDBack : 0,
				IDFront : 0,
				indexBrFront: 0
			);

			// Save initial data to PlayerPrefs
			string json = JsonUtility.ToJson(initialBanner);
			PlayerPrefs.SetString("BannerData", json);
			PlayerPrefs.Save();
			curBannerEntity = initialBanner;
			// Apply default data to the UI
			ApplyToWorldUI(initialBanner);
		}
	}


	#endregion
	#region Support
	public void OpenModal()
	{
		modalUI.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		modalUI.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		modalUI.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);
	}
	public void CloseModal()
	{
	
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		modalUI.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			modalUI.SetActive(false);
		});
	}
	#endregion

}





