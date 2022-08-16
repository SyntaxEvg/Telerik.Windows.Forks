using System;

namespace Telerik.Windows.Documents.Utilities
{
	class BeginEndCounter
	{
		public BeginEndCounter()
			: this(null, null)
		{
		}

		public BeginEndCounter(Action endAction)
			: this(null, endAction)
		{
		}

		public BeginEndCounter(Action beginAction, Action endAction)
		{
			this.beginAction = beginAction;
			this.endAction = endAction;
		}

		public bool IsUpdateInProgress
		{
			get
			{
				return this.beginCount > 0;
			}
		}

		public int BeginUpdateCounter
		{
			get
			{
				return this.beginCount;
			}
		}

		public IDisposable Begin()
		{
			if (this.beginCount == 0 && this.beginAction != null)
			{
				this.beginAction();
			}
			this.beginCount++;
			return new DisposableObject(new Action(this.End));
		}

		public void End()
		{
			if (this.beginCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			if (this.beginCount == 1 && this.endAction != null)
			{
				this.endAction();
			}
			this.beginCount--;
		}

		readonly Action beginAction;

		readonly Action endAction;

		int beginCount;
	}
}
