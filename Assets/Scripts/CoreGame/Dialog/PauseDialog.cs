using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PauseDialog : BaseDialog
{
	private int index;
	public override void Setup(DialogParam data)
	{
		PauseDialogParam param = data as PauseDialogParam;
		index = param.index;
	}
	public override void OnShowDialog()
	{
		Time.timeScale= 0f;
	}
	public override void OnHideDialog()
	{
		Time.timeScale = 1f;
	}
	public void OnResume()
	{
		DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
	}
	public void OnBack()
	{
		DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
		SceneManager.UnloadScene(index);
	}	
}
