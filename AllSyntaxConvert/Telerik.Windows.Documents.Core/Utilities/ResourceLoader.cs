using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Utilities
{
	class ResourceLoader : IDisposable
	{
		public ResourceLoader()
		{
			this.isLoading = false;
		}

		internal event EventHandler<DoWorkEventArgs> DoWork;

		internal event EventHandler<WorkCompletedEventArgs> WorkCompleted;

		void OnWorkCompleted(WorkCompletedEventArgs args)
		{
			if (this.WorkCompleted != null)
			{
				this.WorkCompleted(this, args);
			}
		}

		void OnDoWork(DoWorkEventArgs args)
		{
			if (this.DoWork != null)
			{
				this.DoWork(this, args);
			}
		}

		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
		}

		void AttachToBackgroundWorkerEvents(BackgroundWorker worker)
		{
			if (worker == null)
			{
				return;
			}
			//worker.DoWork += this.Worker_DoWork;
			worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
		}

		void DetachToBackgroundWorkerEvents(BackgroundWorker worker)
		{
			if (worker == null)
			{
				return;
			}
			//worker.DoWork -= this.Worker_DoWork;
			worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
		}

		internal void Load(object argument)
		{
			if (this.isLoading)
			{
				throw new InvalidOperationException("The Load method has been already called.");
			}
			this.isLoading = true;
			this.currentThreadToken = new ResourceLoader.CancellationToken(argument);
			this.DetachToBackgroundWorkerEvents(this.backgroundWorker);
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.WorkerReportsProgress = false;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.AttachToBackgroundWorkerEvents(this.backgroundWorker);
			this.backgroundWorker.RunWorkerAsync(this.currentThreadToken);
		}

		internal void StopLoading()
		{
			if (this.currentThreadToken == null)
			{
				return;
			}
			if (this.currentThreadToken.DoWorkArgs != null)
			{
				this.currentThreadToken.DoWorkArgs.Canceled = true;
			}
			if (this.backgroundWorker != null)
			{
				this.backgroundWorker.CancelAsync();
			}
			this.currentThreadToken = null;
			this.isLoading = false;
		}

		public void Dispose()
		{
			if (this.backgroundWorker != null)
			{
				this.backgroundWorker.Dispose();
			}
		}

		void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ResourceLoader.CancellationToken cancellationToken = (ResourceLoader.CancellationToken)e.Result;
			if (cancellationToken != null && !cancellationToken.DoWorkArgs.Canceled && !e.Cancelled && e.Error == null)
			{
				this.OnWorkCompleted(new WorkCompletedEventArgs
				{
					Result = cancellationToken.DoWorkArgs.Result
				});
			}
			this.isLoading = false;
		}

		//void Worker_DoWork(object sender, DoWorkEventArgs e)
		//{
		//	BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		//	ResourceLoader.CancellationToken cancellationToken = (ResourceLoader.CancellationToken)e.Argument;
		//	if (cancellationToken == null)
		//	{
		//		return;
		//	}
		//	DoWorkEventArgs doWorkEventArgs = new DoWorkEventArgs();
		//	doWorkEventArgs.Argument = cancellationToken.Argument;
		//	cancellationToken.DoWorkArgs = doWorkEventArgs;
		//	if (e.Cancel || backgroundWorker.CancellationPending)
		//	{
		//		return;
		//	}
		//	try
		//	{
		//		this.OnDoWork(doWorkEventArgs);
		//	}
		//	catch
		//	{
		//	}
		//	if (!doWorkEventArgs.Canceled)
		//	{
		//		e.Result = cancellationToken;
		//	}
		//}

		bool isLoading;

		ResourceLoader.CancellationToken currentThreadToken;

		BackgroundWorker backgroundWorker;

		class CancellationToken
		{
			public CancellationToken(object argument)
			{
				this.Argument = argument;
			}

			public object Argument { get; set; }

			public DoWorkEventArgs DoWorkArgs { get; set; }
		}
	}
}
