using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class StateManager<EState> : MonoBehaviour where EState : Enum
	{
		protected Dictionary<EState, BaseState<EState>> States = new();
		protected BaseState<EState> CurrentState;

		public void TransitonToState(EState stateKey)
		{
			if(CurrentState != null)
			{
				CurrentState.Exit();
			}
			CurrentState = States[stateKey];
			CurrentState.Enter();
		}
	}
}
