using System;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.Exporters.Csv;
using Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public static class SpreadExporter
	{
		public static IWorkbookExporter CreateWorkbookExporter(SpreadDocumentFormat documentFormat, Stream stream)
		{
			return SpreadExporter.CreateWorkbookExporter(documentFormat, stream, SpreadExportMode.Create);
		}

		public static IWorkbookExporter CreateWorkbookExporter(SpreadDocumentFormat documentFormat, Stream stream, SpreadExportMode exportMode)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			stream.Seek(0L, SeekOrigin.Begin);
			switch (documentFormat)
			{
			case SpreadDocumentFormat.Xlsx:
				return SpreadExporter.CreateXlsxWorkbookExporter(stream, exportMode);
			case SpreadDocumentFormat.Csv:
				return SpreadExporter.CreateCsvWorkbookExporter(stream, exportMode);
			default:
				throw new NotSupportedException();
			}
		}

		static IWorkbookExporter CreateXlsxWorkbookExporter(Stream stream, SpreadExportMode exportMode)
		{
			switch (exportMode)
			{
			case SpreadExportMode.Create:
				return new XlsxWorkbookExporter(stream);
			case SpreadExportMode.Append:
				if (stream.Length > 0L)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						stream.CopyTo(memoryStream);
						stream.Seek(0L, SeekOrigin.Begin);
						return new XlsxAppendWorkbookExporter(memoryStream, stream);
					}
				}
				return new XlsxWorkbookExporter(stream);
			default:
				throw new NotSupportedException();
			}
		}

		static IWorkbookExporter CreateCsvWorkbookExporter(Stream stream, SpreadExportMode exportMode)
		{
			switch (exportMode)
			{
			case SpreadExportMode.Create:
				return new CsvWorkbookExporter(stream, false);
			case SpreadExportMode.Append:
				if (stream.Length > 0L)
				{
					return new CsvWorkbookExporter(stream, true);
				}
				return new CsvWorkbookExporter(stream, false);
			default:
				throw new NotSupportedException();
			}
		}
	}
}
