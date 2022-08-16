using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class ChoiceFieldPropertiesEditor : SingleStateAppearanceFieldPropertiesEditor
	{
		public ChoiceFieldPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
			PdfSourceImportContext context2 = base.Field.FileSource.Context;
			this.hasOptionsArray = base.Field.PdfDictionary.TryGetElement<PdfArray>(context2.Reader, context2, "Opt", out this.options);
		}

		protected bool FieldHasOptions
		{
			get
			{
				return this.hasOptionsArray;
			}
		}

		protected string GetOptionValue(int optionIndex)
		{
			Guard.ThrowExceptionIfFalse(this.FieldHasOptions, "FieldHasOptions");
			PdfPrimitive pdfPrimitive = this.options[optionIndex];
			string result;
			if (pdfPrimitive.Type == PdfElementType.Array)
			{
				PdfArray pdfArray = (PdfArray)pdfPrimitive;
				result = pdfArray[0].ToString();
			}
			else
			{
				result = pdfPrimitive.ToString();
			}
			return result;
		}

		bool hasOptionsArray;

		readonly PdfArray options;
	}
}
