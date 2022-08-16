using System;

namespace Telerik.Windows.Documents.Utilities
{
	class SnippetPerformanceCounterRelease
	{
		public SnippetPerformanceCounterRelease(string snippetName)
		{
			this.snippetName = snippetName;
			this.totalMilliseconds = 0.0;
			this.isStartCalled = false;
			this.hasBeenStarted = false;
		}

		public void Start()
		{
			if (!this.hasBeenStarted)
			{
				this.hasBeenStarted = true;
			}
			if (!this.isStartCalled)
			{
				this.currentStartTime = DateTime.Now;
				this.isStartCalled = true;
			}
			this.hitCounter++;
		}

		public void Stop()
		{
			if (this.isStartCalled)
			{
				this.totalMilliseconds += (DateTime.Now - this.currentStartTime).TotalMilliseconds;
				this.isStartCalled = false;
			}
		}

		public void End()
		{
			this.Stop();
			bool flag = this.hasBeenStarted;
		}

		public double TotalMilliseconds
		{
			get
			{
				return this.totalMilliseconds;
			}
		}

		string snippetName;

		double totalMilliseconds;

		bool isStartCalled;

		DateTime currentStartTime;

		bool hasBeenStarted;

		int hitCounter;
	}
}
