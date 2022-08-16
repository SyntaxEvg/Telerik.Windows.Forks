using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	interface IRadFixedDocumentExportContext : IPdfExportContext
	{
		RadFixedDocument Document { get; }

		void PrepareWidgetAppearancesForExport();
	}
}
