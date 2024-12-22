using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
	[SerializeField] private int index;
	public void OnPause()
	{
		PauseDialogParam pauseDialogParam = new PauseDialogParam { index = index };
		DialogManager.Instance.ShowDialog(DialogIndex.PauseDialog, pauseDialogParam);
	}	
}
