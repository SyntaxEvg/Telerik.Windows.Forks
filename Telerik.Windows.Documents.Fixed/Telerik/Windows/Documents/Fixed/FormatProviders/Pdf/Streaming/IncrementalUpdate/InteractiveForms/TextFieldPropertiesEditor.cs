using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class TextFieldPropertiesEditor : SingleStateAppearanceFieldPropertiesEditor
	{
		public TextFieldPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetValue(string text)
		{
			PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
			if (string.IsNullOrEmpty(text))
			{
				clonedFieldDictionary.Remove("V");
				return;
			}
			clonedFieldDictionary["V"] = text.ToPdfString();
		}
	}
}
