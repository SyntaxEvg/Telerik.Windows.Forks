using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Utilities
{
	static class Extensions
	{
		public static byte[] ReadAllBytes(this Stream reader)
		{
			if (!reader.CanRead)
			{
				return null;
			}
			if (reader.CanSeek)
			{
				reader.Seek(0L, SeekOrigin.Begin);
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				reader.CopyTo(memoryStream);
				byte[] array;
				if (memoryStream.Length == (long)memoryStream.Capacity)
				{
					array = memoryStream.GetBuffer();
				}
				else
				{
					array = memoryStream.ToArray();
				}
				result = array;
			}
			return result;
		}

		public static bool IsNullEmptyOrWhiteSpace(string str)
		{
			return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
		}

		public static Size GetSize(this TextBlock block)
		{
			block.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return block.DesiredSize;
		}

		public static double GetBaselineOffset(this TextBlock textBlock)
		{
			textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return textBlock.BaselineOffset;
		}

		internal static string GetFontFileName(this GlyphTypeface typeFace)
		{
			return Path.GetFileName(typeFace.FontUri.AbsolutePath);
		}

		const int BufferSize = 10485760;
	}
}
