using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class RtfTagHandlers
	{
		public static void InitializeAllTagHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["deff"] = new ControlTagHandler(RtfTagHandlers.DefaultFontHandler);
			tagHandlers["deflang"] = new ControlTagHandler(RtfTagHandlers.DefaultLanguageHandler);
			tagHandlers["adeff"] = new ControlTagHandler(RtfTagHandlers.DefaultLanguageHandler);
			DocumentStyleHandlers.InitializeDocumentStyleHandlers(tagHandlers);
			SectionStyleHandlers.InitializeSectionStyleHandlers(tagHandlers);
			SpanStyleHandlers.InitializeSpanStyleHandlers(tagHandlers);
			ParagraphStyleHandlers.InitializeParagraphStyleHandlers(tagHandlers);
			TableStyleHandlers.InitializeTableStyleHandlers(tagHandlers);
			SpecialSymbolsHandlers.InitializeSymbolsHandlers(tagHandlers);
			tagHandlers["cs"] = new ControlTagHandler(RtfTagHandlers.CharacterStyleHandler);
			tagHandlers["s"] = new ControlTagHandler(RtfTagHandlers.ParagraphStyleHandler);
			tagHandlers["ts"] = new ControlTagHandler(RtfTagHandlers.TableStyleHandler);
			tagHandlers["sect"] = new ControlTagHandler(RtfTagHandlers.SectionEndHandler);
			tagHandlers["par"] = new ControlTagHandler(RtfTagHandlers.ParagraphEndHandler);
			tagHandlers["cell"] = new ControlTagHandler(RtfTagHandlers.CellEndHandler);
			tagHandlers["nestcell"] = new ControlTagHandler(RtfTagHandlers.CellEndHandler);
			tagHandlers["row"] = new ControlTagHandler(RtfTagHandlers.RowEndHandler);
			tagHandlers["nestrow"] = new ControlTagHandler(RtfTagHandlers.RowEndHandler);
			tagHandlers["intbl"] = new ControlTagHandler(RtfTagHandlers.ParagraphInTableHandler);
			tagHandlers["itap"] = new ControlTagHandler(RtfTagHandlers.ParagraphNestingLevelHandler);
		}

		static void DefaultFontHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "deff");
			context.DefaultFontId = "f" + tag.ValueAsNumber.ToString();
		}

		static void DefaultLanguageHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "deflang", "adeff" });
		}

		static void SectionEndHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sect");
			context.HandleSectionEnd();
		}

		static void ParagraphInTableHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "intbl");
			context.CurrentStyle.IsInTable = true;
			context.CurrentStyle.CurrentNestingLevel = 1;
		}

		static void ParagraphNestingLevelHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "itap");
			context.CurrentStyle.CurrentNestingLevel = tag.ValueAsNumber;
		}

		static void ParagraphEndHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "par");
			context.HandleParagraphEnd();
		}

		static void RowEndHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "row", "nestrow" });
			context.HandleRowEnd();
		}

		static void CellEndHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "cell", "nestcell" });
			context.HandleCellEnd();
		}

		static void CharacterStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "cs");
			Style styleById = context.StylesTable.GetStyleById(tag.ValueAsNumber);
			if (styleById != null && styleById.StyleType == StyleType.Character)
			{
				context.CurrentStyle.CharacterStyle.StyleId = styleById.Id;
			}
		}

		static void ParagraphStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "s");
			Style styleById = context.StylesTable.GetStyleById(tag.ValueAsNumber);
			if (styleById != null && styleById.StyleType == StyleType.Paragraph)
			{
				context.CurrentStyle.ParagraphStyle.StyleId = styleById.Id;
			}
		}

		static void TableStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ts");
			Style styleById = context.StylesTable.GetStyleById(tag.ValueAsNumber);
			if (styleById != null && styleById.StyleType == StyleType.Table)
			{
				context.CurrentStyle.RowStyle.TableStyleId = styleById.Id;
			}
		}
	}
}
