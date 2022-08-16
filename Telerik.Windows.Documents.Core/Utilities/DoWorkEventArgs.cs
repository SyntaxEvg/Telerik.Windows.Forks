using System;

namespace Telerik.Windows.Documents.Utilities
{
	class DoWorkEventArgs : EventArgs
	{
		public object Argument { get; set; }

		public object Result { get; set; }

		public bool Canceled { get; set; }
	}
}
