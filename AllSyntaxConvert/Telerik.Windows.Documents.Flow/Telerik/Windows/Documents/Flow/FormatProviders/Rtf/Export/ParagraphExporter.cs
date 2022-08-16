using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class ParagraphExporter
	{
		public ParagraphExporter(RtfDocumentExporter exporter)
		{
			this.documentExporter = exporter;
			this.writer = exporter.Writer;
		}

		ExportContext Context
		{
			get
			{
				return this.documentExporter.Context;
			}
		}

		public static void ExportParagraphProperties(ParagraphProperties paragraphProperties, ExportContext context, RtfWriter writer, bool exportTabStops, bool exportIndentation = true, bool exportListId = false)
		{
			ParagraphExporter.ExportParagraphSpacing(paragraphProperties, writer);
			Alignment value = paragraphProperties.TextAlignment.GetActualValue().Value;
			bool flag = paragraphProperties.FlowDirection.GetActualValue().Value == FlowDirection.RightToLeft;
			if (value != Alignment.Left || flag)
			{
				if (value == Alignment.LowKashida)
				{
					writer.WriteTag("qk", 0);
				}
				else if (value == Alignment.MediumKashida)
				{
					writer.WriteTag("qk", 10);
				}
				else if (value == Alignment.HighKashida)
				{
					writer.WriteTag("qk", 20);
				}
				else
				{
					writer.WriteTag(RtfHelper.AlignmentToRtfTag(value, flag));
				}
			}
			ParagraphBorders actualValue = paragraphProperties.Borders.GetActualValue();
			if (actualValue != null && actualValue != DocumentDefaultStyleSettings.ParagraphBorders)
			{
				BorderExporter.ExportParagraphBorders(actualValue, context, writer);
			}
			if (exportIndentation)
			{
				ParagraphExporter.ExportParagraphIndents(paragraphProperties, flag, writer);
			}
			DocumentTheme theme = paragraphProperties.Document.Theme;
			Color actualValue2 = paragraphProperties.BackgroundColor.GetActualValue().GetActualValue(theme);
			Color actualValue3 = paragraphProperties.ShadingPatternColor.GetActualValue().GetActualValue(theme);
			if (!RtfHelper.IsTransparentColor(actualValue2) || !RtfHelper.IsTransparentColor(actualValue3))
			{
				if (!RtfHelper.IsTransparentColor(actualValue2))
				{
					writer.WriteTag("cbpat", context.ColorTable[actualValue2]);
				}
				if (!RtfHelper.IsTransparentColor(actualValue3))
				{
					writer.WriteTag("cfpat", context.ColorTable[actualValue3]);
				}
				writer.WriteTag("shading", RtfHelper.ShadingPatternToRtfTag(paragraphProperties.ShadingPattern.GetActualValue().Value));
			}
			if (exportListId && paragraphProperties.ListId.HasLocalValue)
			{
				writer.WriteTag("ls", paragraphProperties.ListId.LocalValue.Value);
			}
			if (paragraphProperties.ListLevel.HasLocalValue && paragraphProperties.ListLevel.LocalValue != -1)
			{
				writer.WriteTag("ilvl", paragraphProperties.ListLevel.LocalValue.Value);
			}
			if (exportTabStops)
			{
				ParagraphExporter.ExportTabStops(paragraphProperties.TabStops.GetActualValue(), writer);
			}
		}

		public void ExportParagraph(Paragraph paragraph)
		{
			this.ExportParagraphProperties(paragraph);
			foreach (InlineBase inline in paragraph.Inlines)
			{
				this.documentExporter.InlineExporter.ExportInline(inline);
			}
			TableCell tableCell = paragraph.Parent as TableCell;
			if (tableCell == null || paragraph != tableCell.Blocks.Last<BlockBase>())
			{
				if (!string.IsNullOrEmpty(paragraph.StyleId))
				{
					ParagraphExporter.GetParagraphMarkerPropertiesFromParagraphStyle(paragraph);
				}
				using (this.writer.WriteGroup())
				{
					InlineExporter.ExportCharacterProperties(paragraph.Properties.ParagraphMarkerProperties, this.Context, this.writer);
					this.writer.WriteTag("par");
				}
			}
		}

		static void GetParagraphMarkerPropertiesFromParagraphStyle(Paragraph paragraph)
		{
			Style style = paragraph.Document.StyleRepository.GetStyle(paragraph.StyleId);
			if (style != null)
			{
				foreach (IStyleProperty styleProperty in style.CharacterProperties.StyleProperties)
				{
					IStyleProperty styleProperty2 = paragraph.Properties.ParagraphMarkerProperties.GetStyleProperty(styleProperty.PropertyDefinition);
					if (styleProperty.HasLocalValue && !styleProperty2.HasLocalValue)
					{
						styleProperty2.SetValueAsObject(styleProperty.GetLocalValueAsObject());
					}
				}
			}
		}

		static void ExportParagraphIndents(ParagraphProperties paragraphProperties, bool isRtl, RtfWriter writer)
		{
			double num = paragraphProperties.LeftIndent.GetActualValue().Value;
			double num2 = paragraphProperties.RightIndent.GetActualValue().Value;
			double num3 = paragraphProperties.FirstLineIndent.GetActualValue().Value;
			double value = paragraphProperties.HangingIndent.GetActualValue().Value;
			if (isRtl)
			{
				double num4 = num;
				num = num2;
				num2 = num4;
			}
			if (value > 0.0)
			{
				num3 = -value;
			}
			int parameter = Unit.DipToTwipI(num);
			int parameter2 = Unit.DipToTwipI(num2);
			if (num3 != 0.0)
			{
				writer.WriteTag("fi", Unit.DipToTwipI(num3));
			}
			if (num != 0.0 || isRtl)
			{
				writer.WriteTag("li", parameter);
				writer.WriteTag("lin", parameter);
			}
			if (num2 != 0.0 || isRtl)
			{
				writer.WriteTag("ri", parameter2);
				writer.WriteTag("rin", parameter2);
			}
		}

		static void ExportTabStops(IEnumerable<TabStop> tabStops, RtfWriter writer)
		{
			foreach (TabStop tabStop in tabStops)
			{
				string fromValue = RtfHelper.TabStopLeaderMapper.GetFromValue(tabStop.Leader);
				if (!string.IsNullOrEmpty(fromValue))
				{
					writer.WriteTag(fromValue);
				}
				if (tabStop.Type != TabStopType.Bar)
				{
					string fromValue2 = RtfHelper.TabStopTypeMapper.GetFromValue(tabStop.Type);
					if (!string.IsNullOrEmpty(fromValue2))
					{
						writer.WriteTag(fromValue2);
					}
					writer.WriteTag("tx", Unit.DipToTwipI(tabStop.Position));
				}
				else
				{
					writer.WriteTag("tb", Unit.DipToTwipI(tabStop.Position));
				}
			}
		}

		static void ExportParagraphSpacing(ParagraphProperties paragraphProperties, RtfWriter writer)
		{
			HeightType value = paragraphProperties.LineSpacingType.GetActualValue().Value;
			double value2 = paragraphProperties.LineSpacing.GetActualValue().Value;
			switch (value)
			{
			case HeightType.Auto:
				writer.WriteTag("sl", (int)Math.Round(value2 * RtfHelper.DefaultLineSpacing));
				writer.WriteTag("slmult", 1);
				break;
			case HeightType.AtLeast:
				writer.WriteTag("sl", Unit.DipToTwipI(value2));
				writer.WriteTag("slmult", 0);
				break;
			case HeightType.Exact:
				writer.WriteTag("sl", -Unit.DipToTwipI(value2));
				writer.WriteTag("slmult", 0);
				break;
			}
			double value3 = paragraphProperties.SpacingBefore.GetActualValue().Value;
			if (value3 != 0.0)
			{
				writer.WriteTag("sb", Unit.DipToTwipI(value3));
			}
			if (paragraphProperties.AutomaticSpacingBefore.GetActualValue().Value)
			{
				writer.WriteTag("sbauto", 1);
			}
			double value4 = paragraphProperties.SpacingAfter.GetActualValue().Value;
			if (value4 != 0.0)
			{
				writer.WriteTag("sa", Unit.DipToTwipI(value4));
			}
			if (paragraphProperties.AutomaticSpacingAfter.GetActualValue().Value)
			{
				writer.WriteTag("saauto", 1);
			}
		}

		static int GetParagraphNestingLevel(Paragraph paragraph)
		{
			int nestingLevel = RtfHelper.GetNestingLevel(paragraph);
			return (nestingLevel - 2) / 3;
		}

		void ExportParagraphProperties(Paragraph paragraph)
		{
			this.writer.WriteTag("pard");
			if (this.Context.TableStylesStack.Peek() != null)
			{
				this.writer.WriteTag("yts", this.Context.TableStylesStack.Peek().Value);
			}
			int parameter;
			if (!string.IsNullOrEmpty(paragraph.StyleId) && this.Context.StyleTable.TryGetValue(paragraph.StyleId, out parameter))
			{
				this.writer.WriteTag("s", parameter);
			}
			if (paragraph.Properties.FlowDirection.GetActualValue().Value == FlowDirection.LeftToRight)
			{
				this.writer.WriteTag("ltrpar");
			}
			else
			{
				this.writer.WriteTag("rtlpar");
			}
			bool flag = paragraph.Parent != null && paragraph.Parent is TableCell;
			if (flag)
			{
				this.writer.WriteTag("intbl");
				int paragraphNestingLevel = ParagraphExporter.GetParagraphNestingLevel(paragraph);
				this.writer.WriteTag("itap", paragraphNestingLevel);
			}
			ParagraphExporter.ExportTabStops(paragraph.TabStops, this.writer);
			bool exportIndentation = true;
			if (paragraph.ListId != -1)
			{
				int parameter2 = paragraph.ListId;
				if (this.Context.StyleNumberingListIdsToActualListIds.ContainsKey(paragraph.ListId))
				{
					parameter2 = this.Context.StyleNumberingListIdsToActualListIds[paragraph.ListId];
				}
				this.writer.WriteTag("ls", parameter2);
				if (paragraph.ListLevel != -1)
				{
					this.writer.WriteTag("ilvl", paragraph.ListLevel);
				}
				List list = paragraph.Document.Lists.GetList(paragraph.ListId);
				ListLevel listLevel;
				if (list.MultilevelType == MultilevelType.SingleLevel || paragraph.ListLevel == -1)
				{
					listLevel = list.Levels[0];
				}
				else
				{
					listLevel = list.Levels[paragraph.ListLevel];
				}
				string text = listLevel.NumberTextFormat;
				if (string.IsNullOrEmpty(text))
				{
					text = "\t";
				}
				using (this.writer.WriteGroup("listtext", false))
				{
					if (listLevel.NumberingStyle != NumberingStyle.Bullet)
					{
						text = RtfHelper.ConvertNumberedListFormatToRtfLevelText(text);
					}
					this.writer.WriteText(text);
				}
				ParagraphExporter.ExportParagraphIndents(listLevel.ParagraphProperties, false, this.writer);
				exportIndentation = false;
			}
			ParagraphExporter.ExportParagraphProperties(paragraph.Properties, this.Context, this.writer, false, exportIndentation, false);
		}

		readonly RtfDocumentExporter documentExporter;

		readonly RtfWriter writer;
	}
}
