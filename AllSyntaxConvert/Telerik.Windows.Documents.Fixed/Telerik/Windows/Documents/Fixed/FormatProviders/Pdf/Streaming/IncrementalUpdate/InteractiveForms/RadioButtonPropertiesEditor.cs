using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class RadioButtonPropertiesEditor : TwoStateAppearanceFieldPropertiesEditor
	{
		public RadioButtonPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetValue(int widgetIndex)
		{
			PdfSourceImportContext context = base.Field.FileSource.Context;
			PdfDictionary content = base.Field.WidgetObjects[widgetIndex].GetContent<PdfDictionary>();
			PdfName pdfName;
			string text;
			if ((!content.TryGetElement<PdfName>(context.Reader, context, "AS", out pdfName) || !(pdfName.Value != "Off")) && base.TryGetWidgetOnState(widgetIndex, out text))
			{
				PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
				clonedFieldDictionary["V"] = new PdfName(text);
				for (int i = 0; i < base.Field.WidgetObjects.Length; i++)
				{
					string a;
					if (base.TryGetWidgetOnState(i, out a))
					{
						string initialValue = ((a == text) ? text : "Off");
						PdfDictionary clonedWidgetDictionary = base.GetClonedWidgetDictionary(i);
						clonedWidgetDictionary["AS"] = new PdfName(initialValue);
					}
				}
			}
		}

		public void ClearValue()
		{
			if (base.Field.PdfDictionary.ContainsKey("V"))
			{
				PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
				clonedFieldDictionary.Remove("V");
				for (int i = 0; i < base.Field.WidgetObjects.Length; i++)
				{
					PdfDictionary clonedWidgetDictionary = base.GetClonedWidgetDictionary(i);
					clonedWidgetDictionary["AS"] = new PdfName("Off");
				}
			}
		}
	}
}
