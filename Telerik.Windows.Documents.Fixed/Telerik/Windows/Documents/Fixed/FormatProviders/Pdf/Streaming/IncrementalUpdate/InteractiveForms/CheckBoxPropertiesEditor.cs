using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class CheckBoxPropertiesEditor : TwoStateAppearanceFieldPropertiesEditor
	{
		public CheckBoxPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetIsChecked(bool isChecked)
		{
			PdfSourceImportContext context = base.Field.FileSource.Context;
			PdfName pdfName;
			bool flag = base.Field.PdfDictionary.TryGetElement<PdfName>(context.Reader, context, "V", out pdfName) && pdfName.Value != "Off";
			if (!(isChecked ? flag : (!flag)))
			{
				string initialValue = (isChecked ? this.GetOnStateName() : "Off");
				PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
				clonedFieldDictionary["V"] = new PdfName(initialValue);
				for (int i = 0; i < base.Field.WidgetObjects.Length; i++)
				{
					PdfDictionary clonedWidgetDictionary = base.GetClonedWidgetDictionary(i);
					clonedWidgetDictionary["AS"] = new PdfName(initialValue);
				}
			}
		}

		string GetOnStateName()
		{
			PdfSourceImportContext context = base.Field.FileSource.Context;
			PdfArray pdfArray;
			if (base.Field.PdfDictionary.TryGetElement<PdfArray>(context.Reader, context, "Opt", out pdfArray) && pdfArray.Count > 0)
			{
				PdfPrimitive pdfPrimitive = pdfArray[0];
				return pdfPrimitive.ToString();
			}
			PdfName pdfName;
			if (base.Field.PdfDictionary.TryGetElement<PdfName>(context.Reader, context, "V", out pdfName) && pdfName.Value != "Off")
			{
				return pdfName.Value;
			}
			for (int i = 0; i < base.Field.WidgetObjects.Length; i++)
			{
				string result;
				if (base.TryGetWidgetOnState(i, out result))
				{
					return result;
				}
			}
			return "Yes";
		}
	}
}
