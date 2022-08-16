using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class ExportContext
	{
		public ExportContext(RadFlowDocument document)
		{
			this.Document = document;
			this.TableStylesStack = new Stack<int?>();
			this.TableStylesStack.Push(null);
			this.Comments = new Dictionary<Comment, int>();
			this.ListNumberingStyleNamesToListIds = new Dictionary<string, int>();
			this.StyleNumberingListIdsToActualListIds = new Dictionary<int, int>();
		}

		public RadFlowDocument Document { get; set; }

		public Dictionary<Color, int> ColorTable { get; set; }

		public Dictionary<FontFamily, int> FontTable { get; set; }

		public Dictionary<string, int> StyleTable { get; set; }

		public Stack<int?> TableStylesStack { get; set; }

		public Dictionary<Comment, int> Comments { get; set; }

		public Dictionary<string, int> ListNumberingStyleNamesToListIds { get; set; }

		public Dictionary<int, int> StyleNumberingListIdsToActualListIds { get; set; }

		public void InitializeContext()
		{
			this.ConstructColorAndFontTables();
			this.ConstructStyleTable();
		}

		public int GetIdForComment(Comment comment)
		{
			int num;
			if (this.Comments.ContainsKey(comment))
			{
				num = this.Comments[comment];
			}
			else
			{
				num = AnnotationIdGenerator.GetNext();
				this.Comments.Add(comment, num);
			}
			return num;
		}

		static void AddCharacterProperties(CharacterProperties props, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			if (props.FontFamily.HasLocalValue)
			{
				initContext.Fonts.Add(props.FontFamily.LocalValue.GetActualValue(theme));
			}
			if (props.ForegroundColor.HasLocalValue)
			{
				initContext.Colors.Add(props.ForegroundColor.LocalValue.GetActualValue(theme));
			}
			if (props.HighlightColor.HasLocalValue)
			{
				Color? localValue = props.HighlightColor.LocalValue;
				if (localValue != null)
				{
					initContext.Colors.Add(localValue.Value);
				}
			}
			if (props.UnderlineColor.HasLocalValue)
			{
				initContext.Colors.Add(props.UnderlineColor.LocalValue.GetActualValue(theme));
			}
			ExportContext.AddShadingColors(props, theme, initContext);
		}

		static void AddParagraphProperties(ParagraphProperties props, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			ExportContext.AddShadingColors(props, theme, initContext);
			if (props.ParagraphMarkerProperties != null)
			{
				ExportContext.AddCharacterProperties(props.ParagraphMarkerProperties, theme, initContext);
			}
			ParagraphBorders actualValue = props.Borders.GetActualValue();
			if (actualValue != null)
			{
				ExportContext.AddParagraphBorderColors(actualValue, theme, initContext);
			}
		}

		static void AddTablePropsFromStyle(Style style, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			ExportContext.AddShadingColors(style.TableProperties, theme, initContext);
			ExportContext.AddShadingColors(style.TableCellProperties, theme, initContext);
			ParagraphBorders actualValue = style.ParagraphProperties.Borders.GetActualValue();
			if (actualValue != null)
			{
				ExportContext.AddParagraphBorderColors(actualValue, theme, initContext);
			}
			TableCellBorders actualValue2 = style.TableCellProperties.Borders.GetActualValue();
			if (actualValue2 != null)
			{
				ExportContext.AddTableCellBorderColors(actualValue2, theme, initContext);
			}
			TableBorders actualValue3 = style.TableProperties.Borders.GetActualValue();
			if (actualValue3 != null)
			{
				ExportContext.AddTableBorderColors(actualValue3, theme, initContext);
			}
		}

		static void AddShadingColors(IPropertiesWithShading props, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			if (props.BackgroundColor.HasLocalValue)
			{
				initContext.Colors.Add(props.BackgroundColor.LocalValue.GetActualValue(theme));
			}
			if (props.ShadingPatternColor.HasLocalValue)
			{
				initContext.Colors.Add(props.ShadingPatternColor.LocalValue.GetActualValue(theme));
			}
		}

		static void AddTableCellBorderColors(TableCellBorders cellBorders, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			initContext.Colors.Add(cellBorders.Left.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.Top.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.Right.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.Bottom.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.InsideHorizontal.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.InsideVertical.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.DiagonalDown.Color.GetActualValue(theme));
			initContext.Colors.Add(cellBorders.DiagonalUp.Color.GetActualValue(theme));
		}

		static void AddTableBorderColors(TableBorders tableBorders, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			initContext.Colors.Add(tableBorders.Left.Color.GetActualValue(theme));
			initContext.Colors.Add(tableBorders.Top.Color.GetActualValue(theme));
			initContext.Colors.Add(tableBorders.Right.Color.GetActualValue(theme));
			initContext.Colors.Add(tableBorders.Bottom.Color.GetActualValue(theme));
			initContext.Colors.Add(tableBorders.InsideHorizontal.Color.GetActualValue(theme));
			initContext.Colors.Add(tableBorders.InsideVertical.Color.GetActualValue(theme));
		}

		static void AddParagraphBorderColors(ParagraphBorders parBorders, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			initContext.Colors.Add(parBorders.Left.Color.GetActualValue(theme));
			initContext.Colors.Add(parBorders.Top.Color.GetActualValue(theme));
			initContext.Colors.Add(parBorders.Right.Color.GetActualValue(theme));
			initContext.Colors.Add(parBorders.Bottom.Color.GetActualValue(theme));
			initContext.Colors.Add(parBorders.Between.Color.GetActualValue(theme));
		}

		void ConstructStyleTable()
		{
			this.StyleTable = new Dictionary<string, int>();
			int num = 1;
			foreach (Style style in this.Document.StyleRepository.Styles)
			{
				Dictionary<string, int> styleTable = this.StyleTable;
				string id = style.Id;
				int value;
				if (style.StyleType != StyleType.Paragraph || !style.IsDefault)
				{
					num = (value = num) + 1;
				}
				else
				{
					value = 0;
				}
				styleTable[id] = value;
				if (style.LinkedStyleId != null && this.Document.StyleRepository.Contains(style.LinkedStyleId) && !this.StyleTable.ContainsKey(style.LinkedStyleId))
				{
					this.StyleTable[style.LinkedStyleId] = num++;
				}
			}
		}

		void ConstructColorAndFontTables()
		{
			ExportContext.InitTablesContext initTablesContext = new ExportContext.InitTablesContext();
			initTablesContext.Fonts.Add(this.Document.DefaultStyle.CharacterProperties.FontFamily.GetActualValue().GetActualValue(this.Document.Theme));
			initTablesContext.Fonts.Add(Run.FontFamilyPropertyDefinition.DefaultValue.GetActualValue(this.Document.Theme));
			initTablesContext.Colors.Add(Run.ForegroundColorPropertyDefinition.DefaultValue.GetActualValue(this.Document.Theme));
			this.InitTablesForDocument(initTablesContext);
			initTablesContext.Colors.Remove(Colors.Transparent);
			int index = 0;
			this.ColorTable = initTablesContext.Colors.ToDictionary((Color c) => index++);
			index = 0;
			this.FontTable = initTablesContext.Fonts.ToDictionary((FontFamily f) => index++);
		}

		void InitTablesForDocument(ExportContext.InitTablesContext initContext)
		{
			if (this.Document == null)
			{
				return;
			}
			DocumentTheme theme = this.Document.Theme;
			foreach (DocumentElementBase element in this.Document.EnumerateChildrenOfType<DocumentElementBase>())
			{
				this.InitTablesForElement(element, theme, initContext);
			}
			ExportContext.AddCharacterProperties(this.Document.DefaultStyle.CharacterProperties, theme, initContext);
			ExportContext.AddParagraphProperties(this.Document.DefaultStyle.ParagraphProperties, theme, initContext);
			foreach (Style style in this.Document.StyleRepository.Styles)
			{
				switch (style.StyleType)
				{
				case StyleType.Character:
					ExportContext.AddCharacterProperties(style.CharacterProperties, theme, initContext);
					break;
				case StyleType.Paragraph:
					ExportContext.AddCharacterProperties(style.CharacterProperties, theme, initContext);
					ExportContext.AddParagraphProperties(style.ParagraphProperties, theme, initContext);
					break;
				case StyleType.Table:
					ExportContext.AddCharacterProperties(style.CharacterProperties, theme, initContext);
					ExportContext.AddParagraphProperties(style.ParagraphProperties, theme, initContext);
					ExportContext.AddTablePropsFromStyle(style, theme, initContext);
					break;
				}
			}
			foreach (List list in this.Document.Lists)
			{
				foreach (ListLevel listLevel in list.Levels)
				{
					ExportContext.AddCharacterProperties(listLevel.CharacterProperties, theme, initContext);
				}
			}
		}

		void InitTablesForElement(DocumentElementBase element, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			if (element == null)
			{
				return;
			}
			Run run = element as Run;
			if (run != null)
			{
				ExportContext.AddCharacterProperties(run.Properties, theme, initContext);
				return;
			}
			if (element is Paragraph)
			{
				Paragraph paragraph = (Paragraph)element;
				ExportContext.AddParagraphProperties(paragraph.Properties, theme, initContext);
				return;
			}
			if (element is TableCell)
			{
				TableCell tableCell = (TableCell)element;
				ExportContext.AddShadingColors(tableCell.Properties, theme, initContext);
				ExportContext.AddTableCellBorderColors(tableCell.Borders, theme, initContext);
				return;
			}
			if (element is Table)
			{
				Table table = (Table)element;
				ExportContext.AddShadingColors(table.Properties, theme, initContext);
				ExportContext.AddTableBorderColors(table.Borders, theme, initContext);
				return;
			}
			if (element is Section)
			{
				Section section = (Section)element;
				this.InitTablesForHeaderFooter(section.Headers.First, theme, initContext);
				this.InitTablesForHeaderFooter(section.Headers.Default, theme, initContext);
				this.InitTablesForHeaderFooter(section.Headers.Even, theme, initContext);
				this.InitTablesForHeaderFooter(section.Footers.First, theme, initContext);
				this.InitTablesForHeaderFooter(section.Footers.Default, theme, initContext);
				this.InitTablesForHeaderFooter(section.Footers.Even, theme, initContext);
			}
		}

		void InitTablesForHeaderFooter(HeaderFooterBase headerFooter, DocumentTheme theme, ExportContext.InitTablesContext initContext)
		{
			if (headerFooter == null)
			{
				return;
			}
			foreach (DocumentElementBase element in headerFooter.EnumerateChildrenOfType<DocumentElementBase>())
			{
				this.InitTablesForElement(element, theme, initContext);
			}
		}

		class InitTablesContext
		{
			public InitTablesContext()
			{
				this.Colors = new HashSet<Color>();
				this.Fonts = new HashSet<FontFamily>();
			}

			public HashSet<Color> Colors { get; set; }

			public HashSet<FontFamily> Fonts { get; set; }
		}
	}
}
