using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadCellStyleCollection : IEnumerable<SpreadCellStyle>, IEnumerable
	{
		static SpreadCellStyleCollection()
		{
			SpreadCellStyleCollection.RegisterStyleBuiltinId(SpreadCellStyleCollection.DefaultStyleName, 0);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Comma", 3);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Currency", 4);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Percent", 5);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Comma [0]", 6);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Currency [0]", 7);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Hyperlink", 8);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Followed Hyperlink", 9);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Note", 10);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Warning Text", 11);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Title", 15);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Heading 1", 16);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Heading 2", 17);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Heading 3", 18);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Heading 4", 19);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Input", 20);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Output", 21);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Calculation", 22);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Check Cell", 23);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Linked Cell", 24);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Total", 25);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Good", 26);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Bad", 27);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Neutral", 28);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent1", 29);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent1", 30);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent1", 31);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent1", 32);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent2", 33);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent2", 34);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent2", 35);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent2", 36);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent3", 37);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent3", 38);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent3", 39);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent3", 40);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent4", 41);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent4", 42);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent4", 43);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent4", 44);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent5", 45);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent5", 46);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent5", 47);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent5", 48);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Accent6", 49);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("20% - Accent6", 50);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("40% - Accent6", 51);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("60% - Accent6", 52);
			SpreadCellStyleCollection.RegisterStyleBuiltinId("Explanatory Text", 53);
		}

		internal SpreadCellStyleCollection()
		{
			this.nameToStyles = new Dictionary<string, SpreadCellStyle>();
			this.InitDefaultStyles();
		}

		public int Count
		{
			get
			{
				return this.nameToStyles.Keys.Count;
			}
		}

		public SpreadCellStyle this[string styleName]
		{
			get
			{
				return this.GetByName(styleName);
			}
		}

		public bool Contains(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			return this.nameToStyles.ContainsKey(styleName);
		}

		public bool Contains(SpreadCellStyle style)
		{
			Guard.ThrowExceptionIfNull<SpreadCellStyle>(style, "style");
			return this.Contains(style.Name);
		}

		public SpreadCellStyle GetByName(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			SpreadCellStyle result = null;
			this.nameToStyles.TryGetValue(styleName, out result);
			return result;
		}

		public SpreadCellStyle Add(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			if (this.Contains(styleName))
			{
				throw new InvalidOperationException(string.Format("Style with name '{0}' already exists", styleName));
			}
			SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle(styleName);
			this.AddInternal(spreadCellStyle);
			return spreadCellStyle;
		}

		public IEnumerator<SpreadCellStyle> GetEnumerator()
		{
			return this.nameToStyles.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpreadCellStyle>)this).GetEnumerator();
		}

		internal void AddInternal(SpreadCellStyle style)
		{
			this.nameToStyles.Add(style.Name, style);
		}

		static int? GetStyleBuiltinId(string styleName)
		{
			int value;
			if (!SpreadCellStyleCollection.styleToBuiltinId.TryGetValue(styleName, out value))
			{
				return null;
			}
			return new int?(value);
		}

		static void RegisterStyleBuiltinId(string styleName, int builtinId)
		{
			SpreadCellStyleCollection.styleToBuiltinId.Add(styleName, builtinId);
		}

		static SpreadCellStyle CreateCellStyle(string styleName)
		{
			int? styleBuiltinId = SpreadCellStyleCollection.GetStyleBuiltinId(styleName);
			return new SpreadCellStyle(styleName, styleBuiltinId);
		}

		void InitDefaultStyles()
		{
			this.AddGoodBadAndNeutralStyles();
			this.AddDataAndModelStyles();
			this.AddTitlesAndHeadingsStyles();
			this.AddThemedStyles();
			this.AddNumberFormatStyles();
		}

		void AddGoodBadAndNeutralStyles()
		{
			SpreadCellStyle style = SpreadCellStyleCollection.CreateCellStyle(SpreadCellStyleCollection.DefaultStyleName);
			this.AddInternal(style);
			SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle("Bad");
			spreadCellStyle.ForeColor = SpreadThemableColor.FromRgb(156, 0, 6);
			spreadCellStyle.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(byte.MaxValue, 199, 206));
			this.AddInternal(spreadCellStyle);
			SpreadCellStyle spreadCellStyle2 = SpreadCellStyleCollection.CreateCellStyle("Good");
			spreadCellStyle2.ForeColor = SpreadThemableColor.FromRgb(0, 97, 0);
			spreadCellStyle2.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(198, 239, 206));
			this.AddInternal(spreadCellStyle2);
			SpreadCellStyle spreadCellStyle3 = SpreadCellStyleCollection.CreateCellStyle("Neutral");
			spreadCellStyle3.ForeColor = SpreadThemableColor.FromRgb(156, 101, 0);
			spreadCellStyle3.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(byte.MaxValue, 235, 156));
			this.AddInternal(spreadCellStyle3);
		}

		void AddDataAndModelStyles()
		{
			SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle("Calculation");
			spreadCellStyle.ForeColor = SpreadThemableColor.FromRgb(250, 125, 0);
			SpreadBorder spreadBorder = new SpreadBorder(SpreadBorderStyle.Thin, SpreadThemableColor.FromRgb(127, 127, 127));
			spreadCellStyle.LeftBorder = spreadBorder;
			spreadCellStyle.TopBorder = spreadBorder;
			spreadCellStyle.RightBorder = spreadBorder;
			spreadCellStyle.BottomBorder = spreadBorder;
			spreadCellStyle.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(242, 242, 242));
			this.AddInternal(spreadCellStyle);
			SpreadCellStyle spreadCellStyle2 = SpreadCellStyleCollection.CreateCellStyle("Check Cell");
			spreadCellStyle2.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Light1);
			SpreadBorder spreadBorder2 = new SpreadBorder(SpreadBorderStyle.Double, SpreadThemableColor.FromRgb(127, 127, 127));
			spreadCellStyle2.LeftBorder = spreadBorder2;
			spreadCellStyle2.TopBorder = spreadBorder2;
			spreadCellStyle2.RightBorder = spreadBorder2;
			spreadCellStyle2.BottomBorder = spreadBorder2;
			spreadCellStyle2.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(165, 165, 165));
			this.AddInternal(spreadCellStyle2);
			SpreadCellStyle spreadCellStyle3 = SpreadCellStyleCollection.CreateCellStyle("Explanatory Text");
			spreadCellStyle3.ForeColor = SpreadThemableColor.FromRgb(127, 127, 127);
			spreadCellStyle3.IsItalic = new bool?(true);
			this.AddInternal(spreadCellStyle3);
			SpreadCellStyle spreadCellStyle4 = SpreadCellStyleCollection.CreateCellStyle("Followed Hyperlink");
			spreadCellStyle4.FontFamily = new SpreadThemableFontFamily(SpreadThemeFontType.Minor);
			spreadCellStyle4.Underline = new SpreadUnderlineType?(SpreadUnderlineType.Single);
			spreadCellStyle4.ForeColor = new SpreadThemableColor(SpreadThemeColorType.FollowedHyperlink);
			this.AddInternal(spreadCellStyle4);
			SpreadCellStyle spreadCellStyle5 = SpreadCellStyleCollection.CreateCellStyle("Hyperlink");
			spreadCellStyle5.FontFamily = new SpreadThemableFontFamily(SpreadThemeFontType.Minor);
			spreadCellStyle5.Underline = new SpreadUnderlineType?(SpreadUnderlineType.Single);
			spreadCellStyle5.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Hyperlink);
			this.AddInternal(spreadCellStyle5);
			SpreadCellStyle spreadCellStyle6 = SpreadCellStyleCollection.CreateCellStyle("Input");
			spreadCellStyle6.ForeColor = SpreadThemableColor.FromRgb(63, 63, 118);
			SpreadBorder spreadBorder3 = new SpreadBorder(SpreadBorderStyle.Thin, new SpreadThemableColor(new SpreadColor(127, 127, 127)));
			spreadCellStyle6.LeftBorder = spreadBorder3;
			spreadCellStyle6.TopBorder = spreadBorder3;
			spreadCellStyle6.RightBorder = spreadBorder3;
			spreadCellStyle6.BottomBorder = spreadBorder3;
			spreadCellStyle6.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(byte.MaxValue, 204, 153));
			this.AddInternal(spreadCellStyle6);
			SpreadCellStyle spreadCellStyle7 = SpreadCellStyleCollection.CreateCellStyle("Linked Cell");
			spreadCellStyle7.ForeColor = SpreadThemableColor.FromRgb(250, 125, 0);
			SpreadBorder bottomBorder = new SpreadBorder(SpreadBorderStyle.Double, new SpreadThemableColor(new SpreadColor(byte.MaxValue, 128, 1)));
			spreadCellStyle7.BottomBorder = bottomBorder;
			this.AddInternal(spreadCellStyle7);
			SpreadCellStyle spreadCellStyle8 = SpreadCellStyleCollection.CreateCellStyle("Note");
			SpreadBorder spreadBorder4 = new SpreadBorder(SpreadBorderStyle.Thin, new SpreadThemableColor(new SpreadColor(178, 178, 178)));
			spreadCellStyle8.LeftBorder = spreadBorder4;
			spreadCellStyle8.TopBorder = spreadBorder4;
			spreadCellStyle8.RightBorder = spreadBorder4;
			spreadCellStyle8.BottomBorder = spreadBorder4;
			spreadCellStyle8.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(byte.MaxValue, byte.MaxValue, 204));
			this.AddInternal(spreadCellStyle8);
			SpreadCellStyle spreadCellStyle9 = SpreadCellStyleCollection.CreateCellStyle("Output");
			spreadCellStyle9.ForeColor = SpreadThemableColor.FromRgb(63, 63, 63);
			spreadCellStyle9.IsBold = new bool?(true);
			SpreadBorder spreadBorder5 = new SpreadBorder(SpreadBorderStyle.Thin, SpreadThemableColor.FromRgb(63, 63, 63));
			spreadCellStyle9.LeftBorder = spreadBorder5;
			spreadCellStyle9.TopBorder = spreadBorder5;
			spreadCellStyle9.RightBorder = spreadBorder5;
			spreadCellStyle9.BottomBorder = spreadBorder5;
			spreadCellStyle9.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(242, 242, 242));
			this.AddInternal(spreadCellStyle9);
			SpreadCellStyle spreadCellStyle10 = SpreadCellStyleCollection.CreateCellStyle("Warning Text");
			spreadCellStyle10.ForeColor = SpreadThemableColor.FromRgb(byte.MaxValue, 0, 0);
			this.AddInternal(spreadCellStyle10);
		}

		void AddTitlesAndHeadingsStyles()
		{
			SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle("Heading 1");
			spreadCellStyle.IsBold = new bool?(true);
			spreadCellStyle.FontSize = new double?(15.0);
			spreadCellStyle.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark2);
			SpreadBorder bottomBorder = new SpreadBorder(SpreadBorderStyle.Thick, new SpreadThemableColor(SpreadThemeColorType.Accent1));
			spreadCellStyle.BottomBorder = bottomBorder;
			this.AddInternal(spreadCellStyle);
			SpreadCellStyle spreadCellStyle2 = SpreadCellStyleCollection.CreateCellStyle("Heading 2");
			spreadCellStyle2.IsBold = new bool?(true);
			spreadCellStyle2.FontSize = new double?(13.0);
			spreadCellStyle2.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark2);
			SpreadBorder bottomBorder2 = new SpreadBorder(SpreadBorderStyle.Thick, new SpreadThemableColor(SpreadThemeColorType.Accent1, new SpreadColorShadeType?(SpreadColorShadeType.Shade2)));
			spreadCellStyle2.BottomBorder = bottomBorder2;
			this.AddInternal(spreadCellStyle2);
			SpreadCellStyle spreadCellStyle3 = SpreadCellStyleCollection.CreateCellStyle("Heading 3");
			spreadCellStyle3.IsBold = new bool?(true);
			spreadCellStyle3.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark2);
			SpreadBorder bottomBorder3 = new SpreadBorder(SpreadBorderStyle.Medium, new SpreadThemableColor(SpreadThemeColorType.Accent1, new SpreadColorShadeType?(SpreadColorShadeType.Shade2)));
			spreadCellStyle3.BottomBorder = bottomBorder3;
			this.AddInternal(spreadCellStyle3);
			SpreadCellStyle spreadCellStyle4 = SpreadCellStyleCollection.CreateCellStyle("Heading 4");
			spreadCellStyle4.IsBold = new bool?(true);
			spreadCellStyle4.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark2);
			this.AddInternal(spreadCellStyle4);
			SpreadCellStyle spreadCellStyle5 = SpreadCellStyleCollection.CreateCellStyle("Title");
			spreadCellStyle5.FontFamily = new SpreadThemableFontFamily(SpreadThemeFontType.Major);
			spreadCellStyle5.FontSize = new double?(18.0);
			spreadCellStyle5.IsBold = new bool?(true);
			spreadCellStyle5.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark2);
			this.AddInternal(spreadCellStyle5);
			SpreadCellStyle spreadCellStyle6 = SpreadCellStyleCollection.CreateCellStyle("Total");
			spreadCellStyle6.IsBold = new bool?(true);
			spreadCellStyle6.ForeColor = new SpreadThemableColor(SpreadThemeColorType.Dark1);
			SpreadBorder topBorder = new SpreadBorder(SpreadBorderStyle.Thin, new SpreadThemableColor(SpreadThemeColorType.Accent1, new SpreadColorShadeType?(SpreadColorShadeType.Shade3)));
			spreadCellStyle6.TopBorder = topBorder;
			SpreadBorder bottomBorder4 = new SpreadBorder(SpreadBorderStyle.Double, new SpreadThemableColor(SpreadThemeColorType.Accent1, new SpreadColorShadeType?(SpreadColorShadeType.Shade3)));
			spreadCellStyle6.BottomBorder = bottomBorder4;
			this.AddInternal(spreadCellStyle6);
		}

		void AddThemedStyles()
		{
			double num = 0.8;
			for (int i = 0; i < 4; i++)
			{
				SpreadThemableColor foreColor = new SpreadThemableColor((i < 2) ? SpreadThemeColorType.Dark1 : SpreadThemeColorType.Light1);
				for (int j = 4; j <= 9; j++)
				{
					SpreadThemeColorType spreadThemeColorType = (SpreadThemeColorType)j;
					string text = spreadThemeColorType.ToString();
					if (num != 0.0)
					{
						text = string.Format("{0}% - ", (1.0 - num) * 100.0) + text;
					}
					SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle(text);
					spreadCellStyle.ForeColor = foreColor;
					if (num == 0.0)
					{
						spreadCellStyle.Fill = SpreadPatternFill.CreateSolidFill(new SpreadThemableColor(spreadThemeColorType, null));
					}
					else
					{
						spreadCellStyle.Fill = SpreadPatternFill.CreateSolidFill(new SpreadThemableColor(spreadThemeColorType, num));
					}
					this.AddInternal(spreadCellStyle);
				}
				if (i < 2)
				{
					num -= 0.2;
				}
				else
				{
					num = 0.0;
				}
			}
		}

		void AddNumberFormatStyles()
		{
			SpreadCellStyle spreadCellStyle = SpreadCellStyleCollection.CreateCellStyle("Comma");
			spreadCellStyle.NumberFormat = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
			this.AddInternal(spreadCellStyle);
			SpreadCellStyle spreadCellStyle2 = SpreadCellStyleCollection.CreateCellStyle("Comma [0]");
			spreadCellStyle2.NumberFormat = "_(* #,##0_);_(* (#,##0);_(* \"-\"_);_(@_)";
			this.AddInternal(spreadCellStyle2);
			SpreadCellStyle spreadCellStyle3 = SpreadCellStyleCollection.CreateCellStyle("Currency");
			spreadCellStyle3.NumberFormat = "_(\\$* #,##0.00_);_(\\$* (#,##0.00);_(\\$* \"-\"??_);_(@_)";
			this.AddInternal(spreadCellStyle3);
			SpreadCellStyle spreadCellStyle4 = SpreadCellStyleCollection.CreateCellStyle("Currency [0]");
			spreadCellStyle4.NumberFormat = "_(\\$* #,##0_);_(\\$* (#,##0);_(\\$* \"-\"_);_(@_)";
			this.AddInternal(spreadCellStyle4);
			SpreadCellStyle spreadCellStyle5 = SpreadCellStyleCollection.CreateCellStyle("Percent");
			spreadCellStyle5.NumberFormat = "0%";
			this.AddInternal(spreadCellStyle5);
		}

		internal static readonly string DefaultStyleName = "Normal";

		static readonly Dictionary<string, int> styleToBuiltinId = new Dictionary<string, int>();

		readonly Dictionary<string, SpreadCellStyle> nameToStyles;
	}
}
