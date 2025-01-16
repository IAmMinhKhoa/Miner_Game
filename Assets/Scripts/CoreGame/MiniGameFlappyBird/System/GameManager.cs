using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Patterns.Singleton<GameManager>
{
    public InGameUI gameUI;
    private bool gameover;
    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameUI.canvasGroup.alpha = 0;
            gameUI.gameObject.SetActive(false);
        }
    }
    public IEnumerator GameOver()
    {
        Time.timeScale = 0;
        gameUI.canvasGroup.alpha = 0.5f;
        gameUI.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
		//SceneManager.LoadScene(0);
		Time.timeScale = 1;
		PoolManager.Instance.dic_pool["Pipe"].DesSpawnedAll();
		SceneManager.UnloadScene(2);
	}

}
