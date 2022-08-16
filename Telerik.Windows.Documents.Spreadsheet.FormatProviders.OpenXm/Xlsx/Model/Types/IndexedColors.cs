using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class IndexedColors
	{
		static IndexedColors()
		{
			IndexedColors.RegisterIndexedColor(0, "FF000000");
			IndexedColors.RegisterIndexedColor(1, "FFFFFFFF");
			IndexedColors.RegisterIndexedColor(2, "FFFF0000");
			IndexedColors.RegisterIndexedColor(3, "FF00FF00");
			IndexedColors.RegisterIndexedColor(4, "FF0000FF");
			IndexedColors.RegisterIndexedColor(5, "FFFFFF00");
			IndexedColors.RegisterIndexedColor(6, "FFFF00FF");
			IndexedColors.RegisterIndexedColor(7, "FF00FFFF");
			IndexedColors.RegisterIndexedColor(8, "FF000000");
			IndexedColors.RegisterIndexedColor(9, "FFFFFFFF");
			IndexedColors.RegisterIndexedColor(10, "FFFF0000");
			IndexedColors.RegisterIndexedColor(11, "FF00FF00");
			IndexedColors.RegisterIndexedColor(12, "FF0000FF");
			IndexedColors.RegisterIndexedColor(13, "FFFFFF00");
			IndexedColors.RegisterIndexedColor(14, "FFFF00FF");
			IndexedColors.RegisterIndexedColor(15, "FF00FFFF");
			IndexedColors.RegisterIndexedColor(16, "FF800000");
			IndexedColors.RegisterIndexedColor(17, "FF008000");
			IndexedColors.RegisterIndexedColor(18, "FF000080");
			IndexedColors.RegisterIndexedColor(19, "FF808000");
			IndexedColors.RegisterIndexedColor(20, "FF800080");
			IndexedColors.RegisterIndexedColor(21, "FF008080");
			IndexedColors.RegisterIndexedColor(22, "FFC0C0C0");
			IndexedColors.RegisterIndexedColor(23, "FF808080");
			IndexedColors.RegisterIndexedColor(24, "FF9999FF");
			IndexedColors.RegisterIndexedColor(25, "FF993366");
			IndexedColors.RegisterIndexedColor(26, "FFFFFFCC");
			IndexedColors.RegisterIndexedColor(27, "FFCCFFFF");
			IndexedColors.RegisterIndexedColor(28, "FF660066");
			IndexedColors.RegisterIndexedColor(29, "FFFF8080");
			IndexedColors.RegisterIndexedColor(30, "FF0066CC");
			IndexedColors.RegisterIndexedColor(31, "FFCCCCFF");
			IndexedColors.RegisterIndexedColor(32, "FF000080");
			IndexedColors.RegisterIndexedColor(33, "FFFF00FF");
			IndexedColors.RegisterIndexedColor(34, "FFFFFF00");
			IndexedColors.RegisterIndexedColor(35, "FF00FFFF");
			IndexedColors.RegisterIndexedColor(36, "FF800080");
			IndexedColors.RegisterIndexedColor(37, "FF800000");
			IndexedColors.RegisterIndexedColor(38, "FF008080");
			IndexedColors.RegisterIndexedColor(39, "FF0000FF");
			IndexedColors.RegisterIndexedColor(40, "FF00CCFF");
			IndexedColors.RegisterIndexedColor(41, "FFCCFFFF");
			IndexedColors.RegisterIndexedColor(42, "FFCCFFCC");
			IndexedColors.RegisterIndexedColor(43, "FFFFFF99");
			IndexedColors.RegisterIndexedColor(44, "FF99CCFF");
			IndexedColors.RegisterIndexedColor(45, "FFFF99CC");
			IndexedColors.RegisterIndexedColor(46, "FFCC99FF");
			IndexedColors.RegisterIndexedColor(47, "FFFFCC99");
			IndexedColors.RegisterIndexedColor(48, "FF3366FF");
			IndexedColors.RegisterIndexedColor(49, "FF33CCCC");
			IndexedColors.RegisterIndexedColor(50, "FF99CC00");
			IndexedColors.RegisterIndexedColor(51, "FFFFCC00");
			IndexedColors.RegisterIndexedColor(52, "FFFF9900");
			IndexedColors.RegisterIndexedColor(53, "FFFF6600");
			IndexedColors.RegisterIndexedColor(54, "FF666699");
			IndexedColors.RegisterIndexedColor(55, "FF969696");
			IndexedColors.RegisterIndexedColor(56, "FF003366");
			IndexedColors.RegisterIndexedColor(57, "FF339966");
			IndexedColors.RegisterIndexedColor(58, "FF003300");
			IndexedColors.RegisterIndexedColor(59, "FF333300");
			IndexedColors.RegisterIndexedColor(60, "FF993300");
			IndexedColors.RegisterIndexedColor(61, "FF993366");
			IndexedColors.RegisterIndexedColor(62, "FF333399");
			IndexedColors.RegisterIndexedColor(63, "FF333333");
			IndexedColors.RegisterIndexedColor(64, Colors.Black);
			IndexedColors.RegisterIndexedColor(65, Colors.White);
		}

		public static int PredefinedIndexedColorsCount
		{
			get
			{
				return IndexedColors.indexedColors.Length;
			}
		}

		public static UnsignedIntHex[] PredefinedIndexedColors
		{
			get
			{
				return IndexedColors.indexedColors;
			}
		}

		static void RegisterIndexedColor(int index, string color)
		{
			IndexedColors.indexedColors[index] = new UnsignedIntHex(color);
		}

		static void RegisterIndexedColor(int index, Color color)
		{
			IndexedColors.indexedColors[index] = new UnsignedIntHex(color);
		}

		static readonly UnsignedIntHex[] indexedColors = new UnsignedIntHex[66];
	}
}
