using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class SLWPFBridge
	{
		public static double GetWidth(this TextBlock block)
		{
			return block.GetSize().Width;
		}

		public static double GetHeight(this TextBlock block)
		{
			return block.GetSize().Height;
		}

		internal static double GetBaselineOffset(this TextBlock textBlock)
		{
			textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return textBlock.BaselineOffset;
		}

		internal static Size GetSize(this TextBlock block)
		{
			block.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return block.DesiredSize;
		}

		internal static string GetFontFileName(this GlyphTypeface typeFace)
		{
			return Path.GetFileName(typeFace.FontUri.AbsolutePath);
		}
	}
}
