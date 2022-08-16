﻿using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions
{
	class SampledFunctionObject : FunctionObject
	{
		public SampledFunctionObject()
		{
			this.size = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Size", true));
			this.bitsPerSample = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("BitsPerSample", true));
			this.encode = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Encode"));
			this.decode = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Decode"));
		}

		public PdfArray Size
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

		public PdfInt BitsPerSample
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

		public PdfArray Encode
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

		public PdfArray Decode
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

		protected override bool HasStreamData
		{
			get
			{
				return true;
			}
		}

		public override FunctionBase ToFunction(PostScriptReader reader,IPdfImportContext context)
		{
			byte[] functionData = this.FunctionData;
			int bitsPerSample = this.BitsPerSample.Value;
			int[] array1 = this.Size.ToArray<PdfInt, int>(reader, context);
			double[] array2 = this.Domain.ToArray<PdfReal, double>(reader, context);
			double[] array3 = this.Range.ToArray<PdfReal, double>(reader, context);
			double[] array4 = this.Encode.ToArray<PdfReal, double>(reader, context);
			double[] array5 = this.Decode.ToArray<PdfReal, double>(reader, context);
			return (FunctionBase)new SampledFunction(functionData, array1, bitsPerSample, array2, array3, array4, array5);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function)
		{
			SampledFunction sampledFunction = (SampledFunction)function;
			this.BitsPerSample = new PdfInt(sampledFunction.BitsPerSample);
			this.Size = sampledFunction.Size.ToPdfArray();
			this.Encode = sampledFunction.Encode.ToPdfArray();
			this.Decode = sampledFunction.Decode.ToPdfArray();
		}

		readonly DirectProperty<PdfArray> size;

		readonly DirectProperty<PdfInt> bitsPerSample;

		readonly DirectProperty<PdfArray> encode;

		readonly DirectProperty<PdfArray> decode;
	}
}
