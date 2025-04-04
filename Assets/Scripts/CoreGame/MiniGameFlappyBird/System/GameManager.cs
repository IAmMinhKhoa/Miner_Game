using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Patterns.Singleton<GameManager>
{
	public InGameUI gameUI;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			gameUI.canvasGroup.alpha = 0;
			gameUI.gameObject.SetActive(false);
		}
	}
	public void GameOver()
	{
		PawManager.Instance.AddPaw(1000000000);
		Time.timeScale = 0;
		PoolManager.Instance.dic_pool["Pipe"].DesSpawnedAll();
		GameEndDialogParam gameEndDialogParam = new GameEndDialogParam { score = PlayerController.score,index=1 };
		DialogManager.Instance.ShowDialog(DialogIndex.GameEndDialog,gameEndDialogParam);
	}
}
