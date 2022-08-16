using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Functions
{
	abstract class FunctionOld : PdfObjectOld
	{
		public FunctionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.domain = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Domain", true));
			this.range = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Range"));
		}

		public PdfArrayOld Domain
		{
			get
			{
				return this.domain.GetValue();
			}
			set
			{
				this.domain.SetValue(value);
			}
		}

		public PdfArrayOld Range
		{
			get
			{
				return this.range.GetValue();
			}
			set
			{
				this.range.SetValue(value);
			}
		}

		internal FunctionBase FunctionModel { get; set; }

		public static FunctionOld CreateFunction(PdfContentManager contentManager, PdfDataStream pdfStream)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(pdfStream, "pdfStream");
			FunctionOld functionOld = FunctionOld.CreateFunctionInternal(contentManager, pdfStream.Dictionary);
			byte[] data = pdfStream.ReadData(contentManager);
			functionOld.FunctionModel = functionOld.CreateFunctionModel(data);
			return functionOld;
		}

		public static FunctionOld CreateFunction(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dict, "dict");
			FunctionOld functionOld = FunctionOld.CreateFunctionInternal(contentManager, dict);
			functionOld.FunctionModel = functionOld.CreateFunctionModel();
			return functionOld;
		}

		public double[] Execute(double[] inputValues)
		{
			Guard.ThrowExceptionIfNull<FunctionBase>(this.FunctionModel, "FunctionModel");
			return this.FunctionModel.Execute(inputValues);
		}

		protected abstract FunctionBase CreateFunctionModel();

		protected abstract FunctionBase CreateFunctionModel(byte[] data);

		protected double[] ReadDoubleArray(PdfArrayOld pdfArray)
		{
			if (pdfArray == null)
			{
				return null;
			}
			double[] array = new double[pdfArray.Count];
			for (int i = 0; i < array.Length; i++)
			{
				double num;
				if (pdfArray.TryGetReal(i, out num))
				{
					array[i] = num;
				}
			}
			return array;
		}

		static FunctionOld CreateFunctionInternal(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dict, "dict");
			int functionType;
			dict.TryGetInt("FunctionType", out functionType);
			FunctionOld functionOld;
			switch (functionType)
			{
			case 0:
				functionOld = new SampledFunctionOld(contentManager);
				goto IL_6D;
			case 2:
				functionOld = new ExponentialInterpolationFunctionOld(contentManager);
				goto IL_6D;
			case 3:
				functionOld = new StitchingFunctionOld(contentManager);
				goto IL_6D;
			case 4:
				functionOld = new PostScriptCalculatorFunctionOld(contentManager);
				goto IL_6D;
			}
			throw new NotSupportedFunctionTypeException(functionType);
			IL_6D:
			functionOld.Load(dict);
			return functionOld;
		}

		readonly InstantLoadProperty<PdfArrayOld> domain;

		readonly InstantLoadProperty<PdfArrayOld> range;
	}
}
