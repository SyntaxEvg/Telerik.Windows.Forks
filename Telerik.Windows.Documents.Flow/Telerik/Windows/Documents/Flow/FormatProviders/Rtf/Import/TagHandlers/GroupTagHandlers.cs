using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Watermarks;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class GroupTagHandlers
	{
		public static void InitializeGroupHandlers(Dictionary<string, ControlGroupHandler> groupHandlers)
		{
			groupHandlers["fonttbl"] = new ControlGroupHandler(GroupTagHandlers.FontTableHandler);
			groupHandlers["colortbl"] = new ControlGroupHandler(GroupTagHandlers.ColorTableHandler);
			groupHandlers["generator"] = new ControlGroupHandler(GroupTagHandlers.GeneratorHandler);
			groupHandlers["defchp"] = new ControlGroupHandler(GroupTagHandlers.DefaultCharacterPropertiesHandler);
			groupHandlers["defpap"] = new ControlGroupHandler(GroupTagHandlers.DefaultParagraphGroupHandler);
			groupHandlers["stylesheet"] = new ControlGroupHandler(GroupTagHandlers.StylesTableHandler);
			groupHandlers["listtable"] = new ControlGroupHandler(GroupTagHandlers.ListTableHandler);
			groupHandlers["listoverridetable"] = new ControlGroupHandler(GroupTagHandlers.ListOverrideTableHandler);
			groupHandlers["themedata"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["colorschememapping"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["filetbl"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["pgptbl"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["revtbl"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["rsidtbl"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["protusertbl"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["pntext"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["info"] = new ControlGroupHandler(GroupTagHandlers.DocumentInfoGroupHandler);
			groupHandlers["pict"] = new ControlGroupHandler(GroupTagHandlers.ImageGroupHandler);
			groupHandlers["shp"] = new ControlGroupHandler(GroupTagHandlers.ShapeGroupHandler);
			groupHandlers["nonesttables"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["nonshppict"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["shppict"] = new ControlGroupHandler(GroupTagHandlers.VisitGroupHandler);
			groupHandlers["nesttableprops"] = new ControlGroupHandler(GroupTagHandlers.VisitGroupHandler);
			groupHandlers["listtext"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
			groupHandlers["bkmkstart"] = new ControlGroupHandler(GroupTagHandlers.BookmarkStartHandler);
			groupHandlers["bkmkend"] = new ControlGroupHandler(GroupTagHandlers.BookmarkEndHandler);
			groupHandlers["field"] = new ControlGroupHandler(GroupTagHandlers.VisitGroupHandler);
			groupHandlers["fldinst"] = new ControlGroupHandler(GroupTagHandlers.VisitGroupHandler);
			groupHandlers["fldrslt"] = new ControlGroupHandler(GroupTagHandlers.VisitGroupHandler);
			groupHandlers["atrfstart"] = new ControlGroupHandler(GroupTagHandlers.CommentStartHandler);
			groupHandlers["atrfend"] = new ControlGroupHandler(GroupTagHandlers.CommentEndHandler);
			groupHandlers["atnauthor"] = new ControlGroupHandler(GroupTagHandlers.CommentAuthorHandler);
			groupHandlers["annotation"] = new ControlGroupHandler(GroupTagHandlers.CommentDefinitionHandler);
			groupHandlers["atnid"] = new ControlGroupHandler(GroupTagHandlers.CommentIdHandler);
			groupHandlers["atndate"] = new ControlGroupHandler(GroupTagHandlers.CommentDateHandler);
			groupHandlers["docvar"] = new ControlGroupHandler(GroupTagHandlers.DocumentVariableHandler);
			groupHandlers["header"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["headerf"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["headerl"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["headerr"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["footer"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["footerf"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["footerl"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["footerr"] = new ControlGroupHandler(GroupTagHandlers.HeaderFooterHandler);
			groupHandlers["footnote"] = new ControlGroupHandler(GroupTagHandlers.IgnoreGroupHandler);
		}

		static bool IgnoreGroupHandler(RtfGroup group, RtfImportContext context)
		{
			return false;
		}

		static bool VisitGroupHandler(RtfGroup group, RtfImportContext context)
		{
			return true;
		}

		static bool FontTableHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "fonttbl");
			context.FontTable.FillFontTable(group);
			return false;
		}

		static bool ColorTableHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "colortbl");
			context.ColorTable.FillColorTable(group);
			return false;
		}

		static bool GeneratorHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "generator");
			if (group.Elements.Count == 3 && group.Elements[2] is RtfText)
			{
				context.Generator = ((RtfText)group.Elements[2]).Text;
			}
			return false;
		}

		static bool DocumentInfoGroupHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "info");
			context.DocumentInfo.FillDocumentInfo(group);
			return false;
		}

		static bool HeaderFooterHandler(RtfGroup group, RtfImportContext context)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e00-1 == null)
				{
					PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e00-1 = new Dictionary<string, int>(8)
					{
						{ "header", 0 },
						{ "headerr", 1 },
						{ "headerf", 2 },
						{ "headerl", 3 },
						{ "footer", 4 },
						{ "footerr", 5 },
						{ "footerf", 6 },
						{ "footerl", 7 }
					};
				}
				int num;
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e00-1.TryGetValue(destination, out num))
				{
					HeaderFooterBase partBlockContainer;
					switch (num)
					{
					case 0:
						partBlockContainer = context.CurrentSection.Headers.Add(HeaderFooterType.Default);
						break;
					case 1:
						partBlockContainer = context.CurrentSection.Headers.Add(HeaderFooterType.Default);
						break;
					case 2:
						partBlockContainer = context.CurrentSection.Headers.Add(HeaderFooterType.First);
						break;
					case 3:
						partBlockContainer = context.CurrentSection.Headers.Add(HeaderFooterType.Even);
						break;
					case 4:
						partBlockContainer = context.CurrentSection.Footers.Add(HeaderFooterType.Default);
						break;
					case 5:
						partBlockContainer = context.CurrentSection.Footers.Add(HeaderFooterType.Default);
						break;
					case 6:
						partBlockContainer = context.CurrentSection.Footers.Add(HeaderFooterType.First);
						break;
					case 7:
						partBlockContainer = context.CurrentSection.Footers.Add(HeaderFooterType.Even);
						break;
					default:
						goto IL_169;
					}
					RtfImportContext context2 = new RtfImportContext(context, partBlockContainer);
					RtfDocumentImporter rtfDocumentImporter = new RtfDocumentImporter(context2);
					rtfDocumentImporter.ImportRoot(group);
					return false;
				}
			}
			IL_169:
			throw new RtfUnexpectedElementException("Unexpected header/footer group.", group.Destination);
		}

		static bool DefaultCharacterPropertiesHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "defchp");
			context.StylesTable.ImportDefaultSpanStyle(group, context);
			return false;
		}

		static bool DefaultParagraphGroupHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "defpap");
			context.StylesTable.ImportDefaultParagraphStyle(group, context);
			return false;
		}

		static bool StylesTableHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "stylesheet");
			context.StylesTable.ReadTable(group, context);
			return false;
		}

		static bool ImageGroupHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "pict");
			ImageInline imageInline = context.ImageBuilder.ReadImage(group);
			if (imageInline != null)
			{
				context.AddInline(imageInline);
			}
			return false;
		}

		static bool ShapeGroupHandler(RtfGroup group, RtfImportContext context)
		{
			InlineBase inlineBase = null;
			Watermark watermark = null;
			context.ShapeBuilder.ReadShape(group, out inlineBase, out watermark);
			if (inlineBase != null)
			{
				context.AddInline(inlineBase);
			}
			if (watermark != null)
			{
				context.AddWatermark(watermark);
			}
			return false;
		}

		static bool BookmarkStartHandler(RtfGroup group, RtfImportContext context)
		{
			RtfText rtfText = group.Elements.FirstOrDefault((RtfElement e) => e is RtfText) as RtfText;
			if (rtfText != null && !context.Bookmarks.ContainsKey(rtfText.Text))
			{
				Bookmark bookmark = new Bookmark(context.Document, rtfText.Text);
				context.AddInline(bookmark.BookmarkRangeStart);
				context.Bookmarks[rtfText.Text] = bookmark;
			}
			return false;
		}

		static bool BookmarkEndHandler(RtfGroup group, RtfImportContext context)
		{
			RtfText rtfText = group.Elements.FirstOrDefault((RtfElement e) => e is RtfText) as RtfText;
			if (rtfText != null)
			{
				Bookmark bookmark = null;
				if (context.Bookmarks.TryGetValue(rtfText.Text, out bookmark))
				{
					context.AddInline(bookmark.BookmarkRangeEnd);
				}
			}
			return false;
		}

		static bool CommentStartHandler(RtfGroup group, RtfImportContext context)
		{
			RtfText rtfText = group.Elements.FirstOrDefault((RtfElement e) => e is RtfText) as RtfText;
			if (rtfText != null)
			{
				Comment comment = context.GetComment(rtfText.Text);
				CommentRangeStart commentRangeStart = comment.CommentRangeStart;
				if (commentRangeStart.Paragraph != null)
				{
					commentRangeStart.Paragraph.Inlines.Remove(commentRangeStart);
				}
				context.AddInline(comment.CommentRangeStart);
			}
			return false;
		}

		static bool CommentEndHandler(RtfGroup group, RtfImportContext context)
		{
			RtfText rtfText = group.Elements.FirstOrDefault((RtfElement e) => e is RtfText) as RtfText;
			if (rtfText != null)
			{
				Comment comment = context.GetComment(rtfText.Text);
				CommentRangeEnd commentRangeEnd = comment.CommentRangeEnd;
				if (commentRangeEnd.Paragraph != null)
				{
					commentRangeEnd.Paragraph.Inlines.Remove(commentRangeEnd);
				}
				context.AddInline(comment.CommentRangeEnd);
			}
			return false;
		}

		static bool CommentIdHandler(RtfGroup group, RtfImportContext context)
		{
			context.CurrentCommentHandler = new CommentImporter(context);
			context.CurrentCommentHandler.ImportCommentId(group);
			return false;
		}

		static bool CommentAuthorHandler(RtfGroup group, RtfImportContext context)
		{
			context.CurrentCommentHandler.ImportAuthor(group);
			return false;
		}

		static bool CommentDateHandler(RtfGroup group, RtfImportContext context)
		{
			context.CurrentCommentHandler.ImportDate(group);
			return false;
		}

		static bool CommentDefinitionHandler(RtfGroup group, RtfImportContext context)
		{
			if (context.CurrentCommentHandler == null)
			{
				context.CurrentCommentHandler = new CommentImporter(context);
			}
			CommentImporter currentCommentHandler = context.CurrentCommentHandler;
			currentCommentHandler.ImportCommentDefinition(group);
			context.CurrentCommentHandler = null;
			return false;
		}

		static bool ListTableHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "listtable");
			context.ListTable.ReadTable(group, context);
			return false;
		}

		static bool ListOverrideTableHandler(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "listoverridetable");
			context.ListOverrideTable.ReadTable(group, context);
			return false;
		}

		static bool DocumentVariableHandler(RtfGroup group, RtfImportContext context)
		{
			RtfGroup[] array = (from e in @group.Elements
				where e is RtfGroup
				select (RtfGroup)e).ToArray<RtfGroup>();
			if (array.Length >= 2)
			{
				string groupText = RtfHelper.GetGroupText(array[0], true);
				string groupText2 = RtfHelper.GetGroupText(array[1], true);
				if (!string.IsNullOrEmpty(groupText))
				{
					context.Document.DocumentVariables[groupText] = groupText2;
				}
			}
			return false;
		}
	}
}
