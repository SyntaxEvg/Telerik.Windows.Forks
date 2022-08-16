using System;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Licensing;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Csv
{
	class CsvWorksheetExporter : EntityBase, IWorksheetExporter, IDisposable
	{
		internal CsvWorksheetExporter(WorkbookExporterBase workbook, StreamWriter writer, bool shouldCheckLicense)
		{
			this.workbook = workbook;
			this.writer = writer;
			if (shouldCheckLicense)
			{
				LicenseCheck.Validate(this);
			}
		}

		public IWorksheetViewExporter CreateWorksheetViewExporter()
		{
			return new CsvWorksheetViewExporter();
		}

		public IRowExporter CreateRowExporter()
		{
			return new CsvRowExporter(this.writer);
		}

		public IColumnExporter CreateColumnExporter()
		{
			return new CsvColumnExporter();
		}

		public void SkipRows(int count)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, count, "count");
			this.writer.WriteLine();
		}

		public void SkipColumns(int count)
		{
		}

		public void MergeCells(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
		}

		internal sealed override void DisposeOverride()
		{
			this.workbook.NotifyWorksheetDisposing();
			this.workbook = null;
		}

		readonly StreamWriter writer;

		WorkbookExporterBase workbook;
	}
}
