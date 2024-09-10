using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class LoaddingScreenManager : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer notFullLoadingBar;
	[SerializeField]
	private SpriteRenderer fullLoadingBar;
	[SerializeField]
	TextMeshPro currentLoading;
	
	private float currentLoad = 0f;
	private float totalLoad = 0f;
	private bool isLoading = false;
	private void Start()
	{
		totalLoad = notFullLoadingBar.size.x;
	}
	private void Update()
	{
		if(isLoading == false)
			currentLoad += totalLoad * 0.0005f;
		fullLoadingBar.size  = new Vector2(currentLoad, fullLoadingBar.size.y);
		currentLoading.text = Mathf.FloorToInt(currentLoad / totalLoad * 100f) + "%";
	}
	public async UniTask FullLoadingBar()
	{
		isLoading = true;
		while(currentLoad < totalLoad )
		{
			currentLoad += totalLoad * 0.001f;
			await UniTask.Yield();
		}
		fullLoadingBar.size = new Vector2(totalLoad, fullLoadingBar.size.y);
		await UniTask.Delay(500);
		Destroy(gameObject);
	}
}
