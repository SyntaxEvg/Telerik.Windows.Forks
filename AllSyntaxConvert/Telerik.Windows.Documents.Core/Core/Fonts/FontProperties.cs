using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Core.Fonts
{
	public class FontProperties
	{
		public FontProperties(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight)
		{
			this.fontFamily = fontFamily;
			this.fontStyle = fontStyle;
			this.fontWeight = fontWeight;
			this.isMonoSpaced = SystemFontsManager.IsMonospaced(fontFamily.Source);
		}

		public FontProperties(FontFamily fontFamily)
			: this(fontFamily, FontStyles.Normal, FontWeights.Normal)
		{
		}

		public string FontFamilyName
		{
			get
			{
				return this.FontFamily.Source;
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return this.fontWeight;
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				return this.fontStyle;
			}
		}

		public FontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		public bool IsMonoSpaced
		{
			get
			{
				return this.isMonoSpaced;
			}
		}

		public override bool Equals(object obj)
		{
			FontProperties fontProperties = obj as FontProperties;
			return fontProperties != null && (this.FontFamilyName == fontProperties.FontFamilyName && this.FontStyle == fontProperties.FontStyle) && this.FontWeight == fontProperties.FontWeight;
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.fontFamily.GetHashCode();
			num = num * 23 + this.fontStyle.GetHashCode();
			return num * 23 + this.fontWeight.GetHashCode();
		}

		readonly FontFamily fontFamily;

		readonly FontStyle fontStyle;

		readonly FontWeight fontWeight;

		readonly bool isMonoSpaced;
	}
}
