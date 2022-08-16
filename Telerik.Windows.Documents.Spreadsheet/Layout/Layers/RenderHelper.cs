using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.TextMeasurer;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	static class RenderHelper
	{
		static RenderHelper()
		{
			double num = 100.0;
			TextMeasurementInfo textMeasurementInfo = RadTextMeasurer.Measure(RenderHelper.SpaceChar, new FontProperties(RenderHelper.SpaceFont, num, false), null);
			RenderHelper.SpaceFactor = num / textMeasurementInfo.Size.Width;
		}

		public static RunRenderable CreateSpaceRun(double gapWidth)
		{
			return new RunRenderable
			{
				Text = RenderHelper.SpaceChar,
				FontFamily = RenderHelper.SpaceFont,
				FontSize = new double?(gapWidth * RenderHelper.SpaceFactor),
				Foreground = new SolidColorBrush(Colors.Transparent)
			};
		}

		static readonly FontFamily SpaceFont = new FontFamily("Arial");

		static readonly double SpaceFactor;

		static readonly string SpaceChar = "+";
	}
}
