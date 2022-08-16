using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class FieldsAndWidgetsPropertiesExportContext : RadFixedDocumentExportContext
	{
		public FieldsAndWidgetsPropertiesExportContext(int startObjectId, RadFixedDocument document, PdfExportSettings settings)
			: base(document, settings)
		{
			this.currentObjectNumber = startObjectId;
		}

		protected override int GetNextObjectNumber()
		{
			int result = this.currentObjectNumber;
			this.currentObjectNumber++;
			return result;
		}

		public override void PrepareWidgetAppearancesForExport()
		{
			base.PrepareWidgetAppearancesForExport();
			RadFixedPage fixedPage = new RadFixedPage();
			foreach (FormField formField in base.Document.AcroForm.FormFields)
			{
				foreach (Widget annotation in formField.Widgets)
				{
					AnnotationFactory.CreatePdfAnnotation(this, annotation, fixedPage);
				}
			}
		}

		int currentObjectNumber;
	}
}
