using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Documents.SpreadsheetStreaming.Core;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Csv
{
	class CsvWorkbookExporter : WorkbookExporterBase
	{
		public CsvWorkbookExporter(Stream stream, bool isAppending)
		{
			this.stream = stream;
			if (isAppending)
			{
				this.stream.Seek(0L, SeekOrigin.End);
			}
			else
			{
				this.stream.Seek(0L, SeekOrigin.Begin);
				this.stream.SetLength(0L);
			}
			this.writer = new CsvStreamWriter(this.stream, Encoding.UTF8, false);
			this.cellStyles = new SpreadCellStyleCollection();
			this.shouldCheckLicense = true;
		}

		public override SpreadCellStyleCollection CellStyles
		{
			get
			{
				return this.cellStyles;
			}
		}

		public override IWorksheetExporter CreateWorksheetExporterOverride(string name)
		{
			CsvWorksheetExporter result = new CsvWorksheetExporter(this, this.writer, this.shouldCheckLicense);
			this.shouldCheckLicense = false;
			return result;
		}

		public override IEnumerable<SheetInfo> GetSheetInfos()
		{
			return Enumerable.Empty<SheetInfo>();
		}

		internal override void CompleteWriteOverride()
		{
			this.writer.Flush();
			this.writer.Dispose();
			base.CompleteWriteOverride();
		}

		readonly Stream stream;

		readonly SpreadCellStyleCollection cellStyles;

		readonly CsvStreamWriter writer;

		bool shouldCheckLicense;
	}
}
