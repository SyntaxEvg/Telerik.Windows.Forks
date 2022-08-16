using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class SeparationColorSpaceObject : PdfRealColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "Separation";
			}
		}

		public override ColorObjectBase DefaultColor
		{
			get
			{
				return new RgbColorObject(0.0, 0.0, 0.0);
			}
		}

		public override ColorSpace Public
		{
			get
			{
				return ColorSpace.Separation;
			}
		}

		public PdfName ColorantName { get; set; }

		public ColorSpaceObject AlternateColorSpace { get; set; }

		public FunctionBase TintTransform { get; set; }

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		protected override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components)
		{
			if (components.Length > 0)
			{
				double[] array = new double[components.Length];
				for (int i = 0; i < components.Length; i++)
				{
					array[i] = components[i].Value;
				}
				double[] array2 = this.TintTransform.Execute(array);
				PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
				foreach (double initialValue in array2)
				{
					pdfArray.Add(new PdfReal(initialValue));
				}
				return this.AlternateColorSpace.GetColor(context, reader, pdfArray);
			}
			return null;
		}

		public override ColorSpaceBase ToColorSpace()
		{
			return new Separation
			{
				ColorantName = this.ColorantName.ToString(),
				AlternateColorSpace = this.AlternateColorSpace.ToColorSpace(),
				TintTransform = this.TintTransform
			};
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			Separation separation = (Separation)colorSpaceBase;
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(separation.AlternateColorSpace.Name);
			colorSpaceObject.CopyPropertiesFrom(separation.AlternateColorSpace);
			this.ColorantName = new PdfName(separation.ColorantName);
			this.AlternateColorSpace = colorSpaceObject;
			this.TintTransform = separation.TintTransform;
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			this.ColorantName = array[1] as PdfName;
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(ColorSpaceObject));
			this.AlternateColorSpace = pdfObjectDescriptor.Converter.Convert(typeof(ColorSpaceObject), reader, context, array[2]) as ColorSpaceObject;
			PdfObjectDescriptor pdfObjectDescriptor2 = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(FunctionObject));
			FunctionObject functionObject = (FunctionObject)pdfObjectDescriptor2.Converter.Convert(typeof(FunctionObject), reader, context, array[3]);
			this.TintTransform = functionObject.ToFunction(reader, context);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			pdfArray.Add(new PdfName(this.Name));
			pdfArray.Add(this.ColorantName);
			IndirectObject indirectObject = context.CreateIndirectObject(this.AlternateColorSpace);
			pdfArray.Add(indirectObject.Reference);
			FunctionObject functionObject = FunctionObject.CreateFunction(new PdfInt((int)this.TintTransform.FunctionType));
			functionObject.CopyPropertiesFrom(context, this.TintTransform);
			IndirectObject indirectObject2 = context.CreateIndirectObject(functionObject);
			pdfArray.Add(indirectObject2.Reference);
			pdfArray.Write(writer, context);
		}
	}
}
