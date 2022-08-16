using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class CellStyleCollection : IEnumerable<CellStyle>, IEnumerable
	{
		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public int Count
		{
			get
			{
				return this.nameToStyles.Keys.Count;
			}
		}

		public CellStyle this[string styleName]
		{
			get
			{
				return this.GetByName(styleName);
			}
		}

		static CellStyleCollection()
		{
			CellStyleCollection.RegisterStyleBuiltinId(SpreadsheetDefaultValues.DefaultStyleName, 0);
			CellStyleCollection.RegisterStyleBuiltinId("Comma", 3);
			CellStyleCollection.RegisterStyleBuiltinId("Currency", 4);
			CellStyleCollection.RegisterStyleBuiltinId("Percent", 5);
			CellStyleCollection.RegisterStyleBuiltinId("Comma [0]", 6);
			CellStyleCollection.RegisterStyleBuiltinId("Currency [0]", 7);
			CellStyleCollection.RegisterStyleBuiltinId(SpreadsheetDefaultValues.HyperlinkStyleName, 8);
			CellStyleCollection.RegisterStyleBuiltinId("Followed Hyperlink", 9);
			CellStyleCollection.RegisterStyleBuiltinId("Note", 10);
			CellStyleCollection.RegisterStyleBuiltinId("Warning Text", 11);
			CellStyleCollection.RegisterStyleBuiltinId("Title", 15);
			CellStyleCollection.RegisterStyleBuiltinId("Heading 1", 16);
			CellStyleCollection.RegisterStyleBuiltinId("Heading 2", 17);
			CellStyleCollection.RegisterStyleBuiltinId("Heading 3", 18);
			CellStyleCollection.RegisterStyleBuiltinId("Heading 4", 19);
			CellStyleCollection.RegisterStyleBuiltinId("Input", 20);
			CellStyleCollection.RegisterStyleBuiltinId("Output", 21);
			CellStyleCollection.RegisterStyleBuiltinId("Calculation", 22);
			CellStyleCollection.RegisterStyleBuiltinId("Check Cell", 23);
			CellStyleCollection.RegisterStyleBuiltinId("Linked Cell", 24);
			CellStyleCollection.RegisterStyleBuiltinId("Total", 25);
			CellStyleCollection.RegisterStyleBuiltinId("Good", 26);
			CellStyleCollection.RegisterStyleBuiltinId("Bad", 27);
			CellStyleCollection.RegisterStyleBuiltinId("Neutral", 28);
			CellStyleCollection.RegisterStyleBuiltinId("Accent1", 29);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent1", 30);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent1", 31);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent1", 32);
			CellStyleCollection.RegisterStyleBuiltinId("Accent2", 33);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent2", 34);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent2", 35);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent2", 36);
			CellStyleCollection.RegisterStyleBuiltinId("Accent3", 37);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent3", 38);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent3", 39);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent3", 40);
			CellStyleCollection.RegisterStyleBuiltinId("Accent4", 41);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent4", 42);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent4", 43);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent4", 44);
			CellStyleCollection.RegisterStyleBuiltinId("Accent5", 45);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent5", 46);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent5", 47);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent5", 48);
			CellStyleCollection.RegisterStyleBuiltinId("Accent6", 49);
			CellStyleCollection.RegisterStyleBuiltinId("20% - Accent6", 50);
			CellStyleCollection.RegisterStyleBuiltinId("40% - Accent6", 51);
			CellStyleCollection.RegisterStyleBuiltinId("60% - Accent6", 52);
			CellStyleCollection.RegisterStyleBuiltinId("Explanatory Text", 53);
		}

		internal CellStyleCollection(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "cells");
			this.workbook = workbook;
			this.nameToStyles = new Dictionary<string, CellStyle>();
			this.InitDefaultStyles();
		}

		static void RegisterStyleBuiltinId(string styleName, int builtinId)
		{
			CellStyleCollection.StyleToBuiltinId.Add(styleName, builtinId);
		}

		void InitDefaultStyles()
		{
			this.AddGoodBadAndNeutralStyles();
			this.AddDataAndModelStyles();
			this.AddTitlesAndHeadingsStyles();
			this.AddThemedStyles();
			this.AddNumberFormatStyles();
		}

		static int? GetStyleBuiltinId(string styleName)
		{
			int value;
			if (!CellStyleCollection.StyleToBuiltinId.TryGetValue(styleName, out value))
			{
				return null;
			}
			return new int?(value);
		}

		CellStyle CreateCellStyle(Workbook workbook, string styleName, CellStyleCategory category, bool isRemoveable = true)
		{
			int? styleBuiltinId = CellStyleCollection.GetStyleBuiltinId(styleName);
			return new CellStyle(workbook, styleName, category, isRemoveable, styleBuiltinId);
		}

		void AddGoodBadAndNeutralStyles()
		{
			CellStyle cellStyle = this.CreateCellStyle(this.Workbook, SpreadsheetDefaultValues.DefaultStyleName, CellStyleCategory.GoodBadAndNeutral, false);
			cellStyle.IncludeAlignment = true;
			cellStyle.IncludeBorder = true;
			cellStyle.IncludeFill = true;
			cellStyle.IncludeFont = true;
			cellStyle.IncludeNumber = true;
			cellStyle.IncludeProtection = true;
			this.AddInternal(cellStyle);
			CellStyle cellStyle2 = this.CreateCellStyle(this.Workbook, "Bad", CellStyleCategory.GoodBadAndNeutral, true);
			cellStyle2.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 156, 0, 6);
			cellStyle2.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, byte.MaxValue, 199, 206));
			cellStyle2.IncludeFont = true;
			cellStyle2.IncludeFill = true;
			this.AddInternal(cellStyle2);
			CellStyle cellStyle3 = this.CreateCellStyle(this.Workbook, "Good", CellStyleCategory.GoodBadAndNeutral, true);
			cellStyle3.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 0, 97, 0);
			cellStyle3.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, 198, 239, 206));
			cellStyle3.IncludeFont = true;
			cellStyle3.IncludeFill = true;
			this.AddInternal(cellStyle3);
			CellStyle cellStyle4 = this.CreateCellStyle(this.Workbook, "Neutral", CellStyleCategory.GoodBadAndNeutral, true);
			cellStyle4.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 156, 101, 0);
			cellStyle4.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, byte.MaxValue, 235, 156));
			cellStyle4.IncludeFont = true;
			cellStyle4.IncludeFill = true;
			this.AddInternal(cellStyle4);
		}

		void AddDataAndModelStyles()
		{
			CellStyle cellStyle = this.CreateCellStyle(this.Workbook, "Calculation", CellStyleCategory.DataAndModel, true);
			cellStyle.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 250, 125, 0);
			CellBorder cellBorder = new CellBorder(CellBorderStyle.Thin, new ThemableColor(Color.FromArgb(byte.MaxValue, 127, 127, 127)));
			cellStyle.LeftBorder = cellBorder;
			cellStyle.TopBorder = cellBorder;
			cellStyle.RightBorder = cellBorder;
			cellStyle.BottomBorder = cellBorder;
			cellStyle.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, 242, 242, 242));
			cellStyle.IncludeFont = true;
			cellStyle.IncludeBorder = true;
			cellStyle.IncludeFill = true;
			this.AddInternal(cellStyle);
			CellStyle cellStyle2 = this.CreateCellStyle(this.Workbook, "Check Cell", CellStyleCategory.DataAndModel, true);
			cellStyle2.ForeColor = new ThemableColor(ThemeColorType.Background1, null);
			CellBorder cellBorder2 = new CellBorder(CellBorderStyle.Thick, new ThemableColor(Color.FromArgb(byte.MaxValue, 127, 127, 127)));
			cellStyle2.LeftBorder = cellBorder2;
			cellStyle2.TopBorder = cellBorder2;
			cellStyle2.RightBorder = cellBorder2;
			cellStyle2.BottomBorder = cellBorder2;
			cellStyle2.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, 165, 165, 165));
			cellStyle2.IncludeFont = true;
			cellStyle2.IncludeBorder = true;
			cellStyle2.IncludeFill = true;
			this.AddInternal(cellStyle2);
			CellStyle cellStyle3 = this.CreateCellStyle(this.Workbook, "Explanatory Text", CellStyleCategory.DataAndModel, true);
			cellStyle3.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 127, 127, 127);
			cellStyle3.IsItalic = true;
			cellStyle3.IncludeFont = true;
			this.AddInternal(cellStyle3);
			CellStyle cellStyle4 = this.CreateCellStyle(this.Workbook, "Followed Hyperlink", CellStyleCategory.DataAndModel, true);
			cellStyle4.FontFamily = new ThemableFontFamily(ThemeFontType.Minor);
			cellStyle4.Underline = UnderlineType.Single;
			cellStyle4.ForeColor = new ThemableColor(ThemeColorType.FollowedHyperlink, null);
			cellStyle4.IncludeFont = true;
			this.AddInternal(cellStyle4);
			CellStyle cellStyle5 = this.CreateCellStyle(this.Workbook, SpreadsheetDefaultValues.HyperlinkStyleName, CellStyleCategory.DataAndModel, true);
			cellStyle5.FontFamily = new ThemableFontFamily(ThemeFontType.Minor);
			cellStyle5.Underline = UnderlineType.Single;
			cellStyle5.ForeColor = new ThemableColor(ThemeColorType.Hyperlink, null);
			cellStyle5.IncludeFont = true;
			this.AddInternal(cellStyle5);
			CellStyle cellStyle6 = this.CreateCellStyle(this.Workbook, "Input", CellStyleCategory.DataAndModel, true);
			cellStyle6.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 63, 63, 118);
			CellBorder cellBorder3 = new CellBorder(CellBorderStyle.Thin, new ThemableColor(Color.FromArgb(byte.MaxValue, 127, 127, 127)));
			cellStyle6.LeftBorder = cellBorder3;
			cellStyle6.TopBorder = cellBorder3;
			cellStyle6.RightBorder = cellBorder3;
			cellStyle6.BottomBorder = cellBorder3;
			cellStyle6.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, byte.MaxValue, 204, 153));
			cellStyle6.IncludeFont = true;
			cellStyle6.IncludeBorder = true;
			cellStyle6.IncludeFill = true;
			this.AddInternal(cellStyle6);
			CellStyle cellStyle7 = this.CreateCellStyle(this.Workbook, "Linked Cell", CellStyleCategory.DataAndModel, true);
			cellStyle7.ForeColor = ThemableColor.FromArgb(byte.MaxValue, 250, 125, 0);
			CellBorder bottomBorder = new CellBorder(CellBorderStyle.Thick, new ThemableColor(Color.FromArgb(byte.MaxValue, byte.MaxValue, 128, 1)));
			cellStyle7.BottomBorder = bottomBorder;
			cellStyle7.IncludeFont = true;
			cellStyle7.IncludeBorder = true;
			this.AddInternal(cellStyle7);
			CellStyle cellStyle8 = this.CreateCellStyle(this.Workbook, "Note", CellStyleCategory.DataAndModel, true);
			CellBorder cellBorder4 = new CellBorder(CellBorderStyle.Thick, new ThemableColor(Color.FromArgb(byte.MaxValue, 178, 178, 178)));
			cellStyle8.LeftBorder = cellBorder4;
			cellStyle8.TopBorder = cellBorder4;
			cellStyle8.RightBorder = cellBorder4;
			cellStyle8.BottomBorder = cellBorder4;
			cellStyle8.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 204));
			cellStyle8.IncludeBorder = true;
			cellStyle8.IncludeFill = true;
			this.AddInternal(cellStyle8);
			CellStyle cellStyle9 = this.CreateCellStyle(this.Workbook, "Output", CellStyleCategory.DataAndModel, true);
			cellStyle9.ForeColor = (ThemableColor)Color.FromArgb(byte.MaxValue, 63, 63, 63);
			cellStyle9.IsBold = true;
			CellBorder cellBorder5 = new CellBorder(CellBorderStyle.Thin, new ThemableColor(Color.FromArgb(byte.MaxValue, 63, 63, 63)));
			cellStyle9.LeftBorder = cellBorder5;
			cellStyle9.TopBorder = cellBorder5;
			cellStyle9.RightBorder = cellBorder5;
			cellStyle9.BottomBorder = cellBorder5;
			cellStyle9.Fill = PatternFill.CreateSolidFill(Color.FromArgb(byte.MaxValue, 242, 242, 242));
			cellStyle9.IncludeFont = true;
			cellStyle9.IncludeBorder = true;
			cellStyle9.IncludeFill = true;
			this.AddInternal(cellStyle9);
			CellStyle cellStyle10 = this.CreateCellStyle(this.Workbook, "Warning Text", CellStyleCategory.DataAndModel, true);
			cellStyle10.ForeColor = (ThemableColor)Colors.Red;
			cellStyle10.IncludeFont = true;
			this.AddInternal(cellStyle10);
		}

		void AddTitlesAndHeadingsStyles()
		{
			CellStyle cellStyle = this.CreateCellStyle(this.Workbook, "Heading 1", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle.IsBold = true;
			cellStyle.FontSize = UnitHelper.PointToDip(15.0);
			cellStyle.ForeColor = new ThemableColor(ThemeColorType.Text2, null);
			CellBorder bottomBorder = new CellBorder(CellBorderStyle.Thick, new ThemableColor(ThemeColorType.Accent1, null));
			cellStyle.BottomBorder = bottomBorder;
			cellStyle.IncludeFont = true;
			cellStyle.IncludeBorder = true;
			this.AddInternal(cellStyle);
			CellStyle cellStyle2 = this.CreateCellStyle(this.Workbook, "Heading 2", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle2.IsBold = true;
			cellStyle2.FontSize = UnitHelper.PointToDip(13.0);
			cellStyle2.ForeColor = new ThemableColor(ThemeColorType.Text2, null);
			CellBorder bottomBorder2 = new CellBorder(CellBorderStyle.Thick, new ThemableColor(ThemeColorType.Accent1, new ColorShadeType?(ColorShadeType.Shade2)));
			cellStyle2.BottomBorder = bottomBorder2;
			cellStyle2.IncludeFont = true;
			cellStyle2.IncludeBorder = true;
			this.AddInternal(cellStyle2);
			CellStyle cellStyle3 = this.CreateCellStyle(this.Workbook, "Heading 3", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle3.IsBold = true;
			cellStyle3.ForeColor = new ThemableColor(ThemeColorType.Text2, null);
			CellBorder bottomBorder3 = new CellBorder(CellBorderStyle.Medium, new ThemableColor(ThemeColorType.Accent1, new ColorShadeType?(ColorShadeType.Shade2)));
			cellStyle3.BottomBorder = bottomBorder3;
			cellStyle3.IncludeFont = true;
			cellStyle3.IncludeBorder = true;
			this.AddInternal(cellStyle3);
			CellStyle cellStyle4 = this.CreateCellStyle(this.Workbook, "Heading 4", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle4.IsBold = true;
			cellStyle4.ForeColor = new ThemableColor(ThemeColorType.Text2, null);
			cellStyle4.IncludeFont = true;
			this.AddInternal(cellStyle4);
			CellStyle cellStyle5 = this.CreateCellStyle(this.Workbook, "Title", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle5.FontFamily = new ThemableFontFamily(ThemeFontType.Major);
			cellStyle5.FontSize = UnitHelper.PointToDip(18.0);
			cellStyle5.IsBold = true;
			cellStyle5.ForeColor = new ThemableColor(ThemeColorType.Text2, null);
			cellStyle5.IncludeFont = true;
			this.AddInternal(cellStyle5);
			CellStyle cellStyle6 = this.CreateCellStyle(this.Workbook, "Total", CellStyleCategory.TitlesAndHeadings, true);
			cellStyle6.IsBold = true;
			cellStyle6.ForeColor = new ThemableColor(ThemeColorType.Text1, null);
			CellBorder topBorder = new CellBorder(CellBorderStyle.Thin, new ThemableColor(ThemeColorType.Accent1, new ColorShadeType?(ColorShadeType.Shade3)));
			cellStyle6.TopBorder = topBorder;
			CellBorder bottomBorder4 = new CellBorder(CellBorderStyle.Thick, new ThemableColor(ThemeColorType.Accent1, new ColorShadeType?(ColorShadeType.Shade3)));
			cellStyle6.BottomBorder = bottomBorder4;
			cellStyle6.IncludeFont = true;
			cellStyle6.IncludeBorder = true;
			this.AddInternal(cellStyle6);
		}

		void AddThemedStyles()
		{
			double num = 0.8;
			for (int i = 0; i < 4; i++)
			{
				ThemableColor foreColor = new ThemableColor((i < 2) ? ThemeColorType.Text1 : ThemeColorType.Background1, null);
				for (int j = 4; j <= 9; j++)
				{
					ThemeColorType themeColorType = (ThemeColorType)j;
					string text = themeColorType.ToString();
					if (num != 0.0)
					{
						text = string.Format("{0}% - ", (1.0 - num) * 100.0) + text;
					}
					CellStyle cellStyle = this.CreateCellStyle(this.Workbook, text, CellStyleCategory.ThemedCellStyles, true);
					cellStyle.ForeColor = foreColor;
					cellStyle.Fill = PatternFill.CreateSolidFill(new ThemableColor(themeColorType, num));
					cellStyle.IncludeFont = true;
					cellStyle.IncludeFill = true;
					this.AddInternal(cellStyle);
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
			CellStyle cellStyle = this.CreateCellStyle(this.Workbook, "Comma", CellStyleCategory.NumberFormat, true);
			cellStyle.Format = new CellValueFormat("_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)");
			cellStyle.IncludeNumber = true;
			this.AddInternal(cellStyle);
			CellStyle cellStyle2 = this.CreateCellStyle(this.Workbook, "Comma [0]", CellStyleCategory.NumberFormat, true);
			cellStyle2.Format = new CellValueFormat("_(* #,##0_);_(* (#,##0);_(* \"-\"_);_(@_)");
			cellStyle2.IncludeNumber = true;
			this.AddInternal(cellStyle2);
			CellStyle cellStyle3 = this.CreateCellStyle(this.Workbook, "Currency", CellStyleCategory.NumberFormat, true);
			cellStyle3.Format = new CellValueFormat("_($* #,##0.00_);_($* (#,##0.00);_($* \"-\"??_);_(@_)");
			cellStyle3.IncludeNumber = true;
			this.AddInternal(cellStyle3);
			CellStyle cellStyle4 = this.CreateCellStyle(this.Workbook, "Currency [0]", CellStyleCategory.NumberFormat, true);
			cellStyle4.Format = new CellValueFormat("_($* #,##0_);_($* (#,##0);_($* \"-\"_);_(@_)");
			cellStyle4.IncludeNumber = true;
			this.AddInternal(cellStyle4);
			CellStyle cellStyle5 = this.CreateCellStyle(this.Workbook, "Percent", CellStyleCategory.NumberFormat, true);
			cellStyle5.Format = new CellValueFormat("0%");
			cellStyle5.IncludeNumber = true;
			this.AddInternal(cellStyle5);
		}

		public bool Contains(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			return this.nameToStyles.ContainsKey(styleName);
		}

		public bool Contains(CellStyle style)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(style, "style");
			return style.Workbook == this.Workbook && this.Contains(style.Name);
		}

		public CellStyle GetByName(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			CellStyle result = null;
			this.nameToStyles.TryGetValue(styleName, out result);
			return result;
		}

		public CellStyle Add(string styleName, CellStyleCategory category = CellStyleCategory.Custom, bool isRemovable = true)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			if (this.Contains(styleName))
			{
				throw new InvalidOperationException(string.Format("Style with name '{0}' already exists", styleName));
			}
			CellStyle cellStyle = this.CreateCellStyle(this.workbook, styleName, category, isRemovable);
			AddRemoveStyleCommandContext context = new AddRemoveStyleCommandContext(this.Workbook, cellStyle);
			this.Workbook.ExecuteCommand<AddRemoveStyleCommandContext>(WorkbookCommands.AddStyle, context);
			return cellStyle;
		}

		public bool Remove(string styleName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleName, "styleName");
			CellStyle byName = this.GetByName(styleName);
			return byName != null && this.Remove(byName);
		}

		public bool Remove(CellStyle style)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(style, "style");
			AddRemoveStyleCommandContext context = new AddRemoveStyleCommandContext(this.Workbook, style);
			return this.Workbook.ExecuteCommand<AddRemoveStyleCommandContext>(WorkbookCommands.RemoveStyle, context);
		}

		internal void AddInternal(CellStyle style)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(style, "style");
			Guard.ThrowExceptionIfNotEqual<Workbook>(this.Workbook, style.Workbook, "style.Workbook");
			this.nameToStyles.Add(style.Name, style);
			this.OnStyleAdded(style);
		}

		internal void RemoveInternal(CellStyle style)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(style, "style");
			Guard.ThrowExceptionIfNotEqual<Workbook>(this.Workbook, style.Workbook, "style.Workbook");
			this.nameToStyles.Remove(style.Name);
			this.OnStyleRemoved(style);
		}

		void OnStyleAdded(CellStyle style)
		{
			this.OnChanged();
			this.OnStyleChanged(style, CellStyle.GetAllProperties());
			style.Changed += this.Style_Changed;
		}

		void OnStyleRemoved(CellStyle style)
		{
			style.Changed -= this.Style_Changed;
			this.OnChanged();
			this.OnStyleChanged(style, CellStyle.GetAllProperties());
		}

		void Style_Changed(object sender, StyleChangedEventArgs e)
		{
			this.OnStyleChanged((CellStyle)sender, e.ChangedProperties);
		}

		void OnStyleChanged(CellStyle style, IEnumerable<IPropertyDefinition> changedProperties)
		{
			using (new UpdateScope(new Action(this.workbook.SuspendLayoutUpdate), new Action(this.workbook.ResumeLayoutUpdate)))
			{
				foreach (Worksheet worksheet in this.workbook.Worksheets)
				{
					worksheet.Cells.PropertyBag.OnStyleChanged(style, changedProperties);
				}
			}
		}

		public IEnumerator<CellStyle> GetEnumerator()
		{
			return this.nameToStyles.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CellStyle>)this).GetEnumerator();
		}

		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		static readonly Dictionary<string, int> StyleToBuiltinId = new Dictionary<string, int>();

		readonly Workbook workbook;

		readonly Dictionary<string, CellStyle> nameToStyles;
	}
}
