using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DialogIndex
{
    GameEndDialog = 0,
	PauseDialog=1,
}
public class DialogParam
{

}
public class GameEndDialogParam : DialogParam
{
	public int score;
	public int index;
}
public class PauseDialogParam : DialogParam
{
	public int index;
}
public class DialogConfig
{
    public static DialogIndex[] dialogIndices = {

        DialogIndex.GameEndDialog,
		DialogIndex.PauseDialog,
    };

}
