using System;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public sealed class UpdateScope : IDisposable
	{
		public UpdateScope(Action beginUpdateAction, Action endUpdateAction)
		{
			this.endUpdateAction = endUpdateAction;
			beginUpdateAction();
		}

		public void Dispose()
		{
			this.endUpdateAction();
		}

		readonly Action endUpdateAction;
	}
}
