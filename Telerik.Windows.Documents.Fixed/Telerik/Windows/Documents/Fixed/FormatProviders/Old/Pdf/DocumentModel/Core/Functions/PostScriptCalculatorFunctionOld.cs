using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions
{
	[PdfClass(TypeName = "Function", SubtypeProperty = "FunctionType", SubtypeValue = "4")]
	class PostScriptCalculatorFunctionOld : FunctionOld
	{
		public PostScriptCalculatorFunctionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		protected override FunctionBase CreateFunctionModel()
		{
			return this.CreateFunctionModel(null);
		}

		protected override FunctionBase CreateFunctionModel(byte[] data)
		{
			double[] domain = base.ReadDoubleArray(base.Domain);
			double[] range = base.ReadDoubleArray(base.Range);
			return new PostScriptCalculatorFunction(data, domain, range);
		}
	}
}
