using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState1 : BaseTutorialState
{
	public TutorialState1(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}
	public override void Do()
	{
		
	}

	public override void Enter()
	{
		tutorialManager.ShowHideGameUI(false);
	}

	public override void Exit()
	{
		
	}
}
