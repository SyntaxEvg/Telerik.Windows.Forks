using System;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	class EntityBase : IChildEntry, IDisposable
	{
		~EntityBase()
		{
			this.Dispose(false);
		}

		bool IChildEntry.IsUsageBegan
		{
			get
			{
				return true;
			}
		}

		bool IChildEntry.IsUsageCompleted
		{
			get
			{
				return this.isUssageCompleted;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		internal virtual void CompleteWriteOverride()
		{
		}

		internal virtual void DisposeOverride()
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.CompleteWriteOverride();
				this.DisposeOverride();
				this.isUssageCompleted = true;
			}
		}

		bool isUssageCompleted;
	}
}
