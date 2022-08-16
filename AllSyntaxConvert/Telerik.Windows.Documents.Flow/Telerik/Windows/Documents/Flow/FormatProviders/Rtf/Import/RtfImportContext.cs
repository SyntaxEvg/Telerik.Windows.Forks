using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Fonts;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfImportContext
	{
		public RtfImportContext()
		{
			this.Document = new RadFlowDocument();
			this.FontTable = new RtfFontTable();
			this.ColorTable = new RtfColorTable();
			this.DocumentInfo = new RtfDocumentInfo();
			this.ImageBuilder = new RtfImageBuilder(this);
			this.ShapeBuilder = new RtfShapeBuilder(this);
			this.ListTable = new RtfListTable();
			this.ListOverrideTable = new RtfListOverrideTable();
			this.Comments = new Dictionary<string, Comment>();
			this.Bookmarks = new Dictionary<string, Bookmark>();
			this.StylesTable = new RtfStylesTable();
			this.styleStack = new Stack<RtfStyle>();
			this.styleStack.Push(new RtfStyle(this.Document));
			this.CurrentSection = this.Document.Sections.AddSection();
			this.structureStack = new Stack<RtfStructureStackItem>();
			this.FieldContext = new FieldContext(true);
		}

		public RtfImportContext(RtfImportContext other, BlockContainerBase partBlockContainer)
			: this(other)
		{
			Guard.ThrowExceptionIfNull<BlockContainerBase>(partBlockContainer, "partBlockContainer");
			this.partBlockContainer = partBlockContainer;
		}

		public RtfImportContext(RtfImportContext other)
		{
			this.Document = other.Document;
			this.FontTable = other.FontTable;
			this.ColorTable = other.ColorTable;
			this.StylesTable = other.StylesTable;
			this.ImageBuilder = other.ImageBuilder;
			this.ShapeBuilder = other.ShapeBuilder;
			this.ListTable = other.ListTable;
			this.ListOverrideTable = other.ListOverrideTable;
			this.Comments = new Dictionary<string, Comment>();
			this.Bookmarks = new Dictionary<string, Bookmark>();
			this.styleStack = new Stack<RtfStyle>();
			this.styleStack.Push(new RtfStyle(other.Document));
			this.structureStack = new Stack<RtfStructureStackItem>();
			this.FieldContext = new FieldContext(true);
		}

		public RtfImageBuilder ImageBuilder { get; set; }

		public RtfShapeBuilder ShapeBuilder { get; set; }

		public RtfStylesTable StylesTable { get; set; }

		public FieldContext FieldContext { get; set; }

		public RtfColorTable ColorTable { get; set; }

		public RtfFontTable FontTable { get; set; }

		public RtfListTable ListTable { get; set; }

		public RtfListOverrideTable ListOverrideTable { get; set; }

		public RtfDocumentInfo DocumentInfo { get; set; }

		public string DefaultFontId { get; set; }

		public FontFamily DefaultFont
		{
			get
			{
				FontFamily result = null;
				if (this.FontTable != null && !string.IsNullOrEmpty(this.DefaultFontId) && this.FontTable.ContainsFontWithId(this.DefaultFontId))
				{
					result = this.FontTable.GetFontFamily(this.DefaultFontId);
				}
				return result;
			}
		}

		public string Generator { get; set; }

		public RadFlowDocument Document { get; set; }

		public Section CurrentSection
		{
			get
			{
				return this.currentSection;
			}
			set
			{
				if (this.IsImportingPartBlockContainer)
				{
					throw new InvalidOperationException("Cannot set current section while importing child document part.");
				}
				this.currentSection = value;
			}
		}

		public BlockContainerBase CurrentBaseBlockConatiner
		{
			get
			{
				if (this.IsImportingPartBlockContainer)
				{
					return this.partBlockContainer;
				}
				return this.CurrentSection;
			}
		}

		public bool IsImportingPartBlockContainer
		{
			get
			{
				return this.partBlockContainer != null;
			}
		}

		public RtfStyle CurrentStyle
		{
			get
			{
				return this.styleStack.Peek();
			}
		}

		public TableRowStyle CurrentRowStyle
		{
			get
			{
				return this.CurrentStyle.RowStyle;
			}
		}

		public CommentImporter CurrentCommentHandler { get; set; }

		public Dictionary<string, Bookmark> Bookmarks { get; set; }

		public Dictionary<string, Comment> Comments { get; set; }

		public void HandleSectionEnd()
		{
			if (this.IsImportingPartBlockContainer)
			{
				return;
			}
			this.FlushSection();
			this.CurrentSection = this.Document.Sections.AddSection();
		}

		public void HandleParagraphEnd()
		{
			this.EnsureCurrentParagraph();
			this.FlushCurrentParagraph();
		}

		public void HandleRowEnd()
		{
			List<TableCell> list = new List<TableCell>();
			int num = ((this.structureStack.Count != 0) ? this.structureStack.Peek().NestingLevel : 0);
			while (this.structureStack.Count != 0 && this.structureStack.Peek().Element is TableCell && this.structureStack.Peek().NestingLevel == num)
			{
				RtfStructureStackItem rtfStructureStackItem = this.structureStack.Pop();
				list.Add(rtfStructureStackItem.Element as TableCell);
			}
			list.Reverse();
			int num2 = this.CurrentStyle.CurrentNestingLevel - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			RtfStructureRowStackItem rtfStructureRowStackItem = new RtfStructureRowStackItem(new TableRow(this.Document), num2, this.CurrentRowStyle);
			rtfStructureRowStackItem.CellsToBeAdded = list;
			this.structureStack.Push(rtfStructureRowStackItem);
			if (this.CurrentStyle.CurrentBorderedElementType == BorderedElementType.TableCell || this.CurrentStyle.CurrentBorderedElementType == BorderedElementType.TableRow)
			{
				this.CurrentStyle.CurrentBorderedElementType = BorderedElementType.Paragraph;
				this.CurrentStyle.CurrentBorder = null;
			}
		}

		public void HandleCellEnd()
		{
			bool flag = this.structureStack.Count == 0 || !this.structureStack.Peek().IsRowOrParagraph;
			this.FlushCurrentParagraph();
			if (!this.CurrentStyle.IsInTable)
			{
				return;
			}
			List<BlockBase> list = new List<BlockBase>();
			int num = this.CurrentStyle.CurrentNestingLevel;
			if (flag)
			{
				if (this.structureStack.Count == 0)
				{
					num = 1;
				}
				else
				{
					num = this.structureStack.Peek().NestingLevel;
				}
			}
			List<RtfStructureRowStackItem> list2 = new List<RtfStructureRowStackItem>();
			while (this.structureStack.Count > 0 && this.structureStack.Peek().IsRowOrParagraph && this.structureStack.Peek().NestingLevel == num)
			{
				RtfStructureStackItem rtfStructureStackItem = this.structureStack.Pop();
				if (rtfStructureStackItem.Element is Paragraph)
				{
					if (list2.Count != 0)
					{
						list.Add(this.FlushTableRowsIntoTable(list2));
						list2.Clear();
					}
					list.Add((Paragraph)rtfStructureStackItem.Element);
				}
				else
				{
					RtfStructureRowStackItem item = (RtfStructureRowStackItem)rtfStructureStackItem;
					list2.Add(item);
				}
			}
			if (list2.Count != 0)
			{
				list.Add(this.FlushTableRowsIntoTable(list2));
				list2.Clear();
			}
			list.Reverse();
			TableCell tableCell = new TableCell(this.Document);
			foreach (BlockBase item2 in list)
			{
				tableCell.Blocks.Add(item2);
			}
			this.structureStack.Push(new RtfStructureStackItem(tableCell, num));
			if (this.CurrentStyle.CurrentBorderedElementType == BorderedElementType.TableCell)
			{
				this.CurrentStyle.CurrentBorderedElementType = BorderedElementType.Paragraph;
				this.CurrentStyle.CurrentBorder = null;
			}
		}

		public void FlushSection()
		{
			this.FlushCurrentParagraph();
			this.FlushStructureStack();
			if (this.CurrentSection != null)
			{
				this.CurrentStyle.ApplySectionStyle(this.CurrentSection);
			}
		}

		public void EnterGroup(RtfGroup group)
		{
			this.styleStack.Push(new RtfStyle(this.CurrentStyle));
			if (group.Destination == "field")
			{
				FieldCharacter fieldCharacter = new FieldCharacter(this.Document, FieldCharacterType.Start);
				this.AddInline(fieldCharacter);
				this.FieldContext.OnFieldCharacter(fieldCharacter);
			}
		}

		public void LeaveGroup(RtfGroup group)
		{
			if (group.Destination == "fldinst")
			{
				FieldCharacter fieldCharacter = new FieldCharacter(this.Document, FieldCharacterType.Separator);
				this.AddInline(fieldCharacter);
				this.FieldContext.OnFieldCharacter(fieldCharacter);
			}
			else if (group.Destination == "field")
			{
				FieldCharacter fieldCharacter2 = new FieldCharacter(this.Document, FieldCharacterType.End);
				this.AddInline(fieldCharacter2);
				this.FieldContext.OnFieldCharacter(fieldCharacter2);
			}
			this.styleStack.Pop();
		}

		public void AddText(string text)
		{
			this.EnsureCurrentParagraph();
			if (!this.CurrentStyle.IsHidden)
			{
				Run run = this.currentParagraph.Inlines.AddRun();
				run.Text = text;
				this.CurrentStyle.ApplyCharacterStyle(run);
			}
		}

		public void AddInline(InlineBase inline)
		{
			this.EnsureCurrentParagraph();
			if (!this.CurrentStyle.IsHidden)
			{
				this.currentParagraph.Inlines.Add(inline);
			}
		}

		public void AddWatermark(Watermark watermark)
		{
			((Header)this.CurrentBaseBlockConatiner).Watermarks.Add(watermark);
		}

		public void ClearUnclosedBookmarkRanges()
		{
			foreach (Bookmark bookmark in this.Bookmarks.Values)
			{
				if (bookmark.BookmarkRangeEnd.Paragraph == null)
				{
					bookmark.BookmarkRangeStart.Paragraph.Inlines.Remove(bookmark.BookmarkRangeStart);
				}
			}
		}

		public Comment GetComment(string commentId)
		{
			Comment comment;
			if (!this.Comments.ContainsKey(commentId))
			{
				comment = this.Document.Comments.AddComment();
				this.Comments[commentId] = comment;
			}
			else
			{
				comment = this.Comments[commentId];
			}
			return comment;
		}

		static void SetCellsPreferredWidth(List<int> cellXs, Table table)
		{
			int num = 0;
			TableRow tableRow = (from ro in table.Rows
				where ro.Cells.Count == cellXs.Count
				select ro).FirstOrDefault<TableRow>();
			if (tableRow != null)
			{
				for (int i = 0; i < cellXs.Count; i++)
				{
					int num2 = cellXs[i] - num;
					tableRow.Cells[i].PreferredWidth = new TableWidthUnit((double)Unit.TwipToDipF((double)num2));
					num = cellXs[i];
				}
			}
		}

		void EnsureCurrentParagraph()
		{
			if (this.currentParagraph == null)
			{
				this.currentParagraph = new Paragraph(this.Document);
			}
		}

		Table FlushTableRowsIntoTable(List<RtfStructureRowStackItem> rowStackItems)
		{
			rowStackItems.Reverse();
			List<int> list = (from x in (from cellStyle in rowStackItems.SelectMany((RtfStructureRowStackItem rowItem) => rowItem.RowStyle.CellStyles)
					select cellStyle.CellXBoundry).Distinct<int>()
				orderby x
				select x).ToList<int>();
			int count = rowStackItems.Count;
			int count2 = list.Count;
			TableCell[,] array = new TableCell[count2, count];
			for (int i = 0; i < rowStackItems.Count; i++)
			{
				RtfStructureRowStackItem rtfStructureRowStackItem = rowStackItems[i];
				TableRow row = rtfStructureRowStackItem.Row;
				List<TableCellStyle> cellStyles = rtfStructureRowStackItem.RowStyle.CellStyles;
				List<TableCell> cellsToBeAdded = rtfStructureRowStackItem.CellsToBeAdded;
				rtfStructureRowStackItem.RowStyle.ApplyRowStyle(row);
				int j = 0;
				for (int k = 0; k < cellsToBeAdded.Count; k++)
				{
					if (cellStyles.Count <= k)
					{
						row.Cells.Add(cellsToBeAdded[k]);
					}
					else
					{
						int num = list.IndexOf(cellStyles[k].CellXBoundry) + 1;
						int num2 = num - j;
						if (num2 != 0)
						{
							if (cellStyles[k].IsInVerticalRange && i > 0 && array[j, i - 1] != null)
							{
								TableCell tableCell = array[j, i - 1];
								int num3 = j + num2;
								while (j < num3)
								{
									array[j, i] = tableCell;
									j++;
								}
								tableCell.RowSpan++;
							}
							else
							{
								int num4 = j + num2;
								while (j < num4)
								{
									array[j, i] = cellsToBeAdded[k];
									j++;
								}
								cellsToBeAdded[k].ColumnSpan = num2;
								cellStyles[k].ApplyCellStyle(cellsToBeAdded[k]);
								row.Cells.Add(cellsToBeAdded[k]);
							}
						}
					}
				}
			}
			Table table = new Table(this.Document);
			rowStackItems.First<RtfStructureRowStackItem>().RowStyle.ApplyTableStyle(table);
			for (int l = 0; l < rowStackItems.Count; l++)
			{
				RtfStructureRowStackItem rtfStructureRowStackItem2 = rowStackItems[l];
				if (rtfStructureRowStackItem2.Row.Cells.Any<TableCell>())
				{
					table.Rows.Add(rtfStructureRowStackItem2.Row);
				}
				else
				{
					HashSet<TableCell> hashSet = new HashSet<TableCell>();
					for (int m = 0; m < count2; m++)
					{
						hashSet.Add(array[m, l]);
					}
					foreach (TableCell tableCell2 in hashSet)
					{
						if (tableCell2.RowSpan > 1)
						{
							tableCell2.RowSpan--;
						}
					}
				}
			}
			if (table.Properties.LayoutType.LocalValue == TableLayoutType.FixedWidth)
			{
				RtfImportContext.SetCellsPreferredWidth(list, table);
			}
			return table;
		}

		void FlushCurrentParagraph()
		{
			if (this.currentParagraph != null)
			{
				this.CurrentStyle.ApplyParagraphStyle(this.currentParagraph);
				if (this.CurrentStyle.IsInTable)
				{
					int nestingLevel = Math.Max(1, this.CurrentStyle.CurrentNestingLevel);
					this.structureStack.Push(new RtfStructureStackItem(this.currentParagraph, nestingLevel));
				}
				else
				{
					this.FlushStructureStack();
					this.CurrentBaseBlockConatiner.Blocks.Add(this.currentParagraph);
				}
			}
			this.currentParagraph = null;
		}

		void FlushStructureStack()
		{
			if (this.structureStack.Count != 0)
			{
				List<RtfStructureRowStackItem> list = new List<RtfStructureRowStackItem>();
				while (this.structureStack.Count != 0)
				{
					RtfStructureStackItem rtfStructureStackItem = this.structureStack.Pop();
					RtfStructureRowStackItem rtfStructureRowStackItem = rtfStructureStackItem as RtfStructureRowStackItem;
					if (rtfStructureRowStackItem != null)
					{
						list.Add(rtfStructureRowStackItem);
					}
				}
				Table item = this.FlushTableRowsIntoTable(list);
				this.CurrentBaseBlockConatiner.Blocks.Add(item);
			}
		}

		readonly Stack<RtfStyle> styleStack;

		readonly Stack<RtfStructureStackItem> structureStack;

		readonly BlockContainerBase partBlockContainer;

		Section currentSection;

		Paragraph currentParagraph;
	}
}
