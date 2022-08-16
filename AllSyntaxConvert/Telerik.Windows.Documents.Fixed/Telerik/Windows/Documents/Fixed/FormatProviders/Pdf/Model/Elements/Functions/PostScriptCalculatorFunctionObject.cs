using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions
{
	class PostScriptCalculatorFunctionObject : FunctionObject
	{
		protected override bool HasStreamData
		{
			get
			{
				return true;
			}
		}

		public override FunctionBase ToFunction(PostScriptReader reader,IPdfImportContext context)
		{
			return (FunctionBase)new PostScriptCalculatorFunction(this.FunctionData, this.Domain.ToArray<PdfReal, double>(reader, context), this.Range.ToArray<PdfReal, double>(reader, context));
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function)
		{
		}
	}
}
