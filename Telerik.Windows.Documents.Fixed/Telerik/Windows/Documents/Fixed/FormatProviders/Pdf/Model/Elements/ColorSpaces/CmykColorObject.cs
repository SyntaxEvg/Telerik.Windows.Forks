using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class CmykColorObject : ColorObjectBase
	{
		public CmykColorObject()
			: this(0.0, 0.0, 0.0, 1.0)
		{
		}

		public CmykColorObject(double c, double m, double y, double k)
			: this(new PdfReal(c), new PdfReal(m), new PdfReal(y), new PdfReal(k))
		{
		}

		public CmykColorObject(PdfReal c, PdfReal m, PdfReal y, PdfReal k)
		{
			this.C = c;
			this.M = m;
			this.Y = y;
			this.K = k;
		}

		public PdfReal C { get; set; }

		public PdfReal M { get; set; }

		public PdfReal Y { get; set; }

		public PdfReal K { get; set; }

		public override ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			Color color = Color.FromCmyk(this.C.Value, this.M.Value, this.Y.Value, this.K.Value);
			return new RgbColor(color.R, color.G, color.B);
		}

		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}
	}
}
