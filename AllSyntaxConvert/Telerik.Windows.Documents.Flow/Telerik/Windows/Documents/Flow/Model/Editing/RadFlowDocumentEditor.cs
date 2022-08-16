using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Collections;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Flow.TextSearch;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Editing
{
	public class RadFlowDocumentEditor
	{
		public RadFlowDocumentEditor(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
			this.position = new EditorPosition(this.document);
			this.characterFormatting = new Run(this.document);
			this.paragraphFormatting = new Paragraph(this.document);
			this.tableFormatting = new Table(this.document);
		}

		public CharacterProperties CharacterFormatting
		{
			get
			{
				return this.characterFormatting.Properties;
			}
		}

		public ParagraphProperties ParagraphFormatting
		{
			get
			{
				return this.paragraphFormatting.Properties;
			}
		}

		public TableProperties TableFormatting
		{
			get
			{
				return this.tableFormatting.Properties;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public Run InsertText(string text)
		{
			this.PreparePositionForInsertInline();
			Run run = null;
			string[] lines = TextHelper.GetLines(text);
			for (int i = 0; i < lines.Length; i++)
			{
				if (i > 0)
				{
					this.InsertParagraph();
				}
				string text2 = lines[i];
				if (!string.IsNullOrEmpty(text2))
				{
					string[] array = text2.Split(new string[] { SpecialSymbols.VerticalTabSymbol }, StringSplitOptions.None);
					for (int j = 0; j < array.Length; j++)
					{
						if (j > 0)
						{
							this.InsertBreak(BreakType.LineBreak);
						}
						string text3 = array[j];
						if (!string.IsNullOrEmpty(text3))
						{
							run = new Run(this.Document);
							this.ApplyCharacterFormatting(run);
							run.Text = text3;
							this.InsertInlineInternal(run);
						}
					}
				}
			}
			return run;
		}

		public Run InsertLine(string text)
		{
			Run result = this.InsertText(text);
			this.InsertParagraph();
			return result;
		}

		public Hyperlink InsertHyperlink(string text, string uri, bool isAnchor, string toolTip = null)
		{
			this.PreparePositionForInsertInline();
			Hyperlink hyperlink = new Hyperlink(this.Document);
			hyperlink.Uri = uri;
			hyperlink.IsAnchor = isAnchor;
			hyperlink.ToolTip = toolTip;
			FieldInfo fieldInfo = new FieldInfo(this.Document, hyperlink);
			this.InsertInlineInternal(fieldInfo.Start);
			this.InsertText(hyperlink.CreateHyperlinkCode());
			this.InsertInlineInternal(fieldInfo.Separator);
			if (string.IsNullOrEmpty(this.characterFormatting.StyleId))
			{
				if (!this.Document.StyleRepository.Contains("Hyperlink"))
				{
					this.Document.StyleRepository.AddBuiltInStyle("Hyperlink");
				}
				this.characterFormatting.StyleId = "Hyperlink";
				this.InsertText(text);
				this.characterFormatting.StyleId = string.Empty;
			}
			else
			{
				this.InsertText(text);
			}
			this.InsertInlineInternal(fieldInfo.End);
			return hyperlink;
		}

		public FieldInfo InsertField(string code, string result)
		{
			this.PreparePositionForInsertInline();
			FieldInfo fieldInfo = new FieldInfo(this.Document);
			this.InsertInlineInternal(fieldInfo.Start);
			if (!string.IsNullOrEmpty(code))
			{
				this.InsertText(code);
			}
			this.InsertInlineInternal(fieldInfo.Separator);
			if (!string.IsNullOrEmpty(result))
			{
				this.InsertText(result);
			}
			this.InsertInlineInternal(fieldInfo.End);
			fieldInfo.LoadFieldFromCode();
			return fieldInfo;
		}

		public Section InsertSection()
		{
			Section section = new Section(this.Document);
			this.InsertSectionInternal(section);
			return section;
		}

		public Paragraph InsertParagraph()
		{
			Paragraph paragraph = new Paragraph(this.Document);
			this.InsertParagraphInternal(paragraph, true);
			return paragraph;
		}

		public ImageInline InsertImageInline(Stream stream, string extension)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			return this.InsertImageInline(new ImageSource(stream, extension), ImageSizeDecodersManager.GetSize(stream, extension));
		}

		public ImageInline InsertImageInline(ImageSource source, Size size)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			this.PreparePositionForInsertInline();
			ImageInline imageInline = new ImageInline(this.Document);
			imageInline.Image.ImageSource = source;
			imageInline.Image.Size = size;
			this.InsertInlineInternal(imageInline);
			return imageInline;
		}

		public ImageInline InsertImageInline(Stream stream, string extension, Size size)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			return this.InsertImageInline(new ImageSource(stream, extension), size);
		}

		public FloatingImage InsertFloatingImage(Stream stream, string extension)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			return this.InsertFloatingImage(new ImageSource(stream, extension), ImageSizeDecodersManager.GetSize(stream, extension));
		}

		public FloatingImage InsertFloatingImage(Stream stream, string extension, Size size)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNullOrEmpty(extension, "extension");
			return this.InsertFloatingImage(new ImageSource(stream, extension), size);
		}

		public FloatingImage InsertFloatingImage(ImageSource source, Size size)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			this.PreparePositionForInsertInline();
			FloatingImage floatingImage = new FloatingImage(this.Document);
			floatingImage.Image.ImageSource = source;
			floatingImage.Image.Size = size;
			this.InsertInlineInternal(floatingImage);
			return floatingImage;
		}

		public InlineBase InsertInline(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			this.PreparePositionForInsertInline();
			this.InsertInlineInternal(inline);
			return inline;
		}

		public Table InsertTable()
		{
			return this.InsertTable(0, 0);
		}

		public Table InsertTable(int rows, int columns)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, int.MaxValue, rows, "rows");
			Guard.ThrowExceptionIfOutOfRange<int>(0, int.MaxValue, columns, "columns");
			Table table = new Table(this.Document, rows, columns);
			this.InsertTableInternal(table, true);
			return table;
		}

		public Break InsertBreak(BreakType type)
		{
			Break br = new Break(this.Document);
			br.BreakType = type;
			if (type == BreakType.LineBreak || !this.position.IsInTableCell())
			{
				this.InsertInline(br);
				if (type == BreakType.PageBreak)
				{
					this.InsertParagraph();
				}
			}
			else
			{
				Section parentContainer = this.position.CurrentBlock.BlockContainer.EnumerateParentsOfType<Section>().FirstOrDefault<Section>();
				TableCell currentPositionCell = this.position.CurrentBlock.Parent.EnumerateParentsOfType<TableCell>().Last<TableCell>();
				this.InsertBreakRespectiveTable(currentPositionCell, parentContainer, delegate(Paragraph p)
				{
					p.Inlines.Add(br);
				});
			}
			return br;
		}

		public Comment InsertComment(string text)
		{
			Comment comment = this.Document.Comments.AddComment();
			comment.Blocks.AddParagraph().Inlines.AddRun(text);
			this.InsertInline(comment.CommentRangeStart);
			this.InsertInline(comment.CommentRangeEnd);
			return comment;
		}

		public Comment InsertComment(string text, InlineBase inlineStart, InlineBase inlineEnd)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inlineStart, "inlineStart");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineEnd, "inlineEnd");
			Comment comment = this.Document.Comments.AddComment();
			comment.Blocks.AddParagraph().Inlines.AddRun(text);
			return this.InsertComment(comment, inlineStart, inlineEnd);
		}

		public Comment InsertComment(Comment comment)
		{
			Guard.ThrowExceptionIfNull<Comment>(comment, "comment");
			Guard.ThrowExceptionIfFalse(comment.Document == this.document, "Comment is from another document.");
			this.InsertInline(comment.CommentRangeStart);
			this.InsertInline(comment.CommentRangeEnd);
			return comment;
		}

		public Comment InsertComment(Comment comment, InlineBase inlineStart, InlineBase inlineEnd)
		{
			Guard.ThrowExceptionIfNull<Comment>(comment, "comment");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineStart, "inlineStart");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineEnd, "inlineEnd");
			Guard.ThrowExceptionIfFalse(comment.Document == this.document, "Comment is from another document.");
			this.MoveToInlineStart(inlineStart);
			this.InsertInline(comment.CommentRangeStart);
			this.MoveToInlineEnd(inlineEnd);
			this.InsertInline(comment.CommentRangeEnd);
			return comment;
		}

		public Bookmark InsertBookmark(string name)
		{
			if (this.Document.GetBookmarkByName(name) != null)
			{
				throw new InvalidOperationException(string.Format("A Bookmark with name '{0}' already exists.", name));
			}
			Bookmark bookmark = new Bookmark(this.Document, name);
			this.InsertInline(bookmark.BookmarkRangeStart);
			this.InsertInline(bookmark.BookmarkRangeEnd);
			return bookmark;
		}

		public Bookmark InsertBookmark(string name, InlineBase inlineStart, InlineBase inlineEnd)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inlineStart, "inlineStart");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineEnd, "inlineEnd");
			if (this.Document.EnumerateChildrenOfType<BookmarkRangeStart>().Any((BookmarkRangeStart b) => b.Bookmark.Name == name))
			{
				throw new InvalidOperationException(string.Format("A Bookmark with name '{0}' already exists.", name));
			}
			Bookmark bookmark = new Bookmark(this.Document, name);
			this.MoveToInlineStart(inlineStart);
			this.InsertInline(bookmark.BookmarkRangeStart);
			this.MoveToInlineEnd(inlineEnd);
			this.InsertInline(bookmark.BookmarkRangeEnd);
			return bookmark;
		}

		public void InsertDocument(RadFlowDocument sourceDocument)
		{
			this.InsertDocument(sourceDocument, null);
		}

		public void InsertDocument(RadFlowDocument sourceDocument, InsertDocumentOptions insertOptions)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(sourceDocument, "sourceDocument");
			if (sourceDocument.Sections.Count == 0)
			{
				throw new ArgumentException("Source document does not contain any sections.");
			}
			if (this.position.IsInTableCell() && sourceDocument.Sections.Count > 1)
			{
				throw new InvalidOperationException("The source document cannot be inserted at the current position.");
			}
			RadFlowDocumentEditor.InsertDocumentEditor insertDocumentEditor = new RadFlowDocumentEditor.InsertDocumentEditor(this);
			insertDocumentEditor.InsertDocument(sourceDocument, insertOptions);
		}

		public void DeleteBookmark(string name)
		{
			BookmarkRangeStart bookmarkRangeStart = (from b in this.Document.EnumerateChildrenOfType<BookmarkRangeStart>()
				where b.Bookmark.Name == name
				select b).FirstOrDefault<BookmarkRangeStart>();
			if (bookmarkRangeStart == null)
			{
				throw new InvalidOperationException(string.Format("A Bookmark with name '{0}' does not exist.", name));
			}
			this.DeleteBookmark(bookmarkRangeStart.Bookmark);
		}

		public void DeleteBookmark(Bookmark bookmark)
		{
			Guard.ThrowExceptionIfFalse(bookmark.Document == this.document, "Bookmark is from another document.");
			BookmarkRangeStart bookmarkRangeStart = bookmark.BookmarkRangeStart;
			BookmarkRangeEnd bookmarkRangeEnd = bookmark.BookmarkRangeEnd;
			if (bookmarkRangeStart.Paragraph == null || bookmarkRangeEnd.Paragraph == null)
			{
				throw new InvalidOperationException("Bookmark is not part of the document.");
			}
			bookmarkRangeStart.Paragraph.Inlines.Remove(bookmarkRangeStart);
			bookmarkRangeEnd.Paragraph.Inlines.Remove(bookmarkRangeEnd);
		}

		public PermissionRange InsertPermissionRange(PermissionRangeCredentials permissionRangeCredentials, InlineBase inlineStart, InlineBase inlineEnd)
		{
			Guard.ThrowExceptionIfNull<PermissionRangeCredentials>(permissionRangeCredentials, "permissionRangeCredentials");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineStart, "inlineStart");
			Guard.ThrowExceptionIfNull<InlineBase>(inlineEnd, "inlineEnd");
			PermissionRange permissionRange = new PermissionRange(this.Document, permissionRangeCredentials);
			this.MoveToInlineStart(inlineStart);
			this.InsertInline(permissionRange.Start);
			this.MoveToInlineEnd(inlineEnd);
			this.InsertInline(permissionRange.End);
			return permissionRange;
		}

		public PermissionRange InsertPermissionRange(PermissionRangeCredentials permissionRangeCredentials, TableCell cell)
		{
			Guard.ThrowExceptionIfNull<PermissionRangeCredentials>(permissionRangeCredentials, "permissionRangeCredentials");
			Guard.ThrowExceptionIfNull<TableCell>(cell, "cell");
			Guard.ThrowExceptionIfNull<TableRow>(cell.Row, "cell.Row");
			Guard.ThrowExceptionIfNull<Table>(cell.Row.Table, "cell.Row.Table");
			int gridColumnIndex = cell.GridColumnIndex;
			int value = cell.GridColumnIndex + cell.ColumnSpan - 1;
			PermissionRange permissionRange = new PermissionRange(this.Document, permissionRangeCredentials, new int?(gridColumnIndex), new int?(value));
			Paragraph paragraph = cell.EnumerateChildrenOfType<Paragraph>().FirstOrDefault<Paragraph>();
			if (paragraph == null)
			{
				paragraph = cell.Blocks.AddParagraph();
			}
			this.position.MoveToParagraphStart(paragraph);
			this.InsertInline(permissionRange.Start);
			Paragraph paragraphFromNextBlockElement = RadFlowDocumentEditor.GetParagraphFromNextBlockElement(cell);
			if (paragraphFromNextBlockElement != null)
			{
				this.position.MoveToParagraphStart(paragraphFromNextBlockElement);
			}
			this.InsertInline(permissionRange.End);
			return permissionRange;
		}

		public PermissionRange InsertPermissionRange(PermissionRangeCredentials permissionRangeCredentials, TableRow row)
		{
			Guard.ThrowExceptionIfNull<PermissionRangeCredentials>(permissionRangeCredentials, "permissionRangeCredentials");
			Guard.ThrowExceptionIfNull<TableRow>(row, "row");
			Guard.ThrowExceptionIfLessThanOrEqual<int>(0, row.Cells.Count, "row.Cells.Count");
			Guard.ThrowExceptionIfNull<Table>(row.Table, "row.Table");
			TableCell tableCell = row.Cells.First<TableCell>();
			TableCell tableCell2 = row.Cells.Last<TableCell>();
			int gridColumnIndex = tableCell.GridColumnIndex;
			int value = tableCell2.GridColumnIndex + tableCell2.ColumnSpan - 1;
			PermissionRange permissionRange = new PermissionRange(this.Document, permissionRangeCredentials, new int?(gridColumnIndex), new int?(value));
			Paragraph paragraph = tableCell.EnumerateChildrenOfType<Paragraph>().FirstOrDefault<Paragraph>();
			if (paragraph == null)
			{
				paragraph = tableCell.Blocks.AddParagraph();
			}
			this.position.MoveToParagraphStart(paragraph);
			this.InsertInline(permissionRange.Start);
			Paragraph paragraphFromNextBlockElement = RadFlowDocumentEditor.GetParagraphFromNextBlockElement(tableCell2);
			if (paragraphFromNextBlockElement != null)
			{
				this.position.MoveToParagraphStart(paragraphFromNextBlockElement);
			}
			this.InsertInline(permissionRange.End);
			return permissionRange;
		}

		public void DeletePermissionRange(PermissionRange permission)
		{
			Guard.ThrowExceptionIfFalse(permission.Document == this.document, "Permission is from another document.");
			PermissionRangeStart start = permission.Start;
			PermissionRangeEnd end = permission.End;
			if (start.Paragraph == null || end.Paragraph == null)
			{
				throw new InvalidOperationException("Bookmark is not part of the document.");
			}
			start.Paragraph.Inlines.Remove(start);
			end.Paragraph.Inlines.Remove(end);
		}

		public void Protect(string password)
		{
			this.Protect(password, ProtectionMode.ReadOnly);
		}

		public void Protect(string password, ProtectionMode protectionMode)
		{
			if (this.Document.ProtectionSettings.Enforced)
			{
				throw new InvalidOperationException("Cannot protect a document that is already protected.");
			}
			if (!string.IsNullOrEmpty(password))
			{
				if (string.IsNullOrEmpty(this.Document.ProtectionSettings.Salt))
				{
					this.Document.ProtectionSettings.Salt = FlowProtectionHelper.Instance.GenerateSaltBase64();
				}
				string hash = FlowProtectionHelper.Instance.GenerateHashBase64(password, this.Document.ProtectionSettings.Salt, this.Document.ProtectionSettings.AlgorithmName, this.Document.ProtectionSettings.SpinCount);
				this.Document.ProtectionSettings.Hash = hash;
			}
			this.Document.ProtectionSettings.Enforced = true;
			this.Document.ProtectionSettings.ProtectionMode = protectionMode;
		}

		public bool Unprotect(string password)
		{
			bool flag = FlowProtectionHelper.Instance.IsPasswordCorrect(password, this.Document.ProtectionSettings.Hash, this.Document.ProtectionSettings.Salt, this.Document.ProtectionSettings.AlgorithmName, this.Document.ProtectionSettings.SpinCount);
			if (flag)
			{
				this.Unprotect();
			}
			return flag;
		}

		public void Unprotect()
		{
			this.Document.ProtectionSettings.Enforced = false;
			this.Document.ProtectionSettings.ProtectionMode = ProtectionMode.NoProtection;
			this.Document.ProtectionSettings.Hash = string.Empty;
			this.Document.ProtectionSettings.Salt = string.Empty;
		}

		public void ReplaceText(string oldText, string newText)
		{
			this.ReplaceText(oldText, newText, true, false);
		}

		public void ReplaceText(string oldText, string newText, bool matchCase, bool matchWholeWord)
		{
			Guard.ThrowExceptionIfNullOrEmpty(oldText, "oldText");
			this.ReplaceText(FindTextService.BuildRegex(oldText, matchCase, matchWholeWord), newText);
		}

		public void ReplaceText(Regex regex, string newText)
		{
			Guard.ThrowExceptionIfNull<Regex>(regex, "regex");
			IEnumerable<RunTextMatch> matches = FindReplaceManager.Find(regex, this.Document);
			FindReplaceManager.Replace(matches, regex, newText, new ReplaceTextHandler());
		}

		public void ReplaceStyling(string searchedText, Action<CharacterProperties> replacePropertiesAction)
		{
			this.ReplaceStyling(searchedText, true, false, replacePropertiesAction);
		}

		public void ReplaceStyling(string searchedText, bool matchCase, bool matchWholeWord, Action<CharacterProperties> replacePropertiesAction)
		{
			Guard.ThrowExceptionIfNullOrEmpty(searchedText, "searchedText");
			this.ReplaceStyling(FindTextService.BuildRegex(searchedText, matchCase, matchWholeWord), replacePropertiesAction);
		}

		public void ReplaceStyling(Regex regex, Action<CharacterProperties> replacePropertiesAction)
		{
			Guard.ThrowExceptionIfNull<Action<CharacterProperties>>(replacePropertiesAction, "replacePropertiesAction");
			IEnumerable<RunTextMatch> matches = FindReplaceManager.Find(regex, this.Document);
			FindReplaceManager.Replace(matches, regex, string.Empty, new ReplacePropertiesHandler(replacePropertiesAction));
		}

		public void SetWatermark(Watermark watermark, Header header)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			Guard.ThrowExceptionIfNull<Header>(header, "header");
			Guard.ThrowExceptionIfNotNull<RadFlowDocument>(watermark.Document, "Document");
			header.Watermarks.Clear();
			header.Watermarks.Add(watermark);
		}

		public void SetWatermark(Watermark watermark, Section section, HeaderFooterType headerType)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			Guard.ThrowExceptionIfNull<Section>(section, "section");
			Header header;
			switch (headerType)
			{
			case HeaderFooterType.Default:
				if (section.Headers.Default == null)
				{
					section.Headers.Add(HeaderFooterType.Default);
				}
				header = section.Headers.Default;
				break;
			case HeaderFooterType.Even:
				if (section.Headers.Even == null)
				{
					section.Headers.Add(HeaderFooterType.Even);
				}
				header = section.Headers.Even;
				break;
			case HeaderFooterType.First:
				if (section.Headers.First == null)
				{
					section.Headers.Add(HeaderFooterType.First);
				}
				header = section.Headers.First;
				break;
			default:
				throw new NotSupportedException("Unsupported header type.");
			}
			this.SetWatermark(watermark, header);
		}

		public void MoveToParagraphStart(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfFalse(paragraph.Document == this.document, "document");
			this.position.MoveToParagraphStart(paragraph);
		}

		public void MoveToInlineEnd(InlineBase inline)
		{
			Guard.ThrowExceptionIfFalse(inline.Document == this.document, "document");
			this.position.MoveToInlineEnd(inline);
		}

		public void MoveToInlineStart(InlineBase inline)
		{
			Guard.ThrowExceptionIfFalse(inline.Document == this.document, "document");
			this.position.MoveToInlineStart(inline);
		}

		public void MoveToTableEnd(Table table)
		{
			Guard.ThrowExceptionIfFalse(table.Document == this.document, "document");
			this.position.MoveAfterTable(table);
		}

		static Table ClonePartOfTable(Table tableToClone, int fromRowIndex, int toRowIndex)
		{
			Table table = new Table(tableToClone.Document);
			table.Properties.CopyPropertiesFrom(tableToClone.Properties);
			for (int i = fromRowIndex; i < toRowIndex; i++)
			{
				table.Rows.Add(tableToClone.Rows[i].Clone());
			}
			return table;
		}

		static Paragraph GetParagraphFromNextBlockElement(TableCell currentCell)
		{
			TableRow row = currentCell.Row;
			Table table = row.Table;
			Paragraph paragraph = null;
			TableCell tableCell = null;
			for (int i = 1; i < row.Cells.Count; i++)
			{
				if (row.Cells[i - 1] == currentCell)
				{
					tableCell = row.Cells[i];
					break;
				}
			}
			if (tableCell == null)
			{
				TableRow tableRow = null;
				for (int j = 1; j < table.Rows.Count; j++)
				{
					if (table.Rows[j - 1] == row)
					{
						tableRow = table.Rows[j];
						break;
					}
				}
				if (tableRow != null)
				{
					tableCell = tableRow.Cells.FirstOrDefault<TableCell>();
					if (tableCell == null)
					{
						tableCell = tableRow.Cells.AddTableCell();
					}
				}
			}
			if (tableCell != null)
			{
				paragraph = tableCell.EnumerateChildrenOfType<Paragraph>().FirstOrDefault<Paragraph>();
				if (paragraph == null)
				{
					paragraph = tableCell.Blocks.AddParagraph();
				}
			}
			if (paragraph == null)
			{
				BlockContainerBase blockContainerBase = table.Parent as BlockContainerBase;
				if (blockContainerBase != null)
				{
					for (int k = 1; k < blockContainerBase.Blocks.Count; k++)
					{
						if (blockContainerBase.Blocks[k - 1] == table)
						{
							paragraph = blockContainerBase.Blocks[k] as Paragraph;
							break;
						}
					}
				}
			}
			return paragraph;
		}

		void InsertInlineInternal(InlineBase run)
		{
			Guard.ThrowExceptionIfFalse(this.position.IsInitialized, "Editor position is not initialized.");
			Guard.ThrowExceptionIfFalse(this.position.CurrentParagraph != null, "Editor position is not correct.");
			this.position.CurrentParagraph.Inlines.Insert(this.position.InlineIndex, run);
			this.position.InlineIndex++;
		}

		void InsertTableInternal(Table table, bool applyCurrentFormatting = true)
		{
			int index = 0;
			BlockContainerBase blockContainerBase;
			if (!this.position.IsInitialized)
			{
				blockContainerBase = this.AssureSection();
			}
			else
			{
				blockContainerBase = this.position.CurrentBlock.BlockContainer;
				if (this.position.CurrentParagraph != null && !this.position.IsAtParagraphEnd())
				{
					Paragraph item = this.InsertParagraph();
					index = blockContainerBase.Blocks.IndexOf(item);
				}
				else
				{
					index = blockContainerBase.Blocks.IndexOf(this.position.CurrentBlock) + 1;
				}
			}
			if (applyCurrentFormatting)
			{
				this.ApplyTableFormatting(table);
			}
			blockContainerBase.Blocks.Insert(index, table);
			this.position.MoveAfterTable(table);
		}

		void InsertParagraphInternal(Paragraph paragraph, bool applyCurrentFormatting = true)
		{
			BlockContainerBase blockContainerBase = null;
			if (!this.position.IsInitialized)
			{
				blockContainerBase = this.AssureSection();
			}
			if (applyCurrentFormatting)
			{
				this.ApplyParagraphFormatting(paragraph);
			}
			if (blockContainerBase != null)
			{
				blockContainerBase.Blocks.Insert(0, paragraph);
				this.position.MoveToParagraphStart(paragraph);
			}
			else
			{
				BlockBase currentBlock = this.position.CurrentBlock;
				blockContainerBase = currentBlock.BlockContainer;
				int index = blockContainerBase.Blocks.IndexOf(currentBlock) + 1;
				blockContainerBase.Blocks.Insert(index, paragraph);
			}
			Paragraph currentParagraph = this.position.CurrentParagraph;
			if (currentParagraph != null)
			{
				int inlineIndex = this.position.InlineIndex;
				while (currentParagraph.Inlines.Count > inlineIndex)
				{
					InlineBase item = currentParagraph.Inlines.Last<InlineBase>();
					currentParagraph.Inlines.RemoveAt(currentParagraph.Inlines.Count - 1);
					paragraph.Inlines.Insert(0, item);
				}
			}
			this.position.MoveToParagraphStart(paragraph);
		}

		void InsertSectionInternal(Section section)
		{
			this.Document.Sections.Add(section);
			this.SetPositionAfterSection(section);
		}

		void SetPositionAfterSection(Section section)
		{
			if (this.position.IsInitialized)
			{
				if (!this.position.IsInTableCell())
				{
					if (this.position.IsAtTableEnd)
					{
						this.InsertParagraph();
					}
					this.position.CurrentParagraph.SetParent(section);
					this.InsertParagraph();
					return;
				}
				TableCell currentPositionCell = this.position.CurrentBlock.Parent.EnumerateParentsOfType<TableCell>().Last<TableCell>();
				Action<Paragraph> insertAction = delegate(Paragraph p)
				{
					this.position.MoveToParagraphStart(p);
					this.position.CurrentParagraph.SetParent(section);
				};
				this.InsertBreakRespectiveTable(currentPositionCell, section, insertAction);
			}
		}

		void PreparePositionForInsertInline()
		{
			this.AssurePositionInitialized();
			if (this.position.IsAtTableEnd)
			{
				Table currentTable = this.position.CurrentTable;
				BlockContainerBase blockContainer = currentTable.BlockContainer;
				int num = blockContainer.Blocks.IndexOf(currentTable) + 1;
				Paragraph paragraph;
				if (num < blockContainer.Blocks.Count && blockContainer.Blocks[num] is Paragraph)
				{
					paragraph = (Paragraph)blockContainer.Blocks[num];
				}
				else
				{
					paragraph = this.InsertParagraph();
				}
				this.position.MoveToParagraphStart(paragraph);
			}
		}

		void AssurePositionInitialized()
		{
			if (!this.position.IsInitialized)
			{
				Section section = this.AssureSection();
				Paragraph paragraph = section.Blocks.FirstOrDefault<BlockBase>() as Paragraph;
				if (paragraph == null)
				{
					paragraph = new Paragraph(this.Document);
					this.ApplyParagraphFormatting(paragraph);
					section.Blocks.Insert(0, paragraph);
				}
				this.position.MoveToParagraphStart(paragraph);
			}
		}

		Section AssureSection()
		{
			Section section = this.Document.Sections.FirstOrDefault<Section>();
			if (section == null)
			{
				section = this.Document.Sections.AddSection();
			}
			return section;
		}

		void ApplyCharacterFormatting(Run run)
		{
			run.Properties.CopyPropertiesFrom(this.characterFormatting.Properties);
		}

		void ApplyParagraphFormatting(Paragraph paragraph)
		{
			paragraph.Properties.CopyPropertiesFrom(this.paragraphFormatting.Properties);
		}

		void ApplyTableFormatting(Table table)
		{
			table.Properties.CopyPropertiesFrom(this.tableFormatting.Properties);
		}

		void InsertBreakRespectiveTable(TableCell currentPositionCell, BlockContainerBase parentContainer, Action<Paragraph> insertAction)
		{
			Table table = currentPositionCell.Table;
			if (table == null || parentContainer == null)
			{
				return;
			}
			BlockContainerBase blockContainer = table.BlockContainer;
			Paragraph paragraph = new Paragraph(this.Document);
			if (currentPositionCell.GridRowIndex != 0)
			{
				Table item = RadFlowDocumentEditor.ClonePartOfTable(table, 0, currentPositionCell.GridRowIndex);
				Table table2 = RadFlowDocumentEditor.ClonePartOfTable(table, currentPositionCell.GridRowIndex, table.GridRowsCount);
				blockContainer.Blocks.Insert(blockContainer.Blocks.IndexOf(table) + 1, item);
				blockContainer.Blocks.Remove(table);
				blockContainer.Blocks.Add(paragraph);
				insertAction(paragraph);
				parentContainer.Blocks.Add(table2);
				this.position.MoveAfterTable(table2);
				return;
			}
			blockContainer.Blocks.Insert(blockContainer.Blocks.IndexOf(table) - 1, paragraph);
			blockContainer.Blocks.Remove(table);
			insertAction(paragraph);
			parentContainer.Blocks.Add(table);
			this.position.MoveAfterTable(table);
		}

		readonly RadFlowDocument document;

		readonly EditorPosition position;

		readonly Run characterFormatting;

		readonly Paragraph paragraphFormatting;

		readonly Table tableFormatting;

		class InsertDocumentEditor
		{
			public InsertDocumentEditor(RadFlowDocumentEditor editor)
			{
				this.editor = editor;
			}

			ParagraphProperties FormattingOfInsertTargetParagraph
			{
				get
				{
					return this.insertTargetParagraphFormatting.Properties;
				}
			}

			public void InsertDocument(RadFlowDocument sourceDocument, InsertDocumentOptions importOptions)
			{
				bool isTargetDocumentEmpty = RadFlowDocumentEditor.InsertDocumentEditor.IsDocumentEmpty(this.editor.Document);
				this.editor.AssurePositionInitialized();
				InsertDocumentContext insertContext = new InsertDocumentContext(this.editor.Document, sourceDocument, importOptions ?? new InsertDocumentOptions());
				this.BeginInsertDocument(sourceDocument, insertContext);
				this.InsertDocumentInternal(sourceDocument, insertContext, isTargetDocumentEmpty);
				this.EndInsertDocument();
			}

			static void SwapSectionPropertiesAndHeaderFooters(Section first, Section second, CloneContext cloneContext)
			{
				Section section = first.ClonePropertiesAndHeadersFooters(cloneContext);
				first.Properties.CopyPropertiesFrom(second.Properties);
				first.Headers.CloneHeadersFootersFrom(second.Headers, cloneContext);
				first.Footers.CloneHeadersFootersFrom(second.Footers, cloneContext);
				second.Properties.CopyPropertiesFrom(section.Properties);
				second.Headers.CloneHeadersFootersFrom(section.Headers, cloneContext);
				second.Footers.CloneHeadersFootersFrom(section.Footers, cloneContext);
			}

			static bool IsDocumentEmpty(RadFlowDocument document)
			{
				return document.Sections.Count == 0;
			}

			static bool ShouldInsertParagraph(Paragraph paragraph, InsertDocumentContext insertContext, BlockCollection blocks, int insertedBlocksCount)
			{
				bool flag = paragraph != insertContext.LastParagraphMarker;
				if (flag)
				{
					bool flag2 = blocks.Count > insertedBlocksCount + 1 && blocks[insertedBlocksCount + 1].Type == DocumentElementType.Table;
					return blocks.Count == 1 || !flag2;
				}
				return false;
			}

			static DocumentElementBase CloneElement(DocumentElementBase block, InsertDocumentContext insertContext)
			{
				DocumentElementBase documentElementBase = block.CloneCore(insertContext.CloneContext);
				if (documentElementBase != null)
				{
					documentElementBase.OnAfterCloneCore(insertContext.CloneContext, documentElementBase);
				}
				return documentElementBase;
			}

			void BeginInsertDocument(RadFlowDocument sourceDocument, InsertDocumentContext insertContext)
			{
				this.insertTargetParagraphFormatting = new Paragraph(this.editor.Document);
				this.clonedSectionIndex = 0;
				this.insertTargetSection = this.editor.position.CurrentBlock.EnumerateParentsOfType<Section>().FirstOrDefault<Section>();
				if (!insertContext.InsertDocumentOptions.InsertLastParagraphMarker)
				{
					insertContext.LastParagraphMarker = sourceDocument.Sections.Last<Section>().EnumerateContentChildrenOfType<Paragraph>().LastOrDefault<Paragraph>();
				}
				if (this.editor.position.CurrentParagraph != null)
				{
					this.isInsertPositionInParagraph = true;
					this.FormattingOfInsertTargetParagraph.CopyPropertiesFrom(this.editor.position.CurrentParagraph.Properties);
				}
				this.initialInsertSectionIndex = this.editor.Document.Sections.IndexOf(this.insertTargetSection) + 1;
			}

			void InsertDocumentInternal(RadFlowDocument sourceDocument, InsertDocumentContext insertContext, bool isTargetDocumentEmpty)
			{
				Section first = this.insertTargetSection;
				foreach (Section section in sourceDocument.Sections)
				{
					this.hasInsertedMultipleParagraphs |= this.InsertBlocksCloned(section.Blocks, insertContext);
					if (isTargetDocumentEmpty || this.clonedSectionIndex != sourceDocument.Sections.Count - 1)
					{
						Section section2 = section.ClonePropertiesAndHeadersFooters(insertContext.CloneContext);
						RadFlowDocumentEditor.InsertDocumentEditor.SwapSectionPropertiesAndHeaderFooters(first, section2, insertContext.CloneContext);
						first = section2;
						if ((this.clonedSectionIndex > 0 || sourceDocument.Sections.Count > 1) && this.clonedSectionIndex != sourceDocument.Sections.Count - 1)
						{
							int num = this.initialInsertSectionIndex + this.clonedSectionIndex;
							if (num >= this.editor.Document.Sections.Count)
							{
								this.editor.Document.Sections.Add(section2);
							}
							else
							{
								this.editor.Document.Sections.Insert(num, section2);
							}
							this.editor.SetPositionAfterSection(section2);
						}
					}
					this.clonedSectionIndex++;
				}
			}

			void EndInsertDocument()
			{
				if (this.hasInsertedMultipleParagraphs && this.isInsertPositionInParagraph)
				{
					this.OverrideCurrentParagraphProperties(this.FormattingOfInsertTargetParagraph);
				}
			}

			bool InsertBlocksCloned(BlockCollection blocks, InsertDocumentContext insertContext)
			{
				bool result = false;
				int num = 0;
				foreach (BlockBase blockBase in blocks)
				{
					switch (blockBase.Type)
					{
					case DocumentElementType.Paragraph:
					{
						this.editor.PreparePositionForInsertInline();
						Paragraph paragraph = (Paragraph)blockBase;
						foreach (InlineBase block in paragraph.Inlines)
						{
							DocumentElementBase documentElementBase = RadFlowDocumentEditor.InsertDocumentEditor.CloneElement(block, insertContext);
							this.editor.InsertInlineInternal((InlineBase)documentElementBase);
						}
						Paragraph paragraph2 = paragraph.CloneWithoutChildren(insertContext.CloneContext);
						this.OverrideCurrentParagraphProperties(paragraph2.Properties);
						if (RadFlowDocumentEditor.InsertDocumentEditor.ShouldInsertParagraph(paragraph, insertContext, blocks, num))
						{
							this.editor.InsertParagraphInternal(paragraph2, false);
							result = true;
						}
						num++;
						break;
					}
					case DocumentElementType.Table:
					{
						DocumentElementBase documentElementBase2 = RadFlowDocumentEditor.InsertDocumentEditor.CloneElement(blockBase, insertContext);
						this.editor.InsertTableInternal((Table)documentElementBase2, false);
						num++;
						break;
					}
					}
				}
				return result;
			}

			void OverrideCurrentParagraphProperties(ParagraphProperties properties)
			{
				Paragraph currentParagraph = this.editor.position.CurrentParagraph;
				if (currentParagraph != null)
				{
					currentParagraph.Properties.ClearLocalValues();
					currentParagraph.Properties.CopyPropertiesFrom(properties);
				}
			}

			readonly RadFlowDocumentEditor editor;

			Paragraph insertTargetParagraphFormatting;

			Section insertTargetSection;

			bool isInsertPositionInParagraph;

			bool hasInsertedMultipleParagraphs;

			int clonedSectionIndex;

			int initialInsertSectionIndex;
		}
	}
}
