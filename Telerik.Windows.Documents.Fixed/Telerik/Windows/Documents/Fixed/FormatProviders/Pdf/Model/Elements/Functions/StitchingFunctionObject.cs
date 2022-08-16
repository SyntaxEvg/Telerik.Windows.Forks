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
	class StitchingFunctionObject : FunctionObject
	{
		public StitchingFunctionObject()
		{
			this.functions = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Functions", true));
			this.bounds = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Bounds", true));
			this.encode = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Encode", true));
		}

		public override PdfElementType ExportAs
		{
			get
			{
				return PdfElementType.PdfObject;
			}
		}

		public PdfArray Functions
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

		public PdfArray Bounds
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

		protected override bool HasStreamData
		{
			get
			{
				return false;
			}
		}

		public override FunctionBase ToFunction(PostScriptReader reader, IPdfImportContext context)
		{
			FunctionBase[] array = new FunctionBase[this.Functions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				FunctionObject functionObject;
				if (!this.Functions.TryGetElement<FunctionObject>(reader, context, i, out functionObject))
				{
					throw new InvalidOperationException(string.Format("Cannot get function at index: {0}", i));
				}
				FunctionBase functionBase = functionObject.ToFunction(reader, context);
				array[i] = functionBase;
			}
			double[] domain = base.Domain.ToArray(reader, context);
			double[] range = base.Range.ToArray(reader, context);
			double[] array2 = this.Bounds.ToArray(reader, context);
			double[] array3 = this.Encode.ToArray(reader, context);
			return new StitchingFunction(array, array2, array3, domain, range);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, FunctionBase function)
		{
			StitchingFunction stitchingFunction = (StitchingFunction)function;
			this.Bounds = stitchingFunction.Bounds.ToPdfArray();
			this.Encode = stitchingFunction.Encode.ToPdfArray();
			this.Functions = new PdfArray(new PdfPrimitive[0]);
			for (int i = 0; i < stitchingFunction.Functions.Length; i++)
			{
				FunctionBase functionBase = stitchingFunction.Functions[i];
				FunctionObject functionObject = FunctionObject.CreateFunction(new PdfInt((int)functionBase.FunctionType));
				functionObject.CopyPropertiesFrom(context, functionBase);
				IndirectObject indirectObject = context.CreateIndirectObject(functionObject);
				this.Functions.Add(indirectObject.Reference);
			}
		}

		readonly DirectProperty<PdfArray> functions;

		readonly DirectProperty<PdfArray> bounds;

		readonly DirectProperty<PdfArray> encode;
	}
}
