using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetGrayColorBase : SetSimpleColorBase<GrayColorObject, DeviceGrayColorSpaceObject>
	{
		protected sealed override void ReadColorComponents(ContentStreamInterpreter interpreter, IPdfContentImportContext context, out GrayColorObject color, out DeviceGrayColorSpaceObject colorspace)
		{
			PdfReal g = SetSimpleColorBase<GrayColorObject, DeviceGrayColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			color = new GrayColorObject(g);
			colorspace = new DeviceGrayColorSpaceObject();
		}
	}
}
