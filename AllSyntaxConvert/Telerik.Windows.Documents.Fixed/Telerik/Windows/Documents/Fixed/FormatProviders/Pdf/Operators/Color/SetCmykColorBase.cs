using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetCmykColorBase : SetSimpleColorBase<CmykColorObject, DeviceCmykColorSpaceObject>
	{
		protected sealed override void ReadColorComponents(ContentStreamInterpreter interpreter, IPdfContentImportContext context, out CmykColorObject color, out DeviceCmykColorSpaceObject colorspace)
		{
			PdfReal k = SetSimpleColorBase<CmykColorObject, DeviceCmykColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			PdfReal y = SetSimpleColorBase<CmykColorObject, DeviceCmykColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			PdfReal m = SetSimpleColorBase<CmykColorObject, DeviceCmykColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			PdfReal c = SetSimpleColorBase<CmykColorObject, DeviceCmykColorSpaceObject>.ReadRestrictedNumberComponentValue(interpreter, context);
			color = new CmykColorObject(c, m, y, k);
			colorspace = new DeviceCmykColorSpaceObject();
		}
	}
}
