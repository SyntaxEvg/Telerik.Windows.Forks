using System;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class RadFixedDocumentExportContext : PdfExportContext, IRadFixedDocumentExportContext, IPdfExportContext
	{
		public RadFixedDocumentExportContext(RadFixedDocument document, PdfExportSettings settings)
			: base(document.DocumentInfo, settings)
		{
			this.document = document;
		}

		public RadFixedDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public virtual void PrepareWidgetAppearancesForExport()
		{
			foreach (FormField formField in this.Document.AcroForm.FormFields)
			{
				formField.PrepareWidgetAppearancesForExport();
			}
		}

		readonly RadFixedDocument document;
	}
}
