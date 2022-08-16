using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Data.StateMachine
{
	class StatesCollection<TArgs>
	{
		public StatesCollection()
		{
			this.store = new Dictionary<string, State<TArgs>>();
		}

		public void Add(State<TArgs> state)
		{
			this.store.Add(state.Name, state);
		}

		public State<TArgs> GetStateByName(string name)
		{
			return this.store[name];
		}

		readonly Dictionary<string, State<TArgs>> store;
	}
}
