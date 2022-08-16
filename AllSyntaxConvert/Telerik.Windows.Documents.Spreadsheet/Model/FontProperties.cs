using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public struct FontProperties : IFontProperties
	{
		public FontFamily FontFamily { get; internal set; }

		public double FontSize { get; internal set; }

		public bool IsBold { get; internal set; }

		public bool IsItalic { get; internal set; }

		public UnderlineType Underline { get; internal set; }

		public ThemableColor ForeColor { get; internal set; }

		public FontWeight FontWeight
		{
			get
			{
				if (!this.IsBold)
				{
					return FontWeights.Normal;
				}
				return FontWeights.Bold;
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				if (!this.IsItalic)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		public bool IsMonospaced
		{
			get
			{
				return this.isMonospaced || SystemFontsManager.IsMonospaced(this.FontFamily.Source);
			}
			set
			{
				this.isMonospaced = value;
			}
		}

		public FontProperties(FontFamily fontFamily, double fontSize, bool isBold)
		{
			this = default(FontProperties);
			this.FontFamily = fontFamily;
			this.FontSize = fontSize;
			this.IsBold = isBold;
		}

		public override bool Equals(object obj)
		{
			return obj is FontProperties && this == (FontProperties)obj;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.FontFamily.GetHashCode(), this.FontSize.GetHashCode(), this.IsBold.GetHashCode(), this.IsItalic.GetHashCode(), this.Underline.GetHashCode(), this.ForeColor.GetHashCode());
		}

		public static bool operator ==(FontProperties left, FontProperties right)
		{
			return left.FontFamily.GetSourceOrNull() == right.FontFamily.GetSourceOrNull() && left.FontSize == right.FontSize && left.IsBold == right.IsBold && left.IsItalic == right.IsItalic && left.Underline == right.Underline && left.ForeColor == right.ForeColor;
		}

		public static bool operator !=(FontProperties left, FontProperties right)
		{
			return !(left == right);
		}

		bool isMonospaced;
	}
}
