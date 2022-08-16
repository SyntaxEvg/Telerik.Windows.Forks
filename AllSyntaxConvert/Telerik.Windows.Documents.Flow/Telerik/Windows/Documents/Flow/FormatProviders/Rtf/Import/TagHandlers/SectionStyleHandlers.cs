using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;
using VerticalAlignment = Telerik.Windows.Documents.Flow.Model.Styles.VerticalAlignment;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class SectionStyleHandlers
	{
		public static void InitializeSectionStyleHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["sectd"] = new ControlTagHandler(SectionStyleHandlers.ResetSectionStyleHandler);
			tagHandlers["lndscpsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionLadscapeModeHandler);
			tagHandlers["pgwsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionPageSizeHandler);
			tagHandlers["pghsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionPageSizeHandler);
			tagHandlers["marglsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionMarginsHandler);
			tagHandlers["margrsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionMarginsHandler);
			tagHandlers["margtsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionMarginsHandler);
			tagHandlers["margbsxn"] = new ControlTagHandler(SectionStyleHandlers.SectionMarginsHandler);
			tagHandlers["titlepg"] = new ControlTagHandler(SectionStyleHandlers.SectionDifferentFirstPageHeaderFooterHandler);
			tagHandlers["headery"] = new ControlTagHandler(SectionStyleHandlers.SectionHeaderTopMarginHandler);
			tagHandlers["footery"] = new ControlTagHandler(SectionStyleHandlers.SectionFooterBottomMarginHandler);
			tagHandlers["sbkpage"] = new ControlTagHandler(SectionStyleHandlers.SectionTypePageHandler);
			tagHandlers["sbkeven"] = new ControlTagHandler(SectionStyleHandlers.SectionTypePageHandler);
			tagHandlers["sbkodd"] = new ControlTagHandler(SectionStyleHandlers.SectionTypePageHandler);
			tagHandlers["sbkcol"] = new ControlTagHandler(SectionStyleHandlers.SectionTypePageHandler);
			tagHandlers["sbknone"] = new ControlTagHandler(SectionStyleHandlers.SectionTypePageHandler);
			tagHandlers["vertalt"] = new ControlTagHandler(SectionStyleHandlers.SectionVerticalAlignmentHandler);
			tagHandlers["vertalc"] = new ControlTagHandler(SectionStyleHandlers.SectionVerticalAlignmentHandler);
			tagHandlers["vertalj"] = new ControlTagHandler(SectionStyleHandlers.SectionVerticalAlignmentHandler);
			tagHandlers["vertalb"] = new ControlTagHandler(SectionStyleHandlers.SectionVerticalAlignmentHandler);
			tagHandlers["vertal"] = new ControlTagHandler(SectionStyleHandlers.SectionVerticalAlignmentHandler);
			tagHandlers["pgnstarts"] = new ControlTagHandler(SectionStyleHandlers.SectionStartingPageNumberHandler);
			tagHandlers["pgnhn"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterHeadingStyleHandler);
			tagHandlers["pgnhnsh"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterSeparatorCharacterHandler);
			tagHandlers["pgnhnsp"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterSeparatorCharacterHandler);
			tagHandlers["pgnhnsc"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterSeparatorCharacterHandler);
			tagHandlers["pgnhnsm"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterSeparatorCharacterHandler);
			tagHandlers["pgnhnsn"] = new ControlTagHandler(SectionStyleHandlers.SectionChapterSeparatorCharacterHandler);
			tagHandlers["pgnbidia"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgncnum"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgndec"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnhindib"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnhindid"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnhindic"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnhindia"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnchosung"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnganada"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnlcltr"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnlcrm"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnthaic"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnthaia"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnthaib"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnucltr"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnucrm"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
			tagHandlers["pgnvieta"] = new ControlTagHandler(SectionStyleHandlers.SectionPageNumberFormatHandler);
		}

		static void ResetSectionStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "sectd");
			context.CurrentStyle.ResetSectionStyle();
		}

		static void SectionTypePageHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentStyle.SectionStyle.SectionType = RtfHelper.SectionTypeMapper.GetToValue(tag.Name);
		}

		static void SectionPageSizeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "pgwsxn", "pghsxn" });
			float num = Unit.TwipToDipF((double)tag.ValueAsNumber);
			Size pageSize = context.CurrentStyle.SectionStyle.PageSize;
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "pgwsxn"))
				{
					if (name == "pghsxn")
					{
						pageSize.Height = (double)num;
					}
				}
				else
				{
					pageSize.Width = (double)num;
				}
			}
			context.CurrentStyle.SectionStyle.PageSize = pageSize;
		}

		static void SectionLadscapeModeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "lndscpsxn");
			context.CurrentStyle.SectionStyle.PageOrientation = PageOrientation.Landscape;
		}

		static void SectionMarginsHandler(RtfTag tag, RtfImportContext context)
		{
			Padding pageMargins = context.CurrentStyle.SectionStyle.PageMargins;
			double left = pageMargins.Left;
			double right = pageMargins.Right;
			double top = pageMargins.Top;
			double bottom = pageMargins.Bottom;
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "marglsxn"))
				{
					if (!(name == "margrsxn"))
					{
						if (!(name == "margtsxn"))
						{
							if (name == "margbsxn")
							{
								bottom = Unit.TwipToDip((double)tag.ValueAsNumber);
							}
						}
						else
						{
							top = Unit.TwipToDip((double)tag.ValueAsNumber);
						}
					}
					else
					{
						right = Unit.TwipToDip((double)tag.ValueAsNumber);
					}
				}
				else
				{
					left = Unit.TwipToDip((double)tag.ValueAsNumber);
				}
			}
			context.CurrentStyle.SectionStyle.PageMargins = new Padding(left, top, right, bottom);
		}

		static void SectionDifferentFirstPageHeaderFooterHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "titlepg");
			context.CurrentStyle.SectionStyle.HasDifferentFirstPageHeaderFooter = true;
		}

		static void SectionHeaderTopMarginHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "headery");
			context.CurrentStyle.SectionStyle.HeaderTopMargin = (double)Unit.TwipToDipI((double)tag.ValueAsNumber);
		}

		static void SectionFooterBottomMarginHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "footery");
			context.CurrentStyle.SectionStyle.FooterBottomMargin = (double)Unit.TwipToDipI((double)tag.ValueAsNumber);
		}

		static void SectionVerticalAlignmentHandler(RtfTag tag, RtfImportContext context)
		{
			VerticalAlignment? verticalAlignment = null;
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "vertalt"))
				{
					if (!(name == "vertalc"))
					{
						if (!(name == "vertalj"))
						{
							if (name == "vertalb" || name == "vertal")
							{
								verticalAlignment = new VerticalAlignment?(VerticalAlignment.Bottom);
							}
						}
						else
						{
							verticalAlignment = new VerticalAlignment?(VerticalAlignment.Justified);
						}
					}
					else
					{
						verticalAlignment = new VerticalAlignment?(VerticalAlignment.Center);
					}
				}
				else
				{
					verticalAlignment = new VerticalAlignment?(VerticalAlignment.Top);
				}
			}
			if (verticalAlignment != null)
			{
				context.CurrentStyle.SectionStyle.VerticalAlignment = verticalAlignment.Value;
			}
		}

		static void SectionStartingPageNumberHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "pgnstarts");
			context.CurrentStyle.SectionStyle.PageNumberingSettings.StartingPageNumber = new int?(tag.ValueAsNumber);
		}

		static void SectionPageNumberFormatHandler(RtfTag tag, RtfImportContext context)
		{
			NumberingStyle value;
			if (RtfHelper.NumberingStyleMapper.TryGetToValue(tag.Name, out value))
			{
				context.CurrentStyle.SectionStyle.PageNumberingSettings.PageNumberFormat = new NumberingStyle?(value);
			}
		}

		static void SectionChapterSeparatorCharacterHandler(RtfTag tag, RtfImportContext context)
		{
			ChapterSeparatorType value;
			if (RtfHelper.ChapterSeparatorTypeMapper.TryGetToValue(tag.Name, out value))
			{
				context.CurrentStyle.SectionStyle.PageNumberingSettings.ChapterSeparatorCharacter = new ChapterSeparatorType?(value);
			}
		}

		static void SectionChapterHeadingStyleHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "pgnhn");
			if (tag.ValueAsNumber != 0)
			{
				context.CurrentStyle.SectionStyle.PageNumberingSettings.ChapterHeadingStyleIndex = new int?(tag.ValueAsNumber);
			}
		}
	}
}
