using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class IndexedColorSpaceObject : ColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "Indexed";
			}
		}

		public ColorSpaceObject Base { get; set; }

		public int HiVal { get; set; }

		public PdfPrimitive Lookup { get; set; }

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
				return ColorSpace.Indexed;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			PdfHexString pdfHexString = this.Lookup as PdfHexString;
			PdfPrimitive pdfPrimitive;
			if (pdfHexString != null)
			{
				pdfPrimitive = pdfHexString;
			}
			else
			{
				pdfPrimitive = this.Lookup;
			}
			PdfArray primitive = new PdfArray(new PdfPrimitive[]
			{
				new PdfName(this.Name),
				this.Base,
				new PdfInt(this.HiVal),
				pdfPrimitive
			});
			IndirectObject indirectObject = context.CreateIndirectObject(primitive);
			writer.WriteIndirectReference(indirectObject.Reference);
		}

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		public override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components)
		{
			throw new NotImplementedException();
		}

		public override ColorSpaceBase ToColorSpace()
		{
			Indexed indexed = new Indexed();
			indexed.Base = this.Base.ToColorSpace();
			indexed.HiVal = this.HiVal;
			PdfLiteralString pdfLiteralString = this.Lookup as PdfLiteralString;
			if (pdfLiteralString != null)
			{
				indexed.Lookup = pdfLiteralString.Value;
			}
			else
			{
				IndexedLookupStream indexedLookupStream = this.Lookup as IndexedLookupStream;
				if (indexedLookupStream != null)
				{
					indexed.Lookup = indexedLookupStream.Data.ToArray<byte>();
				}
			}
			return indexed;
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			Indexed indexed = (Indexed)colorSpaceBase;
			this.Base = ColorSpaceManager.CreateColorSpaceObject(indexed.Base);
			this.HiVal = indexed.HiVal;
			this.Lookup = new IndexedLookupStream(indexed.Lookup);
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(ColorSpaceObject));
			ColorSpaceObject @base = pdfObjectDescriptor.Converter.Convert(typeof(ColorSpaceObject), reader, context, array[1]) as ColorSpaceObject;
			this.Base = @base;
			PdfObjectDescriptor pdfObjectDescriptor2 = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(PdfInt));
			PdfInt pdfInt = pdfObjectDescriptor2.Converter.Convert(typeof(PdfInt), reader, context, array[2]) as PdfInt;
			this.HiVal = pdfInt.Value;
			if (array[3].Type == PdfElementType.String)
			{
				PdfString pdfString = (PdfString)array[3];
				this.Lookup = new IndexedLookupStream(pdfString.Value);
				return;
			}
			PdfObjectDescriptor pdfObjectDescriptor3 = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(IndexedLookupStream));
			IndexedLookupStream lookup = pdfObjectDescriptor3.Converter.Convert(typeof(IndexedLookupStream), reader, context, array[3]) as IndexedLookupStream;
			this.Lookup = lookup;
		}
	}
}
