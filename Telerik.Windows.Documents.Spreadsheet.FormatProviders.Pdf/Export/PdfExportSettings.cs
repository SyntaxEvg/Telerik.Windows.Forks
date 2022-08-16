using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export
{
	public class PdfExportSettings : ExportWhatSettings
	{
		public PdfExportSettings(ExportWhat exportWhat, bool ignorePrintArea)
			: base(exportWhat, ignorePrintArea)
		{
			this.selectedRanges = new List<CellRange>();
			this.PdfFileSettings = new PdfExportSettings();
		}

		public PdfExportSettings(IEnumerable<CellRange> selection)
			: this(ExportWhat.Selection, true)
		{
			this.SelectedRanges.AddRange(selection);
		}

		internal PdfExportSettings()
		{
			this.selectedRanges = new List<CellRange>();
			this.PdfFileSettings = new PdfExportSettings();
		}

		public List<CellRange> SelectedRanges
		{
			get
			{
				return this.selectedRanges;
			}
		}

		public PdfExportSettings PdfFileSettings { get; set; }

		public IPdfChartRenderer ChartRenderer { get; set; }

		readonly List<CellRange> selectedRanges;
	}
}
