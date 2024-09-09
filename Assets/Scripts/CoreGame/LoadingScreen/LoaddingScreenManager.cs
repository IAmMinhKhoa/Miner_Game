using PlayFabManager.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaddingScreenManager : MonoBehaviour
{

	private bool isSceneLoaded = false;
    void Start()
    {
		PlayFabManager.Data.PlayFabDataManager.LoadingIsDone += OnHandleLoadingData;
		StartCoroutine(LoadSceneAsync("MainGame"));
	}
	private void OnDisable()
	{
		PlayFabDataManager.LoadingIsDone -= OnHandleLoadingData;
	}


	private IEnumerator LoadSceneAsync(string  sceneName)
	{
		
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		while (!loadOperation.isDone)
		{
			Debug.Log(Mathf.Clamp01(loadOperation.progress / 0.9f));
			yield return null;
		}
		

	}
	private void OnHandleLoadingData()
	{
		StartCoroutine(UnloadCurrentSceneCoroutine());
	}
	private IEnumerator UnloadCurrentSceneCoroutine()
	{
		string currentSceneName = SceneManager.GetActiveScene().name;
		AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentSceneName);
		while (!unloadOperation.isDone)
		{
			yield return null;
		}
	}

}
