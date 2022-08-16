using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Themes
{
	class SpreadThemeColor
	{
		public SpreadThemeColor(SpreadColor color, SpreadThemeColorType themeColorType)
		{
			this.color = color;
			this.themeColorType = themeColorType;
		}

		public SpreadColor Color
		{
			get
			{
				return this.color;
			}
		}

		public override bool Equals(object obj)
		{
			SpreadThemeColor spreadThemeColor = obj as SpreadThemeColor;
			return spreadThemeColor != null && this.color == spreadThemeColor.color && this.themeColorType == spreadThemeColor.themeColorType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.color.GetHashCode(), this.themeColorType.GetHashCode());
		}

		readonly SpreadThemeColorType themeColorType;

		readonly SpreadColor color;
	}
}
