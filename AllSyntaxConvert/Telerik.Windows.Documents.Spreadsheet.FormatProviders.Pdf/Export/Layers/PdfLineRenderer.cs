using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfLineRenderer : PdfRenderer<LineRenderable>
	{
		public override void RenderOverride(LineRenderable renderable, ViewportPaneType paneType)
		{
			GraphicProperties graphicProperties = base.Editor.GraphicProperties;
			graphicProperties.StrokeColor = new RgbColor(renderable.Stroke.R, renderable.Stroke.G, renderable.Stroke.B);
			graphicProperties.StrokeThickness = renderable.StrokeThickness;
			DoubleCollection strokeDashArrayByBorderStyle = BorderRenderHelper.GetStrokeDashArrayByBorderStyle(renderable.CellBorderStyle);
			if (strokeDashArrayByBorderStyle == null)
			{
				graphicProperties.StrokeDashArray = null;
			}
			else
			{
				double[] array = new double[strokeDashArrayByBorderStyle.Count];
				strokeDashArrayByBorderStyle.CopyTo(array, 0);
				graphicProperties.StrokeDashArray = array;
			}
			base.Editor.DrawLine(new Point(renderable.X1, renderable.Y1), new Point(renderable.X2, renderable.Y2));
		}
	}
}
