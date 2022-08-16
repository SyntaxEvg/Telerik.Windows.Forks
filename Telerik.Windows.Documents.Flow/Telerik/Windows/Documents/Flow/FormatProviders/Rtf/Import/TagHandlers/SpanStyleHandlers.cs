using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class SpanStyleHandlers
	{
		public static void InitializeSpanStyleHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["plain"] = new ControlTagHandler(SpanStyleHandlers.SpanResetFormattingHandler);
			tagHandlers["sub"] = new ControlTagHandler(SpanStyleHandlers.SpanBaselineAlignmentHandler);
			tagHandlers["super"] = new ControlTagHandler(SpanStyleHandlers.SpanBaselineAlignmentHandler);
			tagHandlers["nosupersub"] = new ControlTagHandler(SpanStyleHandlers.SpanBaselineAlignmentHandler);
			tagHandlers["b"] = new ControlTagHandler(SpanStyleHandlers.SpanBoldHandler);
			tagHandlers["i"] = new ControlTagHandler(SpanStyleHandlers.SpanItalicHandler);
			tagHandlers["strike"] = new ControlTagHandler(SpanStyleHandlers.SpanStrikethroughHandler);
			tagHandlers["striked"] = new ControlTagHandler(SpanStyleHandlers.SpanStrikethroughHandler);
			tagHandlers["fs"] = new ControlTagHandler(SpanStyleHandlers.SpanFontSizeHandler);
			tagHandlers["f"] = new ControlTagHandler(SpanStyleHandlers.SpanFontHandler);
			tagHandlers["cf"] = new ControlTagHandler(SpanStyleHandlers.SpanForecolorHandler);
			tagHandlers["cb"] = new ControlTagHandler(SpanStyleHandlers.SpanHighlightColorHandler);
			tagHandlers["highlight"] = new ControlTagHandler(SpanStyleHandlers.SpanHighlightColorHandler);
			tagHandlers["ulc"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineColorHandler);
			tagHandlers["ulnone"] = new ControlTagHandler(SpanStyleHandlers.SpanClearUnderlineHandler);
			tagHandlers["ul"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["uld"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["uldash"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["uldashd"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["uldashdd"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["uldb"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulhwave"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulldash"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulth"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulthd"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulthdash"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulthdashd"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulthdashdd"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulthldash"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ululdbwave"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulw"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["ulwave"] = new ControlTagHandler(SpanStyleHandlers.SpanUnderlineHandler);
			tagHandlers["chcbpat"] = new ControlTagHandler(SpanStyleHandlers.SpanShadingBackroundColorHandler);
			tagHandlers["chcfpat"] = new ControlTagHandler(SpanStyleHandlers.SpanShadingForegroundColorHandler);
			tagHandlers["chshdng"] = new ControlTagHandler(SpanStyleHandlers.SpanShadingPercentHandler);
			tagHandlers["lbr"] = new ControlTagHandler(SpanStyleHandlers.TextWrappingHandler);
			tagHandlers["ltrch"] = new ControlTagHandler(SpanStyleHandlers.SpanLTRHandler);
			tagHandlers["rtlch"] = new ControlTagHandler(SpanStyleHandlers.SpanRTLHandler);
			tagHandlers["flddirty"] = new ControlTagHandler(SpanStyleHandlers.FieldIsDirtyHandler);
			tagHandlers["fldlock"] = new ControlTagHandler(SpanStyleHandlers.FieldIsLockedHandler);
		}

		static void SpanResetFormattingHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "plain");
			context.CurrentStyle.ResetSpanStyle();
		}

		static void SpanFontSizeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "fs");
			double num = (double)tag.ValueAsNumber / 2.0;
			num = Unit.PointToDip(num);
			context.CurrentStyle.CharacterStyle.FontSize = num;
		}

		static void SpanBaselineAlignmentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "sub", "super", "nosupersub" });
			Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment baselineAlignment = Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline;
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "sub"))
				{
					if (!(name == "super"))
					{
						if (name == "nosupersub")
						{
							baselineAlignment = Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline;
						}
					}
					else
					{
						baselineAlignment = Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Superscript;
					}
				}
				else
				{
					baselineAlignment = Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Subscript;
				}
			}
			context.CurrentStyle.CharacterStyle.BaselineAlignment = baselineAlignment;
		}

		static void SpanBoldHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "b");
			context.CurrentStyle.CharacterStyle.FontWeight = ((tag.HasValue && tag.ValueAsNumber == 0) ? FontWeights.Normal : FontWeights.Bold);
		}

		static void SpanItalicHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "i");
			context.CurrentStyle.CharacterStyle.FontStyle = ((tag.HasValue && tag.ValueAsNumber == 0) ? FontStyles.Normal : FontStyles.Italic);
		}

		static void SpanStrikethroughHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "strike", "striked" });
			context.CurrentStyle.CharacterStyle.Strikethrough = !tag.HasValue || tag.ValueAsNumber != 0;
		}

		static void SpanFontHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "f");
			context.CurrentStyle.CharacterStyle.FontFamily = new ThemableFontFamily(context.FontTable.GetFontFamily(tag.FullName));
		}

		static void SpanClearUnderlineHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ulnone");
			context.CurrentStyle.CharacterStyle.Underline.Pattern = UnderlinePattern.None;
		}

		static void SpanUnderlineColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ulc");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.CharacterStyle.Underline.Color = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void SpanUnderlineHandler(RtfTag tag, RtfImportContext context)
		{
			UnderlinePattern pattern;
			if (tag.HasValue && tag.ValueAsNumber == 0)
			{
				pattern = UnderlinePattern.None;
			}
			else
			{
				pattern = RtfHelper.RtfTagToUndelineDecoration(tag.Name);
			}
			context.CurrentStyle.CharacterStyle.Underline.Pattern = pattern;
		}

		static void SpanForecolorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "cf");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.CharacterStyle.ForegroundColor = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void SpanHighlightColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "cb", "highlight" });
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.CharacterStyle.HighlightColor = (rtfColor.IsAutomatic ? Run.HighlightColorPropertyDefinition.DefaultValue.Value : rtfColor.Color);
			}
		}

		static void SpanShadingBackroundColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "chcbpat");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.CharacterStyle.Shading.BackgroundColor = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void SpanShadingForegroundColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "chcfpat");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.CharacterStyle.Shading.PatternColor = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void SpanShadingPercentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "chshdng");
			context.CurrentStyle.CharacterStyle.Shading.Pattern = RtfHelper.RtfTagToShadingPattern(tag);
		}

		static void TextWrappingHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "lbr");
			context.CurrentStyle.CurrentTextWrapping = RtfHelper.LineBreakTextWrappingMapper.GetToValue(tag.ValueAsNumber);
		}

		static void FieldIsLockedHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "fldlock");
			context.FieldContext.SetIsLocked(true);
		}

		static void FieldIsDirtyHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "flddirty");
			context.FieldContext.SetIsDirty(true);
		}

		static void SpanLTRHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ltrch");
			context.CurrentStyle.CharacterStyle.FlowDirection = Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.LeftToRight;
		}

		static void SpanRTLHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "rtlch");
			context.CurrentStyle.CharacterStyle.FlowDirection = Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.RightToLeft;
		}
	}
}
