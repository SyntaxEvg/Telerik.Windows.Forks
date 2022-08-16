using System;

namespace Telerik.Windows.Documents.Utilities
{
	sealed class DisposeValidator : IDisposable
	{
		public DisposeValidator()
		{
			this.isDisposed = false;
		}

		public bool IsDisposed
		{
			get
			{
				return this.isDisposed;
			}
		}

		public void Dispose()
		{
			Guard.ThrowExceptionIfTrue(this.isDisposed, "isDisposed");
			this.isDisposed = true;
		}

		bool isDisposed;
	}
}
