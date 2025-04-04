using Cysharp.Threading.Tasks;
using System;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Tree;
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
	[SerializeField]
	private Transform _spineLogo;
	private float currentLoad = 0f;
	private float totalLoad = 0f;
	private int framePassed = 0;
	private bool isLoading = false;
	private bool allowToLoad;
	private void Start()
	{
		totalLoad = notFullLoadingBar.size.x;
		if (Common.CheckDevice())
		{
			Vector3 currentPosition = _spineLogo.localPosition;

			// Dịch chuyển trục Y trừ xuống 50
			_spineLogo.localPosition = new Vector3(currentPosition.x, currentPosition.y +2.5f, currentPosition.z);
			_spineLogo.localScale = new Vector3(0.15f, 0.15f, 0.15f	);
		}

	}
	private void Update()
	{
		allowToLoad = Common.CheckInternetConnection();
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
		await UniTask.Delay(100);
	}
}
