using System;
using System.Threading;
using System.Windows.Threading;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	class DelayedExecution : IDisposable
	{
		public TimeSpan WaitInterval
		{
			get
			{
				return this.timer.Interval;
			}
			set
			{
				this.timer.Interval = value;
			}
		}

		public DelayedExecution NextAction { get; set; }

		public DelayedExecution(Action action, long milliseconds)
			: this(action, milliseconds, false, null)
		{
		}

		public DelayedExecution(Action action, long milliseconds, bool useThreadingTimer, Dispatcher dispatcher)
		{
			this.dispatcher = dispatcher;
			this.action = action;
			this.useThreadingTimer = useThreadingTimer;
			this.timerInterval = TimeSpan.FromMilliseconds((double)milliseconds);
			if (!useThreadingTimer)
			{
				this.timer = new DispatcherTimer();
				this.timer.Interval = this.timerInterval;
				this.timer.Tick += this.Timer_Tick;
			}
		}

		public void Execute()
		{
			this.EnsureNotDisposed();
			this.Execute(true);
		}

		public void Execute(bool executeAsynch)
		{
			this.EnsureNotDisposed();
			if (executeAsynch)
			{
				if (this.useThreadingTimer)
				{
					this.threadingTimer = new Timer(new TimerCallback(this.OnThreadingTimer), null, this.timerInterval, this.timerInterval);
					return;
				}
				if (!this.timer.IsEnabled)
				{
					this.timer.Start();
					return;
				}
			}
			else
			{
				this.ExecuteAction(false);
			}
		}

		void Timer_Tick(object sender, EventArgs e)
		{
			this.timer.Stop();
			if (this.GetHaltSwitch().halted)
			{
				return;
			}
			this.ExecuteAction(true);
		}

		void OnThreadingTimer(object state)
		{
			this.threadingTimer.Dispose();
			if (this.GetHaltSwitch().halted)
			{
				return;
			}
			try
			{
				this.ExecuteAction(true);
			}
			catch
			{
			}
		}

		void ExecuteAction(bool asynch)
		{
			this.action();
			if (this.NextAction != null)
			{
				this.NextAction.halt = this.GetHaltSwitch();
				Action action = delegate()
				{
					this.NextAction.Execute(asynch);
				};
				if (asynch && this.dispatcher != null)
				{
					this.dispatcher.BeginInvoke(action, new object[0]);
					return;
				}
				action();
			}
		}

		DelayedExecution.HaltSwitch GetHaltSwitch()
		{
			if (this.halt == null)
			{
				this.halt = new DelayedExecution.HaltSwitch();
			}
			return this.halt;
		}

		internal void Halt()
		{
			this.EnsureNotDisposed();
			this.GetHaltSwitch().halted = true;
		}

		public void Dispose()
		{
			if (this.alreadyDisposed)
			{
				return;
			}
			if (this.threadingTimer != null)
			{
				this.threadingTimer.Dispose();
			}
			if (this.timer != null)
			{
				this.timer.Stop();
				this.timer.Tick -= this.Timer_Tick;
			}
			GC.SuppressFinalize(this);
			this.alreadyDisposed = true;
		}

		void EnsureNotDisposed()
		{
			if (this.alreadyDisposed)
			{
				throw new ObjectDisposedException("DelayedExecution");
			}
		}

		readonly Action action;

		readonly DispatcherTimer timer;

		Timer threadingTimer;

		readonly TimeSpan timerInterval;

		readonly bool useThreadingTimer;

		readonly Dispatcher dispatcher;

		DelayedExecution.HaltSwitch halt;

		bool alreadyDisposed;

		class HaltSwitch
		{
			public bool halted;
		}
	}
}
