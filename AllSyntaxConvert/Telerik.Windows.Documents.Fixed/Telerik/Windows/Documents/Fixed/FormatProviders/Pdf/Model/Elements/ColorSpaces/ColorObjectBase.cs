using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	abstract class ColorObjectBase : PdfObject
	{
		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Color;
			}
		}

		public abstract ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context);

		public void CopyPropertiesFrom(IPdfContentExportContext context, ColorBase color)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ColorBase>(color, "color");
			this.CopyPropertiesFromOverride(context, color);
		}

		public static byte ConvertToByte(PdfReal component)
		{
			return (byte)Math.Round(255.0 * component.Value);
		}

		public static PdfReal ConvertFromByte(byte component)
		{
			return new PdfReal((double)component / 255.0);
		}

		protected abstract void CopyPropertiesFromOverride(IPdfContentExportContext context, ColorBase color);
	}
}
