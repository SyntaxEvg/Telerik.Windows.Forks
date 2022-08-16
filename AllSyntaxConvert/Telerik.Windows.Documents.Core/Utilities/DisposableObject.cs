using System;

namespace Telerik.Windows.Documents.Utilities
{
	class DisposableObject : IDisposable
	{
		public DisposableObject(Action action)
		{
			Guard.ThrowExceptionIfNull<Action>(action, "action");
			this.action = action;
		}

		public void Dispose()
		{
			this.action();
		}

		readonly Action action;
	}
}
