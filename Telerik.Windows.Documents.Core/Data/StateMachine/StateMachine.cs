using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Data.StateMachine
{
	class StateMachine<TArgs>
	{
		public StateMachine()
		{
			this.transitions = new TransitionsCollection<TArgs>();
			this.states = new StatesCollection<TArgs>();
		}

		public StatesCollection<TArgs> States
		{
			get
			{
				return this.states;
			}
		}

		public TransitionsCollection<TArgs> Transitions
		{
			get
			{
				return this.transitions;
			}
		}

		public State<TArgs> CurrentState { get; set; }

		public void Start(State<TArgs> initialState, TArgs arguments)
		{
			this.CurrentState = initialState;
			this.CurrentState.Start(arguments);
			this.GoToNextState(arguments);
		}

		public bool GoToNextState(TArgs arguments)
		{
			IList<Transition<TArgs>> allTransitionsForState = this.Transitions.GetAllTransitionsForState(this.CurrentState);
			for (int i = 0; i < allTransitionsForState.Count; i++)
			{
				Transition<TArgs> transition = allTransitionsForState[i];
				if (transition.Predicate(arguments))
				{
					return this.GoToState(transition.ToState, arguments);
				}
			}
			return false;
		}

		public bool GoToState(State<TArgs> state, TArgs arguments)
		{
			if (this.CurrentState == state)
			{
				this.CurrentState.Stay(arguments);
			}
			else
			{
				this.CurrentState.Leave(arguments);
				this.CurrentState = state;
				this.CurrentState.Enter(arguments);
			}
			return true;
		}

		public void Stop(TArgs arguments)
		{
			this.CurrentState.Stop(arguments);
			this.CurrentState = null;
		}

		readonly TransitionsCollection<TArgs> transitions;

		readonly StatesCollection<TArgs> states;
	}
}
