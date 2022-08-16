using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class DeviceNColorSpaceObject : PdfRealColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "DeviceN";
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
				return ColorSpace.DeviceN;
			}
		}

		public string[] Names { get; set; }

		public ColorSpaceObject AlternateColorSpace { get; set; }

		public FunctionBase TintTransform { get; set; }

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			DeviceN deviceN = (DeviceN)colorSpaceBase;
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(deviceN.AlternateColorSpace.Name);
			colorSpaceObject.CopyPropertiesFrom(deviceN.AlternateColorSpace);
			this.Names = deviceN.Names;
			this.AlternateColorSpace = colorSpaceObject;
			this.TintTransform = deviceN.TintTransform;
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
			return new DeviceN
			{
				Names = this.Names,
				AlternateColorSpace = this.AlternateColorSpace.ToColorSpace(),
				TintTransform = this.TintTransform
			};
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfArray array2 = (PdfArray)array[1];
			this.Names = ((PdfArray)array[1]).ToArray<PdfName, string>(reader, context);
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(ColorSpaceObject));
			this.AlternateColorSpace = pdfObjectDescriptor.Converter.Convert(typeof(ColorSpaceObject), reader, context, array[2]) as ColorSpaceObject;
			PdfObjectDescriptor pdfObjectDescriptor2 = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(FunctionObject));
			FunctionObject functionObject = pdfObjectDescriptor2.Converter.Convert(typeof(FunctionObject), reader, context, array[3]) as FunctionObject;
			this.TintTransform = functionObject.ToFunction(reader, context);
			int count = array.Count;
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			pdfArray.Add(new PdfName(this.Name));
			pdfArray.Add(new PdfArray(from text in this.Names select new PdfName(text)));
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
