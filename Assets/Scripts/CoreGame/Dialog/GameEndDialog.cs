using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndDialog : BaseDialog
{
	public TMP_Text score;
	private int index;
	public override void Setup(DialogParam data)
	{
		GameEndDialogParam param = data as GameEndDialogParam;
		score.text ="Score: " +  param.score.ToString();
		index = param.index;
	}
	public void OnPlayAgain()
	{
		Time.timeScale = 1f;
		DialogManager.Instance.HideDialog(DialogIndex.GameEndDialog);
		Scene minigameScene = SceneManager.GetSceneByBuildIndex(index);
		if (minigameScene.isLoaded)
		{
			SceneManager.UnloadSceneAsync(minigameScene);
		}
		SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
	}	
	public void OnExit()
	{
		Time.timeScale = 1f;
		DialogManager.Instance.HideDialog(DialogIndex.GameEndDialog);
		SceneManager.UnloadScene(index);
	}
}
