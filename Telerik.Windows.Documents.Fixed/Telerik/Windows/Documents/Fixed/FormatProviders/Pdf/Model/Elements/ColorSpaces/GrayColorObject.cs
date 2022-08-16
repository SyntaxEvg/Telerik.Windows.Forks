using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class GrayColorObject : ColorObjectBase
	{
		public GrayColorObject()
			: this(0.0)
		{
		}

		public GrayColorObject(double g)
			: this(new PdfReal(g))
		{
		}

		public GrayColorObject(PdfReal g)
		{
			this.G = g;
		}

		public PdfReal G { get; set; }

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			byte b = ColorObjectBase.ConvertToByte(this.G);
			return new RgbColor(b, b, b);
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}
	}
}
