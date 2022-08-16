using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model
{
	class FontProperties
	{
		internal FontProperties()
		{
		}

		internal FontProperties(int fontSize, SpreadThemableColor foreColor, SpreadThemableFontFamily fontFamily)
		{
			this.FontSize = new double?((double)fontSize);
			this.ForeColor = foreColor;
			this.FontFamily = fontFamily;
		}

		public double? FontSize { get; set; }

		public SpreadThemableColor ForeColor { get; set; }

		public bool? IsBold { get; set; }

		public bool? IsItalic { get; set; }

		public SpreadUnderlineType? Underline { get; set; }

		public SpreadThemableFontFamily FontFamily { get; set; }

		public bool HasSetProperties
		{
			get
			{
				return this.FontSize != null || this.ForeColor != null || this.IsBold != null || this.IsItalic != null || this.Underline != null || this.VerticalAlignment != null || this.Strike != null || this.FontFamily != null;
			}
		}

		public string VerticalAlignment { get; set; }

		public bool? Strike { get; set; }

		public static FontProperties FromCellFormat(SpreadCellFormatBase cellFormat)
		{
			return new FontProperties
			{
				FontSize = cellFormat.FontSize,
				ForeColor = cellFormat.ForeColor,
				IsBold = cellFormat.IsBold,
				IsItalic = cellFormat.IsItalic,
				Underline = cellFormat.Underline,
				FontFamily = cellFormat.FontFamily
			};
		}

		public override bool Equals(object obj)
		{
			FontProperties fontProperties = obj as FontProperties;
			return fontProperties != null && (ObjectExtensions.EqualsOfT<double?>(this.FontSize, fontProperties.FontSize) && ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.ForeColor, fontProperties.ForeColor) && ObjectExtensions.EqualsOfT<bool?>(this.IsBold, fontProperties.IsBold) && ObjectExtensions.EqualsOfT<bool?>(this.IsItalic, fontProperties.IsItalic) && ObjectExtensions.EqualsOfT<SpreadUnderlineType?>(this.Underline, fontProperties.Underline) && ObjectExtensions.EqualsOfT<string>(this.VerticalAlignment, fontProperties.VerticalAlignment) && ObjectExtensions.EqualsOfT<bool?>(this.Strike, fontProperties.Strike)) && ObjectExtensions.EqualsOfT<SpreadThemableFontFamily>(this.FontFamily, fontProperties.FontFamily);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.FontSize.GetHashCodeOrZero(), this.ForeColor.GetHashCodeOrZero(), this.IsBold.GetHashCodeOrZero(), this.IsItalic.GetHashCodeOrZero(), this.Underline.GetHashCodeOrZero(), this.VerticalAlignment.GetHashCodeOrZero(), this.Strike.GetHashCodeOrZero(), this.FontFamily.GetHashCodeOrZero());
		}
	}
}
