using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetRgbColorBase : SetSimpleColorBase<RgbColorObject, DeviceRgbColorSpaceObject>
	{
		public void Write(PdfWriter writer, IPdfContentExportContext context, RgbColor color)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { color.ToRgbColor(context) });
		}

		protected sealed override void ReadColorComponents(ContentStreamInterpreter interpreter, IPdfContentImportContext context, out RgbColorObject color, out DeviceRgbColorSpaceObject colorspace)
		{
			PdfReal b = SetSimpleColorBase<RgbColorObject, DeviceRgbColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			PdfReal g = SetSimpleColorBase<RgbColorObject, DeviceRgbColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			PdfReal r = SetSimpleColorBase<RgbColorObject, DeviceRgbColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			color = new RgbColorObject(r, g, b);
			colorspace = new DeviceRgbColorSpaceObject();
		}
	}
}
