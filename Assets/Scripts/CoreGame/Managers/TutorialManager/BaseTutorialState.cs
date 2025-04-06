using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public abstract class BaseTutorialState : BaseState<TutorialState>
{
	protected TutorialManager tutorialManager;
	public BaseTutorialState(TutorialManager tutorialManager)
	{
		this.tutorialManager = tutorialManager;
	}
	public abstract override void Do();

	public abstract override void Enter();

	public abstract override void Exit();

}
