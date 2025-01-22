using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class TutorialStateMachine : StateManager<TutorialState>
{

	public void InitState(TutorialState state, TutorialManager tutorialManager)
	{
	
		States.Add(TutorialState.State1, new TutorialState1(tutorialManager));
		TransitonToState(state);
	}
}
