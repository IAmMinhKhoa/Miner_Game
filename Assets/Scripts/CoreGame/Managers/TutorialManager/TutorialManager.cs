using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
	[Header("UI")]
	[SerializeField] GameUI gameUI;
	[SerializeField] TutotrialUI tutotrialUI;
	//[Header("State1")]
	public bool isTuroialing { set; get; } = false; 
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
		Triggertutorial(1);
	}
	private readonly TutorialStateMachine tutorialStateMachine = new();
	public void Triggertutorial(int state)
	{
		isTuroialing = true; 
		tutorialStateMachine.InitState((TutorialState)state, this);
	}
	public void ShowHideGameUI(bool enable)
	{
		gameUI.ButtonesInteract(enable);
	}
}
public enum TutorialState
{
	State1 = 1,
	State2 = 2,
	State3 = 3,
	State4 = 4,
}
