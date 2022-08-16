using System;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities.Performance
{
	class SnippetMemoryCounter
	{
		public double TotalMemory
		{
			get
			{
				return (double)this.totalMemory;
			}
		}

		public double TotalMemoryMB
		{
			get
			{
				return (double)((float)this.totalMemory / 1024f / 1024f);
			}
		}

		public SnippetMemoryCounter(string snippetName)
		{
			this.logger = new DebugLogger();
			this.snippetName = snippetName;
			this.totalMemory = 0L;
			this.isStartCalled = false;
			this.hasBeenStarted = false;
		}

		public void Start()
		{
			if (!this.hasBeenStarted)
			{
				this.logger.Log(string.Format("[{0}]: FIRST TIME STARTED", this.snippetName));
				this.hasBeenStarted = true;
			}
			if (!this.isStartCalled)
			{
				GC.WaitForPendingFinalizers();
				GC.Collect();
				this.currentStartMemory = GC.GetTotalMemory(true);
				this.isStartCalled = true;
			}
			this.hitCounter++;
		}

		public void Stop()
		{
			if (this.isStartCalled)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
				this.totalMemory += GC.GetTotalMemory(true) - this.currentStartMemory;
				this.isStartCalled = false;
			}
		}

		public void End()
		{
			this.Stop();
			if (this.hasBeenStarted)
			{
				this.logger.Log(string.Format("[{0}]: ENDED WITH TOTAL MEMORY:{1}; TIMES HIT: {2}; TOTAL MEMORY: {3}MB", new object[]
				{
					this.snippetName,
					this.totalMemory,
					this.hitCounter,
					GC.GetTotalMemory(true)
				}));
			}
		}

		ILogger logger;

		string snippetName;

		long totalMemory;

		bool isStartCalled;

		long currentStartMemory;

		bool hasBeenStarted;

		int hitCounter;
	}
}
