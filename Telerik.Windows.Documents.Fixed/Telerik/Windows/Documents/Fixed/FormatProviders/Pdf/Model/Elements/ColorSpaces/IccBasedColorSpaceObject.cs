using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class IccBasedColorSpaceObject : ColorSpaceObject
	{
		public IccBasedColorSpaceObject()
		{
			this.IccProfile = new IccProfileStreamObject();
		}

		public override string Name
		{
			get
			{
				return "ICCBased";
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
				return ColorSpace.ICCBased;
			}
		}

		public IccProfileStreamObject IccProfile { get; set; }

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		public override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components)
		{
			ColorSpaceObject colorSpaceObject;
			if (this.IccProfile.TryGetAlternateColorSpace(out colorSpaceObject))
			{
				return colorSpaceObject.GetColor(context, reader, components);
			}
			throw new NotSupportedException("Cannot get color without alternate colorspace.");
		}

		public override ColorSpaceBase ToColorSpace()
		{
			IccBased iccBased = new IccBased();
			iccBased.Data = this.IccProfile.Data.ToArray<byte>();
			if (this.IccProfile.Alternate != null)
			{
				iccBased.Alternate = this.IccProfile.Alternate.ToColorSpace();
			}
			iccBased.N = this.IccProfile.N.Value;
			if (this.IccProfile.Range != null)
			{
				iccBased.Range = this.IccProfile.Range.ToDoubleArray();
			}
			return iccBased;
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			IccBased iccBased = (IccBased)colorSpaceBase;
			IccProfileStreamObject iccProfileStreamObject = new IccProfileStreamObject(iccBased.Data);
			if (iccBased.Alternate != null)
			{
				iccProfileStreamObject.Alternate = ColorSpaceManager.CreateColorSpaceObject(iccBased.Alternate);
			}
			iccProfileStreamObject.N = iccBased.N.ToPdfInt();
			if (iccBased.Range != null)
			{
				iccProfileStreamObject.Range = iccBased.Range.ToPdfArray();
			}
			this.IccProfile = iccProfileStreamObject;
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(IccProfileStreamObject));
			IccProfileStreamObject iccProfile = pdfObjectDescriptor.Converter.Convert(typeof(IccProfileStreamObject), reader, context, array[1]) as IccProfileStreamObject;
			this.IccProfile = iccProfile;
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[]
			{
				new PdfName(this.Name),
				this.IccProfile
			});
			pdfArray.Write(writer, context);
		}
	}
}
