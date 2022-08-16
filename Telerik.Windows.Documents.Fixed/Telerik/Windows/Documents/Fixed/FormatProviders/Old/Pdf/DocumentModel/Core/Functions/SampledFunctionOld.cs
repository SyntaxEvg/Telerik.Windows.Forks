using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions
{
	[PdfClass(TypeName = "Function", SubtypeProperty = "FunctionType", SubtypeValue = "0")]
	class SampledFunctionOld : FunctionOld
	{
		public SampledFunctionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.size = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Size", true));
			this.bitsPerSample = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("BitsPerSample", true), Converters.PdfIntConverter);
			this.encode = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Encode"));
			this.decode = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Decode"));
		}

		public PdfArrayOld Size
		{
			get
			{
				return this.size.GetValue();
			}
			set
			{
				this.size.SetValue(value);
			}
		}

		public PdfIntOld BitsPerSample
		{
			get
			{
				return this.bitsPerSample.GetValue();
			}
			set
			{
				this.bitsPerSample.SetValue(value);
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

		public PdfArrayOld Decode
		{
			get
			{
				return this.decode.GetValue();
			}
			set
			{
				this.decode.SetValue(value);
			}
		}

		protected override FunctionBase CreateFunctionModel()
		{
			return this.CreateFunctionModel(null);
		}

		protected override FunctionBase CreateFunctionModel(byte[] data)
		{
			int[] array = new int[this.Size.Count];
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (this.Size.TryGetInt(i, out num))
				{
					array[i] = num;
				}
			}
			double[] domain = base.ReadDoubleArray(base.Domain);
			double[] range = base.ReadDoubleArray(base.Range);
			double[] array2 = base.ReadDoubleArray(this.Encode);
			double[] array3 = base.ReadDoubleArray(this.Decode);
			return new SampledFunction(data, array, this.BitsPerSample.Value, domain, range, array2, array3);
		}

		readonly InstantLoadProperty<PdfArrayOld> size;

		readonly InstantLoadProperty<PdfIntOld> bitsPerSample;

		readonly InstantLoadProperty<PdfArrayOld> encode;

		readonly InstantLoadProperty<PdfArrayOld> decode;
	}
}
