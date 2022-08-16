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

		public override FunctionBase ToFunction(PostScriptReader reader, IPdfImportContext context)
		{
			double[] domain = base.Domain.ToArray(reader, context);
			double[] range = base.Range.ToArray(reader, context);
			return new PostScriptCalculatorFunction(base.FunctionData, domain, range);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function)
		{
		}
	}
}
