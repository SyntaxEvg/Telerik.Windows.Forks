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
	class AxialShading : GradientShading
	{
		protected override void CopyPropertiesFromOverride(IPdfContentExportContext context, Gradient gradient)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Gradient>(gradient, "gradient");
			base.Coordinates = new PdfArray(new PdfPrimitive[0]);
			base.Coordinates.Add(gradient.StartPoint.X.ToPdfReal());
			base.Coordinates.Add(gradient.StartPoint.Y.ToPdfReal());
			base.Coordinates.Add(gradient.EndPoint.X.ToPdfReal());
			base.Coordinates.Add(gradient.EndPoint.Y.ToPdfReal());
			base.CopyPropertiesFromOverride(context, gradient);
		}

		protected override Gradient ToColorOverride(PostScriptReader reader, IPdfContentImportContext context, Matrix matrix)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Point point = new Point(base.Coordinates[0].ToReal(), base.Coordinates[1].ToReal());
			Point point2 = new Point(base.Coordinates[2].ToReal(), base.Coordinates[3].ToReal());
			LinearGradient linearGradient = new LinearGradient(point, point2);
			double length = (point2 - point).Length;
			base.InitializeGradientStops(reader, context, linearGradient, matrix, length);
			return linearGradient;
		}
	}
}
