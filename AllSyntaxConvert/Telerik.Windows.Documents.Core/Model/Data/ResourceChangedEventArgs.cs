using System;
using Telerik.Windows.Documents.Common.Model.Data;

namespace Telerik.Windows.Documents.Model.Data
{
	class ResourceChangedEventArgs : EventArgs
	{
		public IResource OldValue { get; set; }

		public IResource NewValue { get; set; }
	}
}
