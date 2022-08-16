using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions
{
	class ExponentialInterpolationFunctionObject : FunctionObject
	{
		public ExponentialInterpolationFunctionObject()
		{
			this.c0 = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("C0"), new PdfArray(new PdfPrimitive[]
			{
				new PdfReal(0.0)
			}));
			this.c1 = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("C1"), new PdfArray(new PdfPrimitive[]
			{
				new PdfReal(1.0)
			}));
			this.number = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("N", true));
		}

		public PdfArray C0
		{
			get
			{
				return this.c0.GetValue();
			}
			set
			{
				this.c0.SetValue(value);
			}
		}

		public PdfArray C1
		{
			get
			{
				return this.c1.GetValue();
			}
			set
			{
				this.c1.SetValue(value);
			}
		}

		public PdfReal Number
		{
			get
			{
				return this.number.GetValue();
			}
			set
			{
				this.number.SetValue(value);
			}
		}

		protected override bool HasStreamData
		{
			get
			{
				return false;
			}
		}

		public override FunctionBase ToFunction(PostScriptReader reader, IPdfImportContext context)
		{
			double value = this.Number.Value;
			double[] array = this.C0.ToArray(reader, context);
			double[] array2 = this.C1.ToArray(reader, context);
			double[] domain = base.Domain.ToArray(reader, context);
			double[] range = base.Range.ToArray(reader, context);
			return new ExponentialInterpolationFunction(value, array, array2, domain, range);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function)
		{
			ExponentialInterpolationFunction exponentialInterpolationFunction = (ExponentialInterpolationFunction)function;
			this.Number = new PdfReal(exponentialInterpolationFunction.N);
			this.C0 = exponentialInterpolationFunction.C0.ToPdfArray();
			this.C1 = exponentialInterpolationFunction.C1.ToPdfArray();
		}

		readonly DirectProperty<PdfArray> c0;

		readonly DirectProperty<PdfArray> c1;

		readonly DirectProperty<PdfReal> number;
	}
}
