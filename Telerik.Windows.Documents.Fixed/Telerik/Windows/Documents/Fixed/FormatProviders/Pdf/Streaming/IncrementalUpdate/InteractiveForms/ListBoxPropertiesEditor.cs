using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class ListBoxPropertiesEditor : ChoiceFieldPropertiesEditor
	{
		public ListBoxPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetValue(int[] sortedSelectedIndices)
		{
			if (sortedSelectedIndices == null || sortedSelectedIndices.Length == 0)
			{
				PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
				clonedFieldDictionary.Remove("V");
				clonedFieldDictionary.Remove("I");
				return;
			}
			if (base.FieldHasOptions)
			{
				PdfDictionary clonedFieldDictionary2 = base.GetClonedFieldDictionary();
				clonedFieldDictionary2["V"] = new PdfArray(from i in sortedSelectedIndices
					select base.GetOptionValue(i).ToPdfString());
				clonedFieldDictionary2["I"] = sortedSelectedIndices.ToPdfArray();
			}
		}
	}
}
