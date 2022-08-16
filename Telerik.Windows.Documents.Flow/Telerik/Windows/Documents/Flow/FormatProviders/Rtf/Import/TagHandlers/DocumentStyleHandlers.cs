using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class DocumentStyleHandlers
	{
		public static void InitializeDocumentStyleHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["viewkind"] = new ControlTagHandler(DocumentStyleHandlers.DocumentLayoutModeHandler);
			tagHandlers["paperw"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageSizeHandler);
			tagHandlers["paperh"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageSizeHandler);
			tagHandlers["margl"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageMarginsHandler);
			tagHandlers["margr"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageMarginsHandler);
			tagHandlers["margt"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageMarginsHandler);
			tagHandlers["margb"] = new ControlTagHandler(DocumentStyleHandlers.DocumentPageMarginsHandler);
			tagHandlers["landscape"] = new ControlTagHandler(DocumentStyleHandlers.DocumentLadscapeModeHandler);
			tagHandlers["facingp"] = new ControlTagHandler(DocumentStyleHandlers.DocumentDifferentOddEvenHeaderFooterHandler);
			tagHandlers["deftab"] = new ControlTagHandler(DocumentStyleHandlers.DocumentTabDefaultWidthHandler);
		}

		static void DocumentLayoutModeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "viewkind");
			context.Document.ViewType = RtfHelper.DocumentViewTypeMapper.GetToValue(tag.ValueAsNumber);
		}

		static void DocumentPageSizeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "paperw", "paperh" });
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "paperw")
				{
					context.CurrentStyle.DocumentSettings.PageWidth = Unit.TwipToDip((double)tag.ValueAsNumber);
					return;
				}
				if (!(name == "paperh"))
				{
					return;
				}
				context.CurrentStyle.DocumentSettings.PageHeight = (double)Unit.TwipToDipF((double)tag.ValueAsNumber);
			}
		}

		static void DocumentPageMarginsHandler(RtfTag tag, RtfImportContext context)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "margl")
				{
					context.CurrentStyle.DocumentSettings.LeftPageMargin = Unit.TwipToDip((double)tag.ValueAsNumber);
					return;
				}
				if (name == "margr")
				{
					context.CurrentStyle.DocumentSettings.RightPageMargin = Unit.TwipToDip((double)tag.ValueAsNumber);
					return;
				}
				if (name == "margt")
				{
					context.CurrentStyle.DocumentSettings.TopPageMargin = Unit.TwipToDip((double)tag.ValueAsNumber);
					return;
				}
				if (!(name == "margb"))
				{
					return;
				}
				context.CurrentStyle.DocumentSettings.BottomPageMargin = Unit.TwipToDip((double)tag.ValueAsNumber);
			}
		}

		static void DocumentLadscapeModeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "landscape");
			context.CurrentStyle.DocumentSettings.PageOrientation = PageOrientation.Landscape;
		}

		static void DocumentDifferentOddEvenHeaderFooterHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "facingp");
			context.Document.HasDifferentEvenOddPageHeadersFooters = true;
		}

		static void DocumentTabDefaultWidthHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "deftab");
			context.Document.DefaultTabStopWidth = (double)Unit.TwipToDipI((double)tag.ValueAsNumber);
		}
	}
}
