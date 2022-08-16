using System;

namespace Telerik.Windows.Documents.Data.StateMachine
{
	class Transition<TArgs>
	{
		public Transition(State<TArgs> stay, Func<TArgs, bool> predicate)
			: this(stay, stay, predicate)
		{
		}

		public Transition(State<TArgs> fromState, State<TArgs> toState, Func<TArgs, bool> predicate)
		{
			this.fromState = fromState;
			this.toState = toState;
			this.predicate = predicate;
		}

		public State<TArgs> ToState
		{
			get
			{
				return this.toState;
			}
		}

		public State<TArgs> FromState
		{
			get
			{
				return this.fromState;
			}
		}

		public Func<TArgs, bool> Predicate
		{
			get
			{
				return this.predicate;
			}
		}

		readonly Func<TArgs, bool> predicate;

		readonly State<TArgs> fromState;

		readonly State<TArgs> toState;
	}
}
