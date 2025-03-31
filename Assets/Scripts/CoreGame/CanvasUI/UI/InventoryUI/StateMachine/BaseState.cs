using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public abstract class BaseState<EState> where EState : Enum
	{

		public abstract void Enter();
		public abstract void Exit();
		public abstract void Do();

	}

}
