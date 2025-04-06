using Cysharp.Threading.Tasks;
using System;
using TMPro;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoaddingScreenManager : MonoBehaviour
{
	[SerializeField] private Image loadingBar;
	[SerializeField]
	TMP_Text currentLoading;
	[SerializeField]
	private int frameRequireToLoad;

	private float currentLoad = 0f;


	private float loadingDuration = 5f;  // tổng thời gian loading
	private float elapsedTime = 0f;

	private float checkInterval = 1f;    // thời gian giữa các lần check mạng
	private float timer = 0f;

	private bool loadingComplete = false;

	private void Start()
	{
		loadingBar.fillAmount = 0f;
	}

	private void Update()
	{
		if (loadingComplete) return;

		// Tăng thời gian đã trôi qua
		elapsedTime += Time.deltaTime;
		float progress = Mathf.Clamp01(elapsedTime / loadingDuration);

		// Cập nhật thanh loading
		loadingBar.fillAmount = progress;

		// Cập nhật text phần trăm (0–100)
		int percent = Mathf.RoundToInt(progress * 100f);
		currentLoading.text ="Loading "+ percent.ToString() + "%";

		// Kiểm tra mạng mỗi 1 giây
		timer += Time.deltaTime;
		if (timer >= checkInterval)
		{
			timer = 0f;
			// (Tuỳ ý) Kiểm tra mạng hoặc xử lý khác ở đây
		}

		// Nếu xong thời gian thì kiểm tra điều kiện hoàn tất
		if (elapsedTime >= loadingDuration)
		{
			loadingComplete = Common.CheckInternetConnection(); // chỉ chuyển scene nếu có mạng

			if (loadingComplete)
			{
				Debug.Log("Loading complete!");
				// TODO: Load scene hoặc hiển thị UI
			}
			else
			{
				currentLoading.text = "SomeWrong";
			}
		}
	}

	public async UniTask FullLoadingBar()
	{
		while (!loadingComplete)
		{
			await UniTask.DelayFrame(1); // Đợi 1 frame rồi tiếp tục (mượt hơn Delay)
		}
		loadingBar.fillAmount = 1;
		await UniTask.Delay(100);
	}
}
