using System;
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
	class LabColorSpaceObject : ColorSpaceObject
	{
		public LabColorSpaceObject()
		{
			this.whitePoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("WhitePoint", true));
			this.blackPoint = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("BlackPoint"));
			this.range = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Range"));
		}

		public override string Name
		{
			get
			{
				return "Lab";
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
				return ColorSpace.Lab;
			}
		}

		public PdfArray WhitePoint
		{
			get
			{
				return this.whitePoint.GetValue();
			}
			set
			{
				this.whitePoint.SetValue(value);
			}
		}

		public PdfArray BlackPoint
		{
			get
			{
				return this.blackPoint.GetValue();
			}
			set
			{
				this.blackPoint.SetValue(value);
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

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		public override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components)
		{
			throw new NotImplementedException();
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(PdfNames.PdfArrayStart);
			base.Write(writer, context);
			writer.Write(PdfNames.PdfArrayEnd);
		}

		public override ColorSpaceBase ToColorSpace()
		{
			Lab lab = new Lab();
			lab.WhitePoint = this.WhitePoint.ToDoubleArray();
			if (this.blackPoint.HasValue)
			{
				lab.BlackPoint = this.BlackPoint.ToDoubleArray();
			}
			if (this.range.HasValue)
			{
				lab.Range = this.Range.ToDoubleArray();
			}
			return lab;
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			Lab lab = (Lab)colorSpaceBase;
			this.WhitePoint = lab.WhitePoint.ToPdfArray();
			if (lab.BlackPoint != null)
			{
				this.BlackPoint = lab.BlackPoint.ToPdfArray();
			}
			if (lab.Range != null)
			{
				this.Range = lab.Range.ToPdfArray();
			}
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			base.Load(reader, context, array[1]);
		}

		readonly ReferenceProperty<PdfArray> whitePoint;

		readonly ReferenceProperty<PdfArray> blackPoint;

		readonly ReferenceProperty<PdfArray> range;
	}
}
