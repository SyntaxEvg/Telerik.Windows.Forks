using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	struct FontInfo
	{
		public FontInfo(CellStyle cellStyle)
		{
			this = default(FontInfo);
			this.Bold = new bool?(cellStyle.IsBold);
			this.Italic = new bool?(cellStyle.IsItalic);
			this.FontSize = new double?(cellStyle.FontSize);
			this.FontFamily = cellStyle.FontFamily;
			this.ForeColor = cellStyle.ForeColor;
			this.UnderlineType = new UnderlineType?(cellStyle.Underline);
		}

		public bool? Bold { get; set; }

		public bool? Italic { get; set; }

		public double? FontSize { get; set; }

		public ThemableFontFamily FontFamily { get; set; }

		public ThemableColor ForeColor { get; set; }

		public UnderlineType? UnderlineType { get; set; }

		public FontInfo MergeWith(FontInfo other)
		{
			if (this.Bold == null)
			{
				this.Bold = other.Bold;
			}
			if (this.Italic == null)
			{
				this.Italic = other.Italic;
			}
			if (this.FontSize == null)
			{
				this.FontSize = other.FontSize;
			}
			if (this.FontFamily == null)
			{
				this.FontFamily = other.FontFamily;
			}
			if (this.ForeColor == null)
			{
				this.ForeColor = other.ForeColor;
			}
			if (this.UnderlineType == null)
			{
				this.UnderlineType = other.UnderlineType;
			}
			return this;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is FontInfo))
			{
				return false;
			}
			FontInfo fontInfo = (FontInfo)obj;
			return TelerikHelper.EqualsOfT<bool?>(this.Bold, fontInfo.Bold) && TelerikHelper.EqualsOfT<bool?>(this.Italic, fontInfo.Italic) && TelerikHelper.EqualsOfT<double?>(this.FontSize, fontInfo.FontSize) && TelerikHelper.EqualsOfT<ThemableFontFamily>(this.FontFamily, fontInfo.FontFamily) && TelerikHelper.EqualsOfT<ThemableColor>(this.ForeColor, fontInfo.ForeColor) && TelerikHelper.EqualsOfT<UnderlineType?>(this.UnderlineType, fontInfo.UnderlineType);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Bold.GetHashCodeOrZero(), this.Italic.GetHashCodeOrZero(), this.FontFamily.GetHashCodeOrZero(), this.FontSize.GetHashCodeOrZero(), this.ForeColor.GetHashCodeOrZero(), this.UnderlineType.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			return string.Format("Bold: {0};Italic: {1}; FontSize: {2}; FontFamily: {3}; ForeColor: {4}; Underline: {5}", new object[] { this.Bold, this.Italic, this.FontSize, this.FontFamily, this.ForeColor, this.UnderlineType });
		}

		internal static FontInfo GetDefaultValue()
		{
			return new FontInfo
			{
				Bold = new bool?(false),
				FontFamily = SpreadsheetDefaultValues.DefaultFontFamily,
				FontSize = new double?(SpreadsheetDefaultValues.DefaultFontSize),
				ForeColor = SpreadsheetDefaultValues.DefaultForeColor,
				Italic = new bool?(false),
				UnderlineType = new UnderlineType?(Telerik.Windows.Documents.Spreadsheet.Model.UnderlineType.None)
			};
		}
	}
}
