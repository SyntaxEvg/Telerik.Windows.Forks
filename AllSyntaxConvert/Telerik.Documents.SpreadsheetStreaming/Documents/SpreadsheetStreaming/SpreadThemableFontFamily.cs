using System;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadThemableFontFamily
	{
		public SpreadThemableFontFamily(string fontFamily)
		{
			if (string.IsNullOrEmpty(fontFamily))
			{
				this.themeFontType = new SpreadThemeFontType?(SpreadThemeFontType.Minor);
				return;
			}
			this.localValue = fontFamily;
		}

		public SpreadThemableFontFamily(SpreadThemeFontType themeFontType)
		{
			this.themeFontType = new SpreadThemeFontType?(themeFontType);
		}

		public string LocalValue
		{
			get
			{
				return this.localValue;
			}
		}

		public SpreadThemeFontType ThemeFontType
		{
			get
			{
				return this.themeFontType.Value;
			}
		}

		public bool IsFromTheme
		{
			get
			{
				return this.localValue == null;
			}
		}

		public static bool operator ==(SpreadThemableFontFamily first, SpreadThemableFontFamily second)
		{
			if (first == null)
			{
				return second == null;
			}
			return first.Equals(second);
		}

		public static bool operator !=(SpreadThemableFontFamily first, SpreadThemableFontFamily second)
		{
			return !(first == second);
		}

		public override string ToString()
		{
			if (this.IsFromTheme)
			{
				return this.ThemeFontType.ToString();
			}
			return this.localValue.ToString();
		}

		public override bool Equals(object obj)
		{
			SpreadThemableFontFamily spreadThemableFontFamily = obj as SpreadThemableFontFamily;
			return !(spreadThemableFontFamily == null) && ObjectExtensions.EqualsOfT<string>(this.localValue, spreadThemableFontFamily.localValue) && this.themeFontType == spreadThemableFontFamily.themeFontType;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.localValue.GetHashCodeOrZero(), this.themeFontType.GetHashCodeOrZero());
		}

		internal string GetActualValue(SpreadTheme theme)
		{
			return this.GetActualValue(theme.FontScheme, SpreadFontLanguageType.Latin);
		}

		string GetActualValue(SpreadThemeFontScheme themeFontScheme, SpreadFontLanguageType fontLanguageType = SpreadFontLanguageType.Latin)
		{
			return this.IsFromTheme ? themeFontScheme[this.ThemeFontType][fontLanguageType].FontFamily : this.localValue;
		}

		readonly string localValue;

		readonly SpreadThemeFontType? themeFontType;
	}
}
