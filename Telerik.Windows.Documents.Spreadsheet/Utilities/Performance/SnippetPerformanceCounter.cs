using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities.Performance
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class SnippetPerformanceCounter
	{
		public int HitCounter
		{
			get
			{
				return this.hitCounter;
			}
		}

		public double TotalMilliseconds
		{
			get
			{
				return this.totalMilliseconds;
			}
		}

		public SnippetPerformanceCounter(string snippetName, bool useLogging = true)
		{
			if (useLogging)
			{
				this.logger = new DebugLogger();
			}
			this.snippetName = snippetName;
			this.Reset();
		}

		public void Start()
		{
			if (!this.hasBeenStarted)
			{
				if (this.logger != null)
				{
					this.logger.Log(string.Format("[{0}]: FIRST TIME STARTED", this.snippetName));
				}
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
			if (this.hasBeenStarted && this.logger != null)
			{
				this.logger.Log(string.Format("[{0}]: ENDED WITH TOTAL:{1}; TIMES HIT: {2}", this.snippetName, this.totalMilliseconds, this.hitCounter));
			}
		}

		public void Reset()
		{
			this.totalMilliseconds = 0.0;
			this.hitCounter = 0;
			this.isStartCalled = false;
			this.hasBeenStarted = false;
		}

		ILogger logger;

		string snippetName;

		double totalMilliseconds;

		bool isStartCalled;

		DateTime currentStartTime;

		bool hasBeenStarted;

		int hitCounter;
	}
}
