using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Data.StateMachine
{
	class TransitionsCollection<TArgs>
	{
		public TransitionsCollection()
		{
			this.uniqueStore = new Dictionary<State<TArgs>, HashSet<Transition<TArgs>>>();
			this.store = new Dictionary<State<TArgs>, List<Transition<TArgs>>>();
		}

		public void AddTransition(Transition<TArgs> transition)
		{
			HashSet<Transition<TArgs>> hashSet;
			List<Transition<TArgs>> list;
			if (!this.uniqueStore.TryGetValue(transition.FromState, out hashSet))
			{
				hashSet = new HashSet<Transition<TArgs>>();
				this.uniqueStore[transition.FromState] = hashSet;
				list = new List<Transition<TArgs>>();
				this.store[transition.FromState] = list;
			}
			else
			{
				list = this.store[transition.FromState];
			}
			if (hashSet.Add(transition))
			{
				list.Add(transition);
			}
		}

		public IList<Transition<TArgs>> GetAllTransitionsForState(State<TArgs> state)
		{
			return this.store[state];
		}

		readonly Dictionary<State<TArgs>, HashSet<Transition<TArgs>>> uniqueStore;

		readonly Dictionary<State<TArgs>, List<Transition<TArgs>>> store;
	}
}
