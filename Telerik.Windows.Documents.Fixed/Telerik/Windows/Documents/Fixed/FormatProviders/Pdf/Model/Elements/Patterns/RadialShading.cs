using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	class RadialShading : GradientShading
	{
		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, Gradient gradient)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Gradient>(gradient, "gradient");
			RadialGradient radialGradient = (RadialGradient)gradient;
			base.Coordinates = new PdfArray(new PdfPrimitive[0]);
			base.Coordinates.Add(radialGradient.StartPoint.X.ToPdfReal());
			base.Coordinates.Add(radialGradient.StartPoint.Y.ToPdfReal());
			base.Coordinates.Add(radialGradient.StartRadius.ToPdfReal());
			base.Coordinates.Add(radialGradient.EndPoint.X.ToPdfReal());
			base.Coordinates.Add(radialGradient.EndPoint.Y.ToPdfReal());
			base.Coordinates.Add(radialGradient.EndRadius.ToPdfReal());
			base.CopyPropertiesFromOverride(context, gradient);
		}

		protected override Gradient ToColorOverride(PostScriptReader reader, IPdfContentImportContext context, Matrix matrix)
		{
			Point point = new Point(base.Coordinates[0].ToReal(), base.Coordinates[1].ToReal());
			double num = base.Coordinates[2].ToReal();
			Point point2 = new Point(base.Coordinates[3].ToReal(), base.Coordinates[4].ToReal());
			double num2 = base.Coordinates[5].ToReal();
			double length = (point2 - point).Length;
			double num3 = Math.Abs(num - num2);
			RadialGradient radialGradient = new RadialGradient(point, point2, num, num2);
			base.InitializeGradientStops(reader, context, radialGradient, matrix, num3 + length);
			return radialGradient;
		}
	}
}
