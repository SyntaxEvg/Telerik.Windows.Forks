using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class RgbColorObject : ColorObjectBase
	{
		public RgbColorObject()
			: this(0.0, 0.0, 0.0)
		{
		}

		public RgbColorObject(double r, double g, double b)
			: this(new PdfReal(r), new PdfReal(g), new PdfReal(b))
		{
		}

		public RgbColorObject(PdfReal r, PdfReal g, PdfReal b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public PdfReal R { get; set; }

		public PdfReal G { get; set; }

		public PdfReal B { get; set; }

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			return new RgbColor
			{
				R = ColorObjectBase.ConvertToByte(this.R),
				G = ColorObjectBase.ConvertToByte(this.G),
				B = ColorObjectBase.ConvertToByte(this.B)
			};
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, ColorBase color)
		{
			Guard.ThrowExceptionIfNull<ColorBase>(color, "color");
			RgbColor rgbColor = (RgbColor)color;
			this.R = ColorObjectBase.ConvertFromByte(rgbColor.R);
			this.G = ColorObjectBase.ConvertFromByte(rgbColor.G);
			this.B = ColorObjectBase.ConvertFromByte(rgbColor.B);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			this.R.Write(writer, context);
			writer.WriteSeparator();
			this.G.Write(writer, context);
			writer.WriteSeparator();
			this.B.Write(writer, context);
		}
	}
}
