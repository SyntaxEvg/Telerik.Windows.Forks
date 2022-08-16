using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class ParagraphStyleHandlers
	{
		public static void InitializeParagraphStyleHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["pard"] = new ControlTagHandler(ParagraphStyleHandlers.ResetParagraphStyleHandler);
			tagHandlers["qc"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["ql"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["qr"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["qj"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["qd"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["qk"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["qt"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAlignementHandler);
			tagHandlers["fi"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphFirstLineIndentHandler);
			tagHandlers["cufi"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphFirstLineIndentInCharUnitsHandler);
			tagHandlers["li"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLeftIndentHandler);
			tagHandlers["lin"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLeftIndentBiDirectionalHandler);
			tagHandlers["culi"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLeftIndentInCharUnitsHandler);
			tagHandlers["ri"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphRightIndentHandler);
			tagHandlers["rin"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphRightIndentBiDirectionalHandler);
			tagHandlers["curi"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphRightIndentInCharUnitsHandler);
			tagHandlers["sb"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphSpacingBeforeHandler);
			tagHandlers["sbauto"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAutomaticSpacingBeforeHandler);
			tagHandlers["sa"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphSpacingAfterHandler);
			tagHandlers["saauto"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphAutomaticSpacingAfterHandler);
			tagHandlers["lisb"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphSpacingBeforeInCharUnitsHandler);
			tagHandlers["lisa"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphSpacingAfterInCharUnitsHandler);
			tagHandlers["sl"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLineSpacingHandler);
			tagHandlers["slmult"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLineSpacingTypeHandler);
			tagHandlers["cbpat"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphShadingBackroundColorHandler);
			tagHandlers["cfpat"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphShadingForegroundColorHandler);
			tagHandlers["shading"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphShadingPercentHandler);
			tagHandlers["brdrl"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphBordersHandler);
			tagHandlers["brdrt"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphBordersHandler);
			tagHandlers["brdrr"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphBordersHandler);
			tagHandlers["brdrb"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphBordersHandler);
			tagHandlers["brdrbtw"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphBordersHandler);
			tagHandlers["ls"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphListIdHandler);
			tagHandlers["ilvl"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphListLevelHandler);
			tagHandlers["tx"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopPositionHandler);
			tagHandlers["tqr"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopTypeHandler);
			tagHandlers["tqc"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopTypeHandler);
			tagHandlers["tqdec"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopTypeHandler);
			tagHandlers["tb"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopBarPositionHandler);
			tagHandlers["tldot"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopLeaderHandler);
			tagHandlers["tlhyph"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopLeaderHandler);
			tagHandlers["tlmdot"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopLeaderHandler);
			tagHandlers["tlth"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopLeaderHandler);
			tagHandlers["tlul"] = new ControlTagHandler(ParagraphStyleHandlers.TabStopLeaderHandler);
			tagHandlers["ltrpar"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphLtrHandler);
			tagHandlers["rtlpar"] = new ControlTagHandler(ParagraphStyleHandlers.ParagraphRtlHandler);
			BorderHandlers.InitializeBorderHandlers(tagHandlers);
		}

		static void ResetParagraphStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "pard");
			context.CurrentStyle.CurrentBorder = null;
			context.CurrentStyle.CurrentBorderedElementType = BorderedElementType.Paragraph;
			context.CurrentStyle.ResetParagraphStyle();
		}

		static void ParagraphAlignementHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentStyle.ParagraphStyle.TextAlignment = RtfHelper.RtfTagToAlignment(tag);
		}

		static void ParagraphSpacingBeforeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sb");
			context.CurrentStyle.ParagraphStyle.Spacing.SpacingBefore = Unit.TwipToDip((double)tag.ValueAsNumber);
		}

		static void ParagraphAutomaticSpacingBeforeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sbauto");
			if (tag.ValueAsText == "1")
			{
				context.CurrentStyle.ParagraphStyle.Spacing.AutomaticSpacingBefore = true;
			}
		}

		static void ParagraphSpacingAfterHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sa");
			context.CurrentStyle.ParagraphStyle.Spacing.SpacingAfter = Unit.TwipToDip((double)tag.ValueAsNumber);
		}

		static void ParagraphAutomaticSpacingAfterHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "saauto");
			if (tag.ValueAsText == "1")
			{
				context.CurrentStyle.ParagraphStyle.Spacing.AutomaticSpacingAfter = true;
			}
		}

		static void ParagraphSpacingBeforeInCharUnitsHandler(RtfTag tag, RtfImportContext context)
		{
		}

		static void ParagraphSpacingAfterInCharUnitsHandler(RtfTag tag, RtfImportContext context)
		{
		}

		static void ParagraphLineSpacingHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sl");
			if (tag.HasValue && tag.ValueAsNumber != 0)
			{
				if (tag.ValueAsNumber < 0)
				{
					context.CurrentStyle.ParagraphStyle.Spacing.LineSpacingType = HeightType.Exact;
					context.CurrentStyle.ParagraphStyle.Spacing.LineSpacing = Unit.TwipToDip((double)(-(double)tag.ValueAsNumber));
					return;
				}
				context.CurrentStyle.ParagraphStyle.Spacing.LineSpacingType = HeightType.AtLeast;
				context.CurrentStyle.ParagraphStyle.Spacing.LineSpacing = Unit.TwipToDip((double)tag.ValueAsNumber);
			}
		}

		static void ParagraphLineSpacingTypeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "slmult");
			if (tag.ValueAsNumber == 1)
			{
				double num = Unit.DipToTwip(context.CurrentStyle.ParagraphStyle.Spacing.LineSpacing);
				num = Math.Round(num / RtfHelper.DefaultLineSpacing, 2);
				context.CurrentStyle.ParagraphStyle.Spacing.LineSpacingType = HeightType.Auto;
				context.CurrentStyle.ParagraphStyle.Spacing.LineSpacing = num;
			}
		}

		static void ParagraphFirstLineIndentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "fi");
			double num = Unit.TwipToDip((double)tag.ValueAsNumber);
			if (num > 0.0)
			{
				context.CurrentStyle.ParagraphStyle.Indentation.FirstLineIndent = num;
				return;
			}
			context.CurrentStyle.ParagraphStyle.Indentation.HangingIndent = -num;
		}

		static void ParagraphFirstLineIndentInCharUnitsHandler(RtfTag tag, RtfImportContext context)
		{
		}

		static void ParagraphLeftIndentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "li");
			double num = Unit.TwipToDip((double)tag.ValueAsNumber);
			if (context.CurrentStyle.ParagraphStyle.FlowDirection == FlowDirection.LeftToRight)
			{
				context.CurrentStyle.ParagraphStyle.Indentation.LeftIndent = num;
				return;
			}
			context.CurrentStyle.ParagraphStyle.Indentation.RightIndent = num;
		}

		static void ParagraphLeftIndentBiDirectionalHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "lin");
			double leftIndent = Unit.TwipToDip((double)tag.ValueAsNumber);
			context.CurrentStyle.ParagraphStyle.Indentation.LeftIndent = leftIndent;
		}

		static void ParagraphLeftIndentInCharUnitsHandler(RtfTag tag, RtfImportContext context)
		{
		}

		static void ParagraphRightIndentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ri");
			double num = Unit.TwipToDip((double)tag.ValueAsNumber);
			if (context.CurrentStyle.ParagraphStyle.FlowDirection == FlowDirection.LeftToRight)
			{
				context.CurrentStyle.ParagraphStyle.Indentation.RightIndent = num;
				return;
			}
			context.CurrentStyle.ParagraphStyle.Indentation.LeftIndent = num;
		}

		static void ParagraphRightIndentBiDirectionalHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "rin");
			context.CurrentStyle.ParagraphStyle.Indentation.RightIndent = Unit.TwipToDip((double)tag.ValueAsNumber);
		}

		static void ParagraphRightIndentInCharUnitsHandler(RtfTag tag, RtfImportContext context)
		{
		}

		static void ParagraphShadingBackroundColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "cbpat");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.ParagraphStyle.Shading.BackgroundColor = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void ParagraphShadingForegroundColorHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "cfpat");
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentStyle.ParagraphStyle.Shading.PatternColor = new ThemableColor(rtfColor.Color, rtfColor.IsAutomatic);
			}
		}

		static void ParagraphShadingPercentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "shading");
			context.CurrentStyle.ParagraphStyle.Shading.Pattern = RtfHelper.RtfTagToShadingPattern(tag);
		}

		static void ParagraphBordersHandler(RtfTag tag, RtfImportContext context)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "brdrt"))
				{
					if (!(name == "brdrl"))
					{
						if (!(name == "brdrb"))
						{
							if (!(name == "brdrr"))
							{
								if (!(name == "brdrbtw"))
								{
									goto IL_E7;
								}
								context.CurrentStyle.CurrentBorder = context.CurrentStyle.ParagraphBorders.Between;
							}
							else
							{
								context.CurrentStyle.CurrentBorder = context.CurrentStyle.ParagraphBorders.Right;
							}
						}
						else
						{
							context.CurrentStyle.CurrentBorder = context.CurrentStyle.ParagraphBorders.Bottom;
						}
					}
					else
					{
						context.CurrentStyle.CurrentBorder = context.CurrentStyle.ParagraphBorders.Left;
					}
				}
				else
				{
					context.CurrentStyle.CurrentBorder = context.CurrentStyle.ParagraphBorders.Top;
				}
				context.CurrentStyle.ParagraphBorders.HasValue = true;
				context.CurrentStyle.CurrentBorder.HasValue = true;
				context.CurrentStyle.CurrentBorderedElementType = BorderedElementType.Paragraph;
				return;
			}
			IL_E7:
			throw new RtfUnexpectedElementException("Paragraph border control word", tag.Name);
		}

		static void ParagraphLtrHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ltrpar");
			context.CurrentStyle.ParagraphStyle.FlowDirection = FlowDirection.LeftToRight;
		}

		static void ParagraphRtlHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "rtlpar");
			context.CurrentStyle.ParagraphStyle.FlowDirection = FlowDirection.RightToLeft;
		}

		static void ParagraphListIdHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ls");
			context.CurrentStyle.CurrentList = context.ListOverrideTable.GetList(tag.ValueAsNumber);
			if (context.CurrentStyle.CurrentList != null)
			{
				context.CurrentStyle.ParagraphStyle.ListId = context.CurrentStyle.CurrentList.Id;
			}
		}

		static void ParagraphListLevelHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ilvl");
			context.CurrentStyle.CurrentListLevel = tag.ValueAsNumber;
			context.CurrentStyle.ParagraphStyle.ListLevel = context.CurrentStyle.CurrentListLevel;
		}

		static void TabStopLeaderHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentStyle.CurrentTabStop.Leader = RtfHelper.TabStopLeaderMapper.GetToValue(tag.Name);
		}

		static void TabStopTypeHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentStyle.CurrentTabStop.Type = RtfHelper.TabStopTypeMapper.GetToValue(tag.Name);
		}

		static void TabStopPositionHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tx");
			context.CurrentStyle.CurrentTabStop.Position = (double)Unit.TwipToDipI((double)tag.ValueAsNumber);
			context.CurrentStyle.ParagraphStyle.TabStops = context.CurrentStyle.ParagraphStyle.Properties.TabStops.LocalValue.Insert(context.CurrentStyle.CurrentTabStop.GetTabStop());
			context.CurrentStyle.ResetTabStop();
		}

		static void TabStopBarPositionHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tb");
			context.CurrentStyle.CurrentTabStop.Type = TabStopType.Bar;
			context.CurrentStyle.CurrentTabStop.Position = (double)Unit.TwipToDipI((double)tag.ValueAsNumber);
			context.CurrentStyle.ParagraphStyle.TabStops = context.CurrentStyle.ParagraphStyle.Properties.TabStops.LocalValue.Insert(context.CurrentStyle.CurrentTabStop.GetTabStop());
			context.CurrentStyle.ResetTabStop();
		}
	}
}
