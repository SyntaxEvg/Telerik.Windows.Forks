using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions
{
	[PdfClass(TypeName = "Function", SubtypeProperty = "FunctionType", SubtypeValue = "2")]
	class ExponentialInterpolationFunctionOld : FunctionOld
	{
		public ExponentialInterpolationFunctionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.c0 = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("C0"));
			this.c1 = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("C1"));
			this.number = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor("N", true), Converters.PdfRealConverter);
		}

		public PdfArrayOld C0
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

		public PdfArrayOld C1
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

		public PdfRealOld Number
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

		protected override FunctionBase CreateFunctionModel()
		{
			double value = this.Number.Value;
			double[] array = base.ReadDoubleArray(this.C0);
			double[] array2 = base.ReadDoubleArray(this.C1);
			double[] domain = base.ReadDoubleArray(base.Domain);
			double[] range = base.ReadDoubleArray(base.Range);
			return new ExponentialInterpolationFunction(value, array, array2, domain, range);
		}

		protected override FunctionBase CreateFunctionModel(byte[] data)
		{
			return this.CreateFunctionModel();
		}

		readonly InstantLoadProperty<PdfArrayOld> c0;

		readonly InstantLoadProperty<PdfArrayOld> c1;

		readonly InstantLoadProperty<PdfRealOld> number;
	}
}
