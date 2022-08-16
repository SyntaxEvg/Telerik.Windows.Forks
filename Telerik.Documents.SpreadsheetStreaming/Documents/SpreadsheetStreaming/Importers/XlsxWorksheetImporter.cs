using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.Importers
{
	class XlsxWorksheetImporter : IWorksheetImporter, IDisposable
	{
		public XlsxWorksheetImporter(ZipArchive archive, string name, string target)
		{
			this.Name = name;
			this.part = new WorksheetPartReader(new PartContext(archive, this.Name, "xl/" + target));
			this.part.BeginRead();
		}

		~XlsxWorksheetImporter()
		{
			this.Dispose(false);
		}

		public string Name { get; set; }

		public IEnumerable<IColumnImporter> Columns
		{
			get
			{
				ColumnsElement columnsElement = this.part.RootElement.GetColumnsElementReader();
				if (columnsElement != null)
				{
					columnsElement.BeginReadElement();
					foreach (ColumnElement columnElement in columnsElement.Columns)
					{
						ColumnRange range = null;
						columnElement.Read(ref range);
						yield return new XlsxColumnImporter(range);
					}
				}
				yield break;
			}
		}

		public IEnumerable<IRowImporter> Rows
		{
			get
			{
				SheetDataElement sheetDataElement = this.part.RootElement.GetSheetDataElementReader();
				if (sheetDataElement != null)
				{
					sheetDataElement.BeginReadElement();
					foreach (RowElement rowElement in sheetDataElement.Rows)
					{
						yield return new XlsxRowImporter(rowElement);
					}
				}
				yield break;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.part.Dispose();
			}
		}

		readonly WorksheetPartReader part;
	}
}
