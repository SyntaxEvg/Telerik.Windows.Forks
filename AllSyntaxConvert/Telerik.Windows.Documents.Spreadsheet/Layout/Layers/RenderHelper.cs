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
			global::Telerik.Windows.Documents.Core.TextMeasurer.TextMeasurementInfo textMeasurementInfo = global::Telerik.Windows.Documents.Spreadsheet.Measurement.RadTextMeasurer.Measure(global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceChar, new global::Telerik.Windows.Documents.Spreadsheet.Model.FontProperties(global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceFont, num, false), null);
			global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceFactor = num / textMeasurementInfo.Size.Width;
		}

		public static global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RunRenderable CreateSpaceRun(double gapWidth)
		{
			return new global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RunRenderable
			{
				Text = global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceChar,
				FontFamily = global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceFont,
				FontSize = new double?(gapWidth * global::Telerik.Windows.Documents.Spreadsheet.Layout.Layers.RenderHelper.SpaceFactor),
				Foreground = new global::System.Windows.Media.SolidColorBrush(global::System.Windows.Media.Colors.Transparent)
			};
		}

		private static readonly global::System.Windows.Media.FontFamily SpaceFont = new global::System.Windows.Media.FontFamily("Arial");

		private static readonly double SpaceFactor;

		private static readonly string SpaceChar = "+";
	}
}
