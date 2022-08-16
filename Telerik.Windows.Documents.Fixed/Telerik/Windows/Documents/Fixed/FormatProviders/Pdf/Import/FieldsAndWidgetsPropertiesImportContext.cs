using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class FieldsAndWidgetsPropertiesImportContext : RadFixedDocumentImportContext
	{
		public FieldsAndWidgetsPropertiesImportContext(PdfImportSettings settings)
			: base(settings)
		{
			this.fields = new List<FormField>();
			this.widgets = new List<Widget>();
		}

		public FormField GetField(int index)
		{
			return this.fields[index];
		}

		public Widget GetWidget(int index)
		{
			return this.widgets[index];
		}

		protected override void BeginImportOverride()
		{
			DocumentCatalog value = base.Root.GetValue();
			if (value.AcroForm != null)
			{
				base.Document.AcroForm.ViewersShouldRecalculateWidgetAppearances = value.AcroForm.NeedAppearance.Value;
				foreach (FormFieldNode formFieldNode in value.AcroForm.Fields)
				{
					bool importFieldProperties = formFieldNode.FieldType.Value != "Sig";
					FormField formField = formFieldNode.ToFormField(base.Reader, this, importFieldProperties);
					base.Document.AcroForm.FormFields.Add(formField);
					this.fields.Add(formField);
					foreach (WidgetObject widgetObject in base.GetChildWidgets(formFieldNode))
					{
						double pageHeightInDip = 0.0;
						Page parentPage = widgetObject.ParentPage;
						if (parentPage != null)
						{
							pageHeightInDip = parentPage.MediaBox.ToDipRect(base.Reader, this).Height;
						}
						Widget item = (Widget)widgetObject.ToAnnotation(base.Reader, this, pageHeightInDip);
						this.widgets.Add(item);
					}
				}
			}
		}

		readonly List<FormField> fields;

		readonly List<Widget> widgets;
	}
}
