using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	private TutorialStateMachine tutorialStateMachine;
	public void Triggertutorial(int state)
	{
		tutorialStateMachine.InitState((TutorialState)state);
	}
}
public enum TutorialState
{
	State1 = 1,
	State2 = 2,
	State3 = 3,
	State4 = 4,
}
