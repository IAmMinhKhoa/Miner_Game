using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DialogIndex
{
    GameEndDialog = 0,
}
public class DialogParam
{

}
public class GameEndDialogParam : DialogParam
{
	public int score;
	public int index;
}

public class DialogConfig
{
    public static DialogIndex[] dialogIndices = {

        DialogIndex.GameEndDialog,
    };

}
