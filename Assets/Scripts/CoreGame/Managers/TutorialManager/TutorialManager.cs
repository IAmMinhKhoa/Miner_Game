using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
	[Header("UI")]
	[SerializeField] public GameUI gameUI;

	//[Header("State1")]
	public bool isTuroialing { set; get; } = false; 
	public TutorialStateMachine TutorialStateMachine { private set; get; }
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	
	public void Triggertutorial(int state)
	{
		isTuroialing = true;
		this.TutorialStateMachine = new();
		TutorialStateMachine.InitState((TutorialState)state, this);
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
