using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model;
using PatternType = Telerik.Windows.Documents.Spreadsheet.Model.PatternType;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfPatternFillRenderer : PdfRenderer<PatternFillRenderable>
	{
		public override void RenderOverride(PatternFillRenderable renderable, ViewportPaneType paneType)
		{
			base.Editor.GraphicProperties.StrokeThickness = 0.0;
			base.Editor.GraphicProperties.IsStroked = false;
			RgbColor rgbColor = renderable.PatternFill.GetBackgroundDependingOnPatternType().GetActualValue(renderable.ColorScheme).ToPdfRgbColor();
			if (renderable.PatternFill.PatternType == PatternType.Solid)
			{
				base.Editor.GraphicProperties.FillColor = rgbColor;
			}
			else
			{
				RgbColor foreground = renderable.PatternFill.PatternColor.GetActualValue(renderable.ColorScheme).ToPdfRgbColor();
				int[,] pattern = PatternsRenderHelper.GetPattern(renderable.PatternFill.PatternType);
				base.Editor.GraphicProperties.FillColor = PdfPatternFillRenderer.CalculateTiling(pattern, foreground, rgbColor);
			}
			foreach (Rect rectangle in renderable.VisibleBoxes)
			{
				base.Editor.DrawRectangle(rectangle);
			}
		}

		static Tiling CalculateTiling(int[,] pattern, RgbColor foreground, RgbColor background)
		{
			int length = pattern.GetLength(0);
			int length2 = pattern.GetLength(1);
			Tiling tiling = new Tiling();
			tiling.BoundingBox = new Rect(0.0, 0.0, (double)length, (double)length2);
			FixedContentEditor fixedContentEditor = new FixedContentEditor(tiling);
			fixedContentEditor.GraphicProperties.StrokeThickness = 0.0;
			fixedContentEditor.GraphicProperties.IsStroked = false;
			fixedContentEditor.GraphicProperties.FillColor = background;
			fixedContentEditor.DrawRectangle(tiling.BoundingBox);
			fixedContentEditor.GraphicProperties.FillColor = foreground;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					if (pattern[i, j] == 1)
					{
						fixedContentEditor.DrawRectangle(new Rect((double)j, (double)i, 1.0, 1.0));
					}
				}
			}
			return tiling;
		}
	}
}
