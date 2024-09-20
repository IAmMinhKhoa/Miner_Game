using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoaddingScreenManager : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer notFullLoadingBar;
	[SerializeField]
	private SpriteRenderer fullLoadingBar;
	[SerializeField]
	TextMeshPro currentLoading;
	[SerializeField]
	private int frameRequireToLoad;

	private float currentLoad = 0f;
	private float totalLoad = 0f;
	private int framePassed = 0;
	private bool isLoading = false;
	private void Start()
	{
		totalLoad = notFullLoadingBar.size.x;

	}
	private void Update()
	{
		bool allowToLoad = Common.CheckInternetConnection();
		if (allowToLoad)
		{
			if (isLoading == false)
			{
				framePassed++;
				currentLoad += totalLoad * 0.0005f;
			}

			fullLoadingBar.size = new Vector2(currentLoad, fullLoadingBar.size.y);
			currentLoading.text = "Loading " + Mathf.FloorToInt(currentLoad / totalLoad * 100f) + "%";
		}
		else
		{
			fullLoadingBar.size = new Vector2(0, fullLoadingBar.size.y);
			currentLoading.text = "No Internet Connection";
		}
	}
	public async UniTask FullLoadingBar()
	{
		bool allowToLoad = Common.CheckInternetConnection();
		if(!allowToLoad) return;
		isLoading = true;
		float valuePerfamre = (float)((totalLoad - currentLoad) / (frameRequireToLoad - framePassed));
		while (framePassed <= frameRequireToLoad)
		{
			framePassed++;
			currentLoad += valuePerfamre;
			await UniTask.Yield();
		}
		fullLoadingBar.size = new Vector2(totalLoad, fullLoadingBar.size.y);
		await UniTask.Delay(500);
	}
}
