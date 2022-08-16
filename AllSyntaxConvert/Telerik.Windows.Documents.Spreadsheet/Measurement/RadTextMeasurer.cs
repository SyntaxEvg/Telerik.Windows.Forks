using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.TextMeasurer;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Measurement
{
	static class RadTextMeasurer
	{
		public static Size MeasureMultiline(string text, Telerik.Windows.Documents.Spreadsheet.Model.FontProperties fontProperties, double? wrappingWidth)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Spreadsheet.Model.FontProperties>(fontProperties, "fontProperties");
			if (wrappingWidth != null)
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, wrappingWidth.Value, "wrappingWidth");
			}
			double num = 0.0;
			double num2 = 0.0;
			text = TextHelper.SanitizeNewLines(text);
			foreach (string text2 in text.Split(new char[] { LineBreak.NewLine }))
			{
				TextMeasurementInfo textMeasurementInfo = RadTextMeasurer.Measure(text2, fontProperties, wrappingWidth);
				num = Math.Max(num, textMeasurementInfo.Size.Width);
				num2 += textMeasurementInfo.Size.Height;
			}
			return new Size(num, num2);
		}

		public static TextMeasurementInfo Measure(string text, Telerik.Windows.Documents.Spreadsheet.Model.FontProperties fontProperties, double? wrappingWidth = null)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Spreadsheet.Model.FontProperties>(fontProperties, "fontProperties");
			if (wrappingWidth != null)
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, wrappingWidth.Value, "wrappingWidth");
			}
			Telerik.Windows.Documents.Core.Fonts.FontProperties fontProperties2 = new Telerik.Windows.Documents.Core.Fonts.FontProperties(fontProperties.FontFamily, fontProperties.FontStyle, fontProperties.FontWeight);
			TextProperties textProperties = new TextProperties(text, fontProperties.FontSize, SubStringPosition.None);
			if (wrappingWidth != null)
			{
				return global::Telerik.Windows.Documents.Core.TextMeasurer.RadTextMeasurer.MeasureTextWithWrapping(textProperties, fontProperties2, wrappingWidth.Value);
			}
			return RadTextMeasurer.textMeasurer.MeasureText(textProperties, fontProperties2);
		}

		private static readonly global::Telerik.Windows.Documents.Core.TextMeasurer.RadTextMeasurer textMeasurer = new global::Telerik.Windows.Documents.Core.TextMeasurer.RadTextMeasurer();
	}
}
