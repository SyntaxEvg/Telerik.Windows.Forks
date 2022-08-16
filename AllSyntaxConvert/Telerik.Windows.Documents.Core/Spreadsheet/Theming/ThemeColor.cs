using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Theming
{
	public class ThemeColor
	{
		public ThemeColor(Color color, ThemeColorType themeColorType)
		{
			this.color = color;
			this.themeColorType = themeColorType;
		}

		public ThemeColorType ThemeColorType
		{
			get
			{
				return this.themeColorType;
			}
		}

		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		public ThemeColor Clone()
		{
			return new ThemeColor(this.Color, this.ThemeColorType);
		}

		public override bool Equals(object obj)
		{
			ThemeColor themeColor = obj as ThemeColor;
			return themeColor != null && this.color == themeColor.color && this.themeColorType == themeColor.themeColorType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.color.GetHashCode(), this.themeColorType.GetHashCode());
		}

		readonly ThemeColorType themeColorType;

		readonly Color color;
	}
}
