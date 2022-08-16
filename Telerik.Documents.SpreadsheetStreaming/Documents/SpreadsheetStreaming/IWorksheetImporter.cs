using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming
{
	interface IWorksheetImporter : IDisposable
	{
		string Name { get; }

		IEnumerable<IColumnImporter> Columns { get; }

		IEnumerable<IRowImporter> Rows { get; }
	}
}
