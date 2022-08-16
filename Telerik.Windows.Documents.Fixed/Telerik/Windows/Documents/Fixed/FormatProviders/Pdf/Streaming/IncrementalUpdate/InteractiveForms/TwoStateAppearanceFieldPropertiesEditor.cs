using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class TwoStateAppearanceFieldPropertiesEditor : FormFieldPropertiesEditor
	{
		public TwoStateAppearanceFieldPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		protected bool TryGetWidgetOnState(int widgetIndex, out string onStateName)
		{
			PdfDictionary content = base.Field.WidgetObjects[widgetIndex].GetContent<PdfDictionary>();
			PdfDictionary pdfDictionary;
			if (base.Field.FileSource.Context.TryGetNestedPrimitive<PdfDictionary>(content, this.normalAppearancePropertyNames, out pdfDictionary))
			{
				foreach (string text in pdfDictionary.Keys)
				{
					if (text != "Off")
					{
						onStateName = text;
						return true;
					}
				}
			}
			onStateName = null;
			return false;
		}

		readonly string[] normalAppearancePropertyNames = new string[] { "AP", "N" };
	}
}
