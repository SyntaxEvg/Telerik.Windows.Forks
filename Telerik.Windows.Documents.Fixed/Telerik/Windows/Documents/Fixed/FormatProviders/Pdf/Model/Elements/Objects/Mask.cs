using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects
{
	class Mask : ImageXObject
	{
		public Mask()
		{
			this.colorKeyMaskArray = null;
		}

		public Mask(PdfArray colorKeyMaskArray)
		{
			this.colorKeyMaskArray = colorKeyMaskArray;
		}

		public bool IsColorKeyMask
		{
			get
			{
				return this.colorKeyMaskArray != null;
			}
		}

		public PdfArray ColorKeyMaskArray
		{
			get
			{
				return this.colorKeyMaskArray;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			if (this.IsColorKeyMask)
			{
				this.colorKeyMaskArray.Write(writer, context);
				return;
			}
			base.Write(writer, context);
		}

		internal static ImageMask CreateImageMask(Mask mask)
		{
			if (mask.IsColorKeyMask)
			{
				return new ImageMask(mask.ColorKeyMaskArray.ToIntArray());
			}
			return new ImageMask(mask.ToImageSource());
		}

		readonly PdfArray colorKeyMaskArray;
	}
}
