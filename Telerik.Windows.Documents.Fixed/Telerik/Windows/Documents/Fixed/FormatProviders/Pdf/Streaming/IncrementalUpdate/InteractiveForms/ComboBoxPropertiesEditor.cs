using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class ComboBoxPropertiesEditor : ChoiceFieldPropertiesEditor
	{
		public ComboBoxPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetValue(string customValue)
		{
			PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
			clonedFieldDictionary.Remove("I");
			if (string.IsNullOrEmpty(customValue))
			{
				clonedFieldDictionary.Remove("V");
				return;
			}
			clonedFieldDictionary["V"] = customValue.ToPdfString();
		}

		public void SetValue(int selectedIndex)
		{
			if (base.FieldHasOptions)
			{
				string optionValue = base.GetOptionValue(selectedIndex);
				PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
				clonedFieldDictionary["V"] = optionValue.ToPdfString();
				clonedFieldDictionary["I"] = new PdfArray(new PdfPrimitive[]
				{
					new PdfInt(selectedIndex)
				});
			}
		}
	}
}
