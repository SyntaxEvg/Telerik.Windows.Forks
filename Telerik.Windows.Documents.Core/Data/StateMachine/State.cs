using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Data.StateMachine
{
	class State<TArgs>
	{
		public State(string name, Action<TArgs> onEnterAction = null, Action<TArgs> onLeaveAction = null, Action<TArgs> onStayAction = null, Action<TArgs> onStartAction = null, Action<TArgs> onStopAction = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
			this.onStartAction = onStartAction;
			this.onStopAction = onStopAction;
			this.onStayAction = onStayAction;
			this.onEnterAction = onEnterAction;
			this.onLeaveAction = onLeaveAction;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public void Start(TArgs arguments)
		{
			State<TArgs>.ExecuteActionIfNotNull(this.onStartAction, arguments);
		}

		public void Stop(TArgs arguments)
		{
			State<TArgs>.ExecuteActionIfNotNull(this.onStopAction, arguments);
		}

		public void Enter(TArgs arguments)
		{
			State<TArgs>.ExecuteActionIfNotNull(this.onEnterAction, arguments);
		}

		public void Leave(TArgs arguments)
		{
			State<TArgs>.ExecuteActionIfNotNull(this.onLeaveAction, arguments);
		}

		public void Stay(TArgs arguments)
		{
			State<TArgs>.ExecuteActionIfNotNull(this.onStayAction, arguments);
		}

		static void ExecuteActionIfNotNull(Action<TArgs> action, TArgs arguments)
		{
			if (action != null)
			{
				action(arguments);
			}
		}

		public override string ToString()
		{
			return this.name;
		}

		readonly string name;

		readonly Action<TArgs> onStartAction;

		readonly Action<TArgs> onStopAction;

		readonly Action<TArgs> onEnterAction;

		readonly Action<TArgs> onLeaveAction;

		readonly Action<TArgs> onStayAction;
	}
}
