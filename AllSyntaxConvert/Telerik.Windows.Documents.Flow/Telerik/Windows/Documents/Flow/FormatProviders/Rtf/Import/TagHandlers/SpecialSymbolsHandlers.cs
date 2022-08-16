using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class SpecialSymbolsHandlers
	{
		public static void InitializeSymbolsHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["tab"] = new ControlTagHandler(SpecialSymbolsHandlers.TabHandler);
			tagHandlers["_"] = new ControlTagHandler(SpecialSymbolsHandlers.NonBreakingHyphenHandler);
			tagHandlers["~"] = new ControlTagHandler(SpecialSymbolsHandlers.NonBreakingSpaceHandler);
			tagHandlers["rquote"] = new ControlTagHandler(SpecialSymbolsHandlers.RightSingleQuoteHandler);
			tagHandlers["lquote"] = new ControlTagHandler(SpecialSymbolsHandlers.LeftSingleQuoteHandler);
			tagHandlers["rdblquote"] = new ControlTagHandler(SpecialSymbolsHandlers.RightDoubleQuoteHandler);
			tagHandlers["ldblquote"] = new ControlTagHandler(SpecialSymbolsHandlers.LeftDoubleQuoteHandler);
			tagHandlers["endash"] = new ControlTagHandler(SpecialSymbolsHandlers.EnDashHandler);
			tagHandlers["emdash"] = new ControlTagHandler(SpecialSymbolsHandlers.EmDashHandler);
			tagHandlers["enspace"] = new ControlTagHandler(SpecialSymbolsHandlers.EnSpaceHandler);
			tagHandlers["emspace"] = new ControlTagHandler(SpecialSymbolsHandlers.EmSpaceHandler);
			tagHandlers["bullet"] = new ControlTagHandler(SpecialSymbolsHandlers.BulletCharHandler);
			tagHandlers["page"] = new ControlTagHandler(SpecialSymbolsHandlers.BreakHandler);
			tagHandlers["line"] = new ControlTagHandler(SpecialSymbolsHandlers.BreakHandler);
			tagHandlers["column"] = new ControlTagHandler(SpecialSymbolsHandlers.BreakHandler);
		}

		static void TabHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("\t");
		}

		static void EnDashHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("–");
		}

		static void EmDashHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("—");
		}

		static void EnSpaceHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("\u2002");
		}

		static void EmSpaceHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("\u2003");
		}

		static void NonBreakingHyphenHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("–");
		}

		static void RightSingleQuoteHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("’");
		}

		static void LeftSingleQuoteHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("‘");
		}

		static void RightDoubleQuoteHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("”");
		}

		static void LeftDoubleQuoteHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("“");
		}

		static void BulletCharHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("•");
		}

		static void NonBreakingSpaceHandler(RtfTag tag, RtfImportContext context)
		{
			context.AddText("\u00a0");
		}

		static void BreakHandler(RtfTag tag, RtfImportContext context)
		{
			Break @break = new Break(context.Document);
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "page"))
				{
					if (!(name == "line"))
					{
						if (name == "column")
						{
							@break.BreakType = BreakType.ColumnBreak;
						}
					}
					else
					{
						@break.BreakType = BreakType.LineBreak;
						@break.TextWrappingRestartLocation = context.CurrentStyle.CurrentTextWrapping;
					}
				}
				else
				{
					@break.BreakType = BreakType.PageBreak;
				}
			}
			context.AddInline(@break);
		}
	}
}
