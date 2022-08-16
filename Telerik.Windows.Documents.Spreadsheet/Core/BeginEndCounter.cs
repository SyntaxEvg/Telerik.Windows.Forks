using System;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	class BeginEndCounter
	{
		public bool IsUpdateInProgress
		{
			get
			{
				return this.beginUpdateCount > 0 || this.isUpdateInProgress;
			}
		}

		public int BeginUpdateCounter
		{
			get
			{
				return this.beginUpdateCount;
			}
		}

		public BeginEndCounter()
		{
		}

		public BeginEndCounter(Action action)
		{
			this.action = action;
		}

		public void BeginUpdate()
		{
			this.beginUpdateCount++;
			this.isUpdateInProgress = true;
		}

		public void EndUpdate()
		{
			if (this.beginUpdateCount == 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			this.beginUpdateCount--;
			if (this.beginUpdateCount == 0)
			{
				if (this.action != null)
				{
					this.action();
				}
				this.isUpdateInProgress = false;
			}
		}

		int beginUpdateCount;

		readonly Action action;

		bool isUpdateInProgress;
	}
}
