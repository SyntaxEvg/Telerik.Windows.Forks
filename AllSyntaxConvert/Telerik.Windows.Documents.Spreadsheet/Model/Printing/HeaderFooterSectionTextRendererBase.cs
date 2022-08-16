using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	abstract class HeaderFooterSectionTextRendererBase : IHeaderFooterSectionTextParseHandler
	{
		protected HeaderFooterSectionTextRendererBase()
		{
			this.parser = new HeaderFooterSectionTextParser(this);
		}

		protected FontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		protected FontWeight FontWeight
		{
			get
			{
				return this.fontWeight;
			}
		}

		protected FontStyle FontStyle
		{
			get
			{
				return this.fontStyle;
			}
		}

		protected double FontSize
		{
			get
			{
				return UnitHelper.PointToDip((double)this.fontSize);
			}
		}

		protected bool IsStrikeThrough
		{
			get
			{
				return this.isStrikeThrough;
			}
		}

		protected UnderlineType UnderlineType
		{
			get
			{
				return this.underlineType;
			}
		}

		protected ThemableColor FontColor
		{
			get
			{
				return this.fontColor;
			}
		}

		protected Color FontColorActualValue
		{
			get
			{
				return this.fontColor.GetActualValue(this.SheetContext.Workbook.Theme);
			}
		}

		protected BaselineAlignment BaselineAlignment
		{
			get
			{
				return this.baselineAlignment;
			}
		}

		public static string GetNormalFontFamily(Workbook workbook)
		{
			return workbook.Styles["Normal"].FontFamily.GetActualValue(workbook.Theme).ToString();
		}

		protected abstract Sheet SheetContext { get; }

		protected void Render(string headerFooterSectionText)
		{
			this.SetInitialStates();
			this.parser.Parse(headerFooterSectionText);
		}

		protected Run CreateRun(string text)
		{
			Run run = new Run();
			run.FontFamily = this.FontFamily;
			run.FontStyle = this.FontStyle;
			run.FontWeight = this.FontWeight;
			run.FontSize = this.FontSize;
			this.SetTextDecorations(run);
			run.BaselineAlignment = this.BaselineAlignment;
			run.Foreground = new SolidColorBrush(this.FontColorActualValue);
			run.Text = text;
			return run;
		}

		protected abstract void OnAppendingAmpersandOverride();

		protected abstract void OnAppendingFileNameOverride();

		protected abstract void OnAppendingPageNumberOverride();

		protected abstract void OnAppendingAddToPageNumberOverride();

		protected abstract void OnAppendingSubstractFromPageNumberOverride();

		protected abstract void OnAppendingNumberOfPagesOverride();

		protected abstract void OnAppendingSheetNameOverride();

		protected abstract void OnAppendingFilePathOverride();

		protected abstract void OnAppendingPictureOverride();

		protected abstract void OnAppendingDateOverride();

		protected abstract void OnAppendingTimeOverride();

		protected abstract void OnAppendingTextFragmentOverride(string text);

		void IHeaderFooterSectionTextParseHandler.OnAmpersandParse()
		{
			this.OnAppendingAmpersandOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnFileNameParse()
		{
			this.OnAppendingFileNameOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnPageNumberParse()
		{
			this.OnAppendingPageNumberOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnAddToPageNumberParse()
		{
			this.OnAppendingAddToPageNumberOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnSubstractFromPageNumberParse()
		{
			this.OnAppendingSubstractFromPageNumberOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnNumberOfPagesParse()
		{
			this.OnAppendingNumberOfPagesOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnTextFragmentParse(string text)
		{
			this.OnAppendingTextFragmentOverride(text);
		}

		void IHeaderFooterSectionTextParseHandler.OnSheetNameParse()
		{
			this.OnAppendingSheetNameOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnDateParse()
		{
			this.OnAppendingDateOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnTimeParse()
		{
			this.OnAppendingTimeOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnFilePathParse()
		{
			this.OnAppendingFilePathOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnPictureParse()
		{
			this.OnAppendingPictureOverride();
		}

		void IHeaderFooterSectionTextParseHandler.OnOutlineStyleParse()
		{
		}

		void IHeaderFooterSectionTextParseHandler.OnShadowStyleParse()
		{
		}

		void IHeaderFooterSectionTextParseHandler.OnDoubleUnderlineParse()
		{
			if (this.underlineType == UnderlineType.Double)
			{
				this.underlineType = UnderlineType.None;
				return;
			}
			this.underlineType = UnderlineType.Double;
		}

		void IHeaderFooterSectionTextParseHandler.OnBoldParse()
		{
			if (this.fontWeight == FontWeights.Bold)
			{
				this.fontWeight = FontWeights.Normal;
				return;
			}
			this.fontWeight = FontWeights.Bold;
		}

		void IHeaderFooterSectionTextParseHandler.OnItalicParse()
		{
			if (this.fontStyle == FontStyles.Italic)
			{
				this.fontStyle = FontStyles.Normal;
				return;
			}
			this.fontStyle = FontStyles.Italic;
		}

		void IHeaderFooterSectionTextParseHandler.OnUnderlineParse()
		{
			if (this.underlineType == UnderlineType.Single)
			{
				this.underlineType = UnderlineType.None;
				return;
			}
			this.underlineType = UnderlineType.Single;
		}

		void IHeaderFooterSectionTextParseHandler.OnFontSizeParse(int fontSize)
		{
			this.fontSize = fontSize;
		}

		void IHeaderFooterSectionTextParseHandler.OnStrikeThroughParse()
		{
			this.isStrikeThrough = !this.isStrikeThrough;
		}

		void IHeaderFooterSectionTextParseHandler.OnSubscriptParse()
		{
			if (this.baselineAlignment == BaselineAlignment.Subscript)
			{
				this.baselineAlignment = BaselineAlignment.Baseline;
				return;
			}
			this.baselineAlignment = BaselineAlignment.Subscript;
		}

		void IHeaderFooterSectionTextParseHandler.OnSuperscriptParse()
		{
			if (this.baselineAlignment == BaselineAlignment.Superscript)
			{
				this.baselineAlignment = BaselineAlignment.Baseline;
				return;
			}
			this.baselineAlignment = BaselineAlignment.Superscript;
		}

		void IHeaderFooterSectionTextParseHandler.OnFontPropertiesParse(string fontPropertiesValue)
		{
			string[] array = fontPropertiesValue.Split(new char[] { ',' });
			bool flag = array.Length == 2;
			if (flag)
			{
				string text = array[0].Trim();
				string text2 = array[1];
				this.fontFamily = new FontFamily((text == '-'.ToString()) ? HeaderFooterSectionTextRendererBase.GetNormalFontFamily(this.SheetContext.Workbook) : text);
				this.fontWeight = (text2.Contains("Bold") ? FontWeights.Bold : FontWeights.Normal);
				this.fontStyle = (text2.Contains("Italic") ? FontStyles.Italic : FontStyles.Normal);
			}
		}

		void IHeaderFooterSectionTextParseHandler.OnColorParse(string colorValue)
		{
			char symbol = colorValue[2];
			if (HeaderFooterSectionTextRendererBase.IsSign(symbol))
			{
				this.ParseThemeColor(colorValue);
				return;
			}
			this.ParseHexColor(colorValue);
		}

		void ParseThemeColor(string colorValue)
		{
			string s = colorValue.Substring(0, 2);
			string s2 = colorValue.Substring(3);
			int num;
			int num2;
			if (int.TryParse(s, out num) && int.TryParse(s2, out num2) && num < HeaderFooterSectionTextRendererBase.ThemeColorTypesCount && (double)Math.Abs(num2) <= 100.0)
			{
				char c = colorValue[2];
				double num3 = (double)((c == '+') ? 1 : (-1)) / 100.0;
				ThemeColorType themeColorType = (ThemeColorType)num;
				this.fontColor = new ThemableColor(themeColorType, (double)num2 * num3);
			}
		}

		void ParseHexColor(string colorValue)
		{
			string s = colorValue.Substring(0, 2);
			string s2 = colorValue.Substring(2, 2);
			string s3 = colorValue.Substring(4);
			byte r;
			byte g;
			byte b;
			if (byte.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out r) && byte.TryParse(s2, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g) && byte.TryParse(s3, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
			{
				this.fontColor = new ThemableColor(Color.FromArgb(byte.MaxValue, r, g, b));
			}
		}

		static bool IsSign(char symbol)
		{
			return symbol == '+' || symbol == '-';
		}

		void SetInitialStates()
		{
			this.fontFamily = new FontFamily(HeaderFooterSectionTextRendererBase.GetNormalFontFamily(this.SheetContext.Workbook));
			this.fontWeight = FontWeights.Normal;
			this.fontStyle = FontStyles.Normal;
			this.isStrikeThrough = false;
			this.underlineType = UnderlineType.None;
			this.baselineAlignment = BaselineAlignment.Baseline;
			this.fontSize = 11;
			this.fontColor = HeaderFooterSectionTextRendererBase.DefaultFontColor;
		}

		void SetTextDecorations(Run run)
		{
			run.TextDecorations = this.GetTextDecorations();
		}

		TextDecorationCollection GetTextDecorations()
		{
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			if (this.UnderlineType != UnderlineType.None)
			{
				textDecorationCollection.Add(TextDecorations.Underline);
			}
			if (this.IsStrikeThrough)
			{
				textDecorationCollection.Add(TextDecorations.Strikethrough);
			}
			return textDecorationCollection;
		}

		public const string Bold = "Bold";

		public const string Italic = "Italic";

		public const string Regular = "Regular";

		public const int DefaultFontSize = 11;

		const double TintAndShadeMultiplier = 100.0;

		const int TintAndShadeSignPositionIndex = 2;

		static readonly int ThemeColorTypesCount = Enum.GetValues(typeof(ThemeColorType)).Length;

		static readonly ThemableColor DefaultFontColor = new ThemableColor(Colors.Black, true);

		readonly HeaderFooterSectionTextParser parser;

		FontFamily fontFamily;

		FontWeight fontWeight;

		FontStyle fontStyle;

		int fontSize;

		ThemableColor fontColor;

		UnderlineType underlineType;

		BaselineAlignment baselineAlignment;

		bool isStrikeThrough;
	}
}
