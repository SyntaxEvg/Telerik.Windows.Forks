using System;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.Importers;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	static class SpreadImporter
	{
		public static IWorkbookImporter CreateWorkbookImporter(SpreadDocumentFormat documentFormat, Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			switch (documentFormat)
			{
			case SpreadDocumentFormat.Xlsx:
				return new XlsxWorkbookImporter(stream);
			}
			throw new NotSupportedException();
		}
	}
}
