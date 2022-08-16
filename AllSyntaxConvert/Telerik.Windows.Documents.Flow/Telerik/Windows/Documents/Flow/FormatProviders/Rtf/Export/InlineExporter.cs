using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class InlineExporter
	{
		public InlineExporter(RtfDocumentExporter exporter)
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

		public static void ExportCharacterProperties(CharacterProperties spanProps, ExportContext context, RtfWriter writer)
		{
			InlineExporter.ExportFlowDirection(spanProps.FlowDirection.GetActualValue(), writer);
			DocumentTheme theme = spanProps.Document.Theme;
			InlineExporter.ExportFontFamily(spanProps.FontFamily.GetActualValue(), context, theme, writer);
			InlineExporter.ExportFontSize(spanProps.FontSize.GetActualValue(), writer);
			InlineExporter.ExportFontStyle(spanProps.FontStyle.GetActualValue(), writer);
			InlineExporter.ExportFontWeight(spanProps.FontWeight.GetActualValue(), writer);
			InlineExporter.ExportStrikethrough(spanProps.Strikethrough.GetActualValue(), writer);
			InlineExporter.ExportBaselineAlignment(spanProps.BaselineAlignment.GetActualValue(), writer);
			InlineExporter.ExportColors(spanProps, theme, context, writer);
			writer.WriteTag(RtfHelper.UndelineDecorationToRtfTag(spanProps.UnderlinePattern.GetActualValue().Value));
		}

		public void ExportInline(InlineBase inline)
		{
			switch (inline.Type)
			{
			case DocumentElementType.Run:
				this.ExportSpan((Run)inline);
				return;
			case DocumentElementType.Header:
			case DocumentElementType.Footer:
			case DocumentElementType.Comment:
				break;
			case DocumentElementType.FieldCharacter:
				this.ExportFieldCharacter((FieldCharacter)inline);
				return;
			case DocumentElementType.ImageInline:
				this.documentExporter.ImageExporter.ExportImage(((ImageInline)inline).Image);
				return;
			case DocumentElementType.FloatingImage:
				this.documentExporter.ImageExporter.ExportFloatingImage((FloatingImage)inline);
				return;
			case DocumentElementType.Break:
				this.ExportBreak((Break)inline);
				break;
			case DocumentElementType.BookmarkRangeStart:
				this.ExportBookmarkRangeStart((BookmarkRangeStart)inline);
				return;
			case DocumentElementType.BookmarkRangeEnd:
				this.ExportBookmarkRangeEnd((BookmarkRangeEnd)inline);
				return;
			case DocumentElementType.CommentRangeStart:
				this.ExportCommentRangeStart((CommentRangeStart)inline);
				return;
			case DocumentElementType.CommentRangeEnd:
				this.ExportCommentRangeEnd((CommentRangeEnd)inline);
				return;
			default:
				return;
			}
		}

		static void ExportColors(CharacterProperties spanProps, DocumentTheme docTheme, ExportContext context, RtfWriter writer)
		{
			Color actualValue = spanProps.ForegroundColor.GetActualValue().GetActualValue(docTheme);
			if (actualValue != Run.ForegroundColorPropertyDefinition.DefaultValue.LocalValue && !RtfHelper.IsTransparentColor(actualValue))
			{
				writer.WriteTag("cf", context.ColorTable[actualValue]);
			}
			Color value = spanProps.HighlightColor.GetActualValue().Value;
			if (value != Run.HighlightColorPropertyDefinition.DefaultValue.Value && !RtfHelper.IsTransparentColor(value))
			{
				writer.WriteTag("cb", context.ColorTable[value]);
				writer.WriteTag("highlight", context.ColorTable[value]);
			}
			Color actualValue2 = spanProps.BackgroundColor.GetActualValue().GetActualValue(docTheme);
			Color actualValue3 = spanProps.ShadingPatternColor.GetActualValue().GetActualValue(docTheme);
			if (!RtfHelper.IsTransparentColor(actualValue2) || !RtfHelper.IsTransparentColor(actualValue3))
			{
				if (!RtfHelper.IsTransparentColor(actualValue2))
				{
					writer.WriteTag("chcbpat", context.ColorTable[actualValue2]);
				}
				if (!RtfHelper.IsTransparentColor(actualValue3))
				{
					writer.WriteTag("chcfpat", context.ColorTable[actualValue3]);
				}
				writer.WriteTag("chshdng", RtfHelper.ShadingPatternToRtfTag(spanProps.ShadingPattern.GetActualValue().Value));
			}
			ThemableColor actualValue4 = spanProps.UnderlineColor.GetActualValue();
			Color actualValue5 = actualValue4.GetActualValue(docTheme);
			if (!actualValue4.IsAutomatic && !RtfHelper.IsTransparentColor(actualValue5))
			{
				writer.WriteTag("ulc", context.ColorTable[actualValue5]);
			}
		}

		static void ExportBaselineAlignment(Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment? baseline, RtfWriter writer)
		{
			if (baseline != Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline)
			{
				if (baseline == Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Subscript)
				{
					writer.WriteTag("sub");
				}
				if (baseline == Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Superscript)
				{
					writer.WriteTag("super");
				}
			}
		}

		static void ExportStrikethrough(bool? strikethrough, RtfWriter writer)
		{
			if (strikethrough != null)
			{
				if (strikethrough.Value)
				{
					writer.WriteTag("strike");
					return;
				}
				writer.WriteTag("strike", 0);
			}
		}

		static void ExportFontWeight(FontWeight? fontWeight, RtfWriter writer)
		{
			if (fontWeight != FontWeights.Normal)
			{
				writer.WriteTag("b");
				return;
			}
			writer.WriteTag("b", 0);
		}

		static void ExportFontStyle(FontStyle? fontStyle, RtfWriter writer)
		{
			if (fontStyle != FontStyles.Normal)
			{
				writer.WriteTag("i");
				return;
			}
			writer.WriteTag("i", 0);
		}

		static void ExportFontSize(double? fontSize, RtfWriter writer)
		{
			if (fontSize != null)
			{
				writer.WriteTag("fs", Unit.DipToPointI(fontSize.Value * 2.0));
			}
		}

		static void ExportFontFamily(ThemableFontFamily actualFont, ExportContext context, DocumentTheme docTheme, RtfWriter writer)
		{
			if (actualFont != null)
			{
				int num = context.FontTable[actualFont.GetActualValue(docTheme)];
				if (num != 0)
				{
					writer.WriteTag("f", num);
				}
			}
		}

		static void ExportFlowDirection(Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection? flowDirection, RtfWriter writer)
		{
			if (flowDirection == Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.LeftToRight)
			{
				writer.WriteTag("ltrch");
				return;
			}
			writer.WriteTag("rtlch");
		}

		void ExportFieldCharacter(FieldCharacter fldChar)
		{
			switch (fldChar.FieldCharacterType)
			{
			case FieldCharacterType.Start:
				this.writer.WriteGroupStart("field", false);
				if (fldChar.FieldInfo.IsLocked)
				{
					this.writer.WriteTag("fldlock");
				}
				if (fldChar.FieldInfo.IsDirty)
				{
					this.writer.WriteTag("flddirty");
				}
				this.writer.WriteGroup("fldinst", true);
				return;
			case FieldCharacterType.End:
				this.writer.WriteGroupEnd();
				this.writer.WriteGroupEnd();
				return;
			case FieldCharacterType.Separator:
				this.writer.WriteGroupEnd();
				this.writer.WriteGroupStart("fldrslt", false);
				return;
			default:
				return;
			}
		}

		void ExportSpan(Run span)
		{
			string text = span.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			using (this.writer.WriteGroup())
			{
				this.ExportSpanProperties(span);
				this.writer.WriteText(text);
			}
		}

		void ExportSpanProperties(Run span)
		{
			int parameter;
			if (!string.IsNullOrEmpty(span.Properties.StyleId) && this.Context.StyleTable.TryGetValue(span.Properties.StyleId, out parameter))
			{
				this.writer.WriteTag("cs", parameter);
			}
			InlineExporter.ExportCharacterProperties(span.Properties, this.Context, this.writer);
		}

		void ExportBookmarkRangeStart(BookmarkRangeStart bookmarkRangeStart)
		{
			using (this.writer.WriteGroup("bkmkstart", true))
			{
				this.writer.WriteText(bookmarkRangeStart.Bookmark.Name);
			}
		}

		void ExportBookmarkRangeEnd(BookmarkRangeEnd bookmarkRangeEnd)
		{
			using (this.writer.WriteGroup("bkmkend", true))
			{
				this.writer.WriteText(bookmarkRangeEnd.Bookmark.Name);
			}
		}

		void ExportBreak(Break br)
		{
			switch (br.BreakType)
			{
			case BreakType.LineBreak:
				using (this.writer.WriteGroup())
				{
					this.writer.WriteTag("lbr", RtfHelper.LineBreakTextWrappingMapper.GetFromValue(br.TextWrappingRestartLocation));
					this.writer.WriteTag("line");
					return;
				}
				break;
			case BreakType.PageBreak:
				break;
			case BreakType.ColumnBreak:
				this.writer.WriteTag("column");
				return;
			default:
				return;
			}
			this.writer.WriteTag("page");
		}

		void ExportCommentRangeStart(CommentRangeStart commentRangeStart)
		{
			int idForComment = this.Context.GetIdForComment(commentRangeStart.Comment);
			using (this.writer.WriteGroup("atrfstart", true))
			{
				this.writer.WriteText(idForComment.ToString());
			}
		}

		void ExportCommentRangeEnd(CommentRangeEnd commentRangeEnd)
		{
			int idForComment = this.Context.GetIdForComment(commentRangeEnd.Comment);
			using (this.writer.WriteGroup("atrfend", true))
			{
				this.writer.WriteText(idForComment.ToString());
			}
			this.documentExporter.CommentExporter.ExportComment(commentRangeEnd);
		}

		readonly RtfDocumentExporter documentExporter;

		readonly RtfWriter writer;
	}
}
