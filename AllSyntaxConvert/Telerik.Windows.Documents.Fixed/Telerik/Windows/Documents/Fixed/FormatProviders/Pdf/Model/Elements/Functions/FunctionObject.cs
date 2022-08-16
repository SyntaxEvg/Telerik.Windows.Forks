using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions
{
	abstract class FunctionObject : PdfStreamObjectBase
	{
		public FunctionObject()
		{
			this.domain = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Domain"));
			this.range = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Range"));
			this.functionType = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("FunctionType"));
		}

		public PdfArray Domain
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

		public PdfArray Range
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

		public PdfInt FunctionType
		{
			get
			{
				return this.functionType.GetValue();
			}
			set
			{
				this.functionType.SetValue(value);
			}
		}

		public override PdfElementType Type
		{
			get
			{
				if (!this.HasStreamData)
				{
					return PdfElementType.PdfObject;
				}
				return base.Type;
			}
		}

		protected byte[] FunctionData
		{
			get
			{
				return this.data;
			}
		}

		protected abstract bool HasStreamData { get; }

		public static FunctionObject CreateFunction(PdfInt type)
		{
			Guard.ThrowExceptionIfNull<PdfInt>(type, "type");
			switch (type.Value)
			{
			case 0:
				return new SampledFunctionObject();
			case 2:
				return new ExponentialInterpolationFunctionObject();
			case 3:
				return new StitchingFunctionObject();
			case 4:
				return new PostScriptCalculatorFunctionObject();
			}
			throw new NotSupportedFunctionTypeException(type.Value);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, FunctionBase function)
		{
			this.Domain = function.Domain.ToPdfArray();
			this.Range = function.Range.ToPdfArray();
			if (this.HasStreamData)
			{
				this.data = function.GetFunctionData();
			}
			this.FunctionType = new PdfInt((int)function.FunctionType);
			this.CopyPropertiesFromOverride(context, function);
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			return this.FunctionData;
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			this.data = stream.ReadDecodedPdfData();
		}

		public abstract FunctionBase ToFunction(PostScriptReader reader, IPdfImportContext context);

		protected abstract void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function);

		readonly DirectProperty<PdfArray> domain;

		readonly DirectProperty<PdfArray> range;

		readonly DirectProperty<PdfInt> functionType;

		byte[] data;
	}
}
