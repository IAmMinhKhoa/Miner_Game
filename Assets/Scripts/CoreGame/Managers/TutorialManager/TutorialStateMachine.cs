using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

using System.Linq;
public class TutorialStateMachine : StateManager<TutorialState>
{
	public void InitState(TutorialState state, TutorialManager tutorialManager)
	{

		States.Add(TutorialState.State1, new TutorialState1(tutorialManager));
		States.Add(TutorialState.State2 , new TutorialState2(tutorialManager));
		States.Add(TutorialState.State3 , new TutorialState3(tutorialManager));

		TransitonToState(state);
	}
	public void TriggerClickableStates(int state)
	{
		States[(TutorialState)state].Do();
	}
}
