using System;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	abstract class ColorWriterBase : ContentElementWriter<ColorBase>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, ColorBase element)
		{
			string name;
			if (context.Owner.CanWriteColors && (name = element.ColorSpace.Name) != null)
			{
				if (name == "DeviceRGB")
				{
					this.WriteRgbColor(writer, context, (RgbColor)element);
					return;
				}
				if (!(name == "Pattern"))
				{
					return;
				}
				this.WritePatternColor(writer, context, (PatternColor)element);
			}
		}

		protected abstract void WriteRgbColor(PdfWriter writer, IPdfContentExportContext context, RgbColor color);

		protected abstract void WritePatternColor(PdfWriter writer, IPdfContentExportContext context, PatternColor color);
	}
}
