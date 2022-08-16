using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming
{
	interface IWorkbookImporter : IDisposable
	{
		IEnumerable<IWorksheetImporter> WorksheetImporters { get; }
	}
}
