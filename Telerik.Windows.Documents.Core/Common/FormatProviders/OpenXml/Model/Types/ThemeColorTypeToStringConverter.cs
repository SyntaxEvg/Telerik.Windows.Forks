using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class ThemeColorTypeToStringConverter : IStringConverter<ThemeColorType>
	{
		public ThemeColorType ConvertFromString(string value)
		{
			switch (value)
			{
			case "tx1":
				return ThemeColorType.Text1;
			case "tx2":
				return ThemeColorType.Text2;
			case "bg1":
				return ThemeColorType.Background1;
			case "bg2":
				return ThemeColorType.Background2;
			case "accent1":
				return ThemeColorType.Accent1;
			case "accent2":
				return ThemeColorType.Accent2;
			case "accent3":
				return ThemeColorType.Accent3;
			case "accent4":
				return ThemeColorType.Accent4;
			case "accent5":
				return ThemeColorType.Accent5;
			case "accent6":
				return ThemeColorType.Accent6;
			case "folHlink":
				return ThemeColorType.FollowedHyperlink;
			case "hlink":
				return ThemeColorType.Hyperlink;
			}
			throw new NotImplementedException();
		}

		public string ConvertToString(ThemeColorType value)
		{
			switch (value)
			{
			case ThemeColorType.Background1:
				return "bg1";
			case ThemeColorType.Text1:
				return "tx1";
			case ThemeColorType.Background2:
				return "bg2";
			case ThemeColorType.Text2:
				return "tx2";
			case ThemeColorType.Accent1:
				return "accent1";
			case ThemeColorType.Accent2:
				return "accent2";
			case ThemeColorType.Accent3:
				return "accent3";
			case ThemeColorType.Accent4:
				return "accent4";
			case ThemeColorType.Accent5:
				return "accent5";
			case ThemeColorType.Accent6:
				return "accent6";
			case ThemeColorType.Hyperlink:
				return "hlink";
			case ThemeColorType.FollowedHyperlink:
				return "folHlink";
			default:
				throw new NotSupportedException();
			}
		}
	}
}
