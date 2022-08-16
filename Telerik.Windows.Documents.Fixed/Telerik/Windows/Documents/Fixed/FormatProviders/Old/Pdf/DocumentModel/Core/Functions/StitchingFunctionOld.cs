using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions
{
	[PdfClass(TypeName = "Function", SubtypeProperty = "FunctionType", SubtypeValue = "3")]
	class StitchingFunctionOld : FunctionOld
	{
		public StitchingFunctionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.functions = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Functions", true));
			this.bounds = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Bounds", true));
			this.encode = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Encode", true));
		}

		public PdfArrayOld Functions
		{
			get
			{
				return this.functions.GetValue();
			}
			set
			{
				this.functions.SetValue(value);
			}
		}

		public PdfArrayOld Bounds
		{
			get
			{
				return this.bounds.GetValue();
			}
			set
			{
				this.bounds.SetValue(value);
			}
		}

		public PdfArrayOld Encode
		{
			get
			{
				return this.encode.GetValue();
			}
			set
			{
				this.encode.SetValue(value);
			}
		}

		protected override FunctionBase CreateFunctionModel()
		{
			FunctionBase[] array = new FunctionBase[this.Functions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				FunctionOld element = this.Functions.GetElement<FunctionOld>(i, Converters.FunctionConverter);
				array[i] = element.FunctionModel;
			}
			double[] array2 = base.ReadDoubleArray(this.Bounds);
			double[] array3 = base.ReadDoubleArray(this.Encode);
			double[] domain = base.ReadDoubleArray(base.Domain);
			double[] range = base.ReadDoubleArray(base.Range);
			return new StitchingFunction(array, array2, array3, domain, range);
		}

		protected override FunctionBase CreateFunctionModel(byte[] data)
		{
			return this.CreateFunctionModel();
		}

		readonly InstantLoadProperty<PdfArrayOld> functions;

		readonly InstantLoadProperty<PdfArrayOld> bounds;

		readonly InstantLoadProperty<PdfArrayOld> encode;
	}
}
