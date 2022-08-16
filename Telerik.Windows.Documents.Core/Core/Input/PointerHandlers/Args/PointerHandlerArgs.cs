using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Input.PointerHandlers.Args
{
	class PointerHandlerArgs
	{
		public bool IsHandled { get; set; }

		public SourceType Source { get; set; }

		public Point Position { get; set; }
	}
}
