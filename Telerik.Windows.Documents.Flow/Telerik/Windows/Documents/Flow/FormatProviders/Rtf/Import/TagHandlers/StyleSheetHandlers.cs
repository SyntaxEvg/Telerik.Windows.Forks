using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class StyleSheetHandlers
	{
		public static void InitializeStyleDefinitionHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["cs"] = new ControlTagHandler(StyleSheetHandlers.StyleDefinitionIdHandler);
			tagHandlers["s"] = new ControlTagHandler(StyleSheetHandlers.StyleDefinitionIdHandler);
			tagHandlers["ts"] = new ControlTagHandler(StyleSheetHandlers.StyleDefinitionIdHandler);
			tagHandlers["sbasedon"] = new ControlTagHandler(StyleSheetHandlers.BasedOnStyleHandler);
			tagHandlers["slink"] = new ControlTagHandler(StyleSheetHandlers.LinkedStyleHandler);
			tagHandlers["snext"] = new ControlTagHandler(StyleSheetHandlers.NextParagraphStyleHandler);
			tagHandlers["sqformat"] = new ControlTagHandler(StyleSheetHandlers.IsPrimaryStyleHandler);
			tagHandlers["spriority"] = new ControlTagHandler(StyleSheetHandlers.UIPriorityStyleHandler);
		}

		static void StyleDefinitionIdHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "cs", "s", "ts" });
			context.CurrentStyle.StyleDefinitionInfo.StyleId = new int?(tag.ValueAsNumber);
		}

		static void BasedOnStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sbasedon");
			context.CurrentStyle.StyleDefinitionInfo.BasedOnStyleId = new int?(tag.ValueAsNumber);
		}

		static void LinkedStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "slink");
			context.CurrentStyle.StyleDefinitionInfo.LinkedStyleId = new int?(tag.ValueAsNumber);
		}

		static void NextParagraphStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "snext");
			context.CurrentStyle.StyleDefinitionInfo.NextStyleId = new int?(tag.ValueAsNumber);
		}

		static void IsPrimaryStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sqformat");
			context.CurrentStyle.StyleDefinitionInfo.IsPrimary = true;
		}

		static void UIPriorityStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "spriority");
			context.CurrentStyle.StyleDefinitionInfo.UIPriority = new int?(tag.ValueAsNumber);
		}
	}
}
