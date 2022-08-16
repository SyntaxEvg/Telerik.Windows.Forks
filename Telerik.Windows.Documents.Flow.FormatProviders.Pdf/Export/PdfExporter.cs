using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	class PdfExporter
	{
		public PdfExporter(PdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			this.context = context;
			this.flowToFixedListId = new Dictionary<int, int>();
			this.hyperlinksStack = new Stack<HyperlinkInfo>();
			this.paragraphSpacingCalculator = new ParagraphSpacingCalculator();
			this.headerFootersInfo = new HeadersFootersInfo();
		}

		public RadFixedDocumentEditor Editor { get; set; }

		HyperlinkInfo CurrentHyperlink
		{
			get
			{
				if (this.hyperlinksStack.Count > 0)
				{
					return this.hyperlinksStack.Peek();
				}
				return null;
			}
		}

		public RadFixedDocument Export()
		{
			this.document = new RadFixedDocument();
			using (this.Editor = new RadFixedDocumentEditor(this.document))
			{
				this.Editor.PageCreated += this.Editor_PageCreated;
				this.ExportDocument(this.context.Document, this.Editor);
				this.Editor.PageCreated -= this.Editor_PageCreated;
			}
			return this.document;
		}

		static bool TryCreateHyperlinkInfo(Hyperlink hyperlink, out HyperlinkInfo hyperlinkInfo)
		{
			hyperlinkInfo = null;
			Uri uri;
			if (Uri.TryCreate(hyperlink.Uri, UriKind.Absolute, out uri))
			{
				hyperlinkInfo = new HyperlinkInfo(uri);
				return true;
			}
			return false;
		}

		static void ExportBreak(RadFixedDocumentEditor editor, Break b, Block block)
		{
			switch (b.BreakType)
			{
			case BreakType.LineBreak:
				block.InsertLineBreak();
				return;
			case BreakType.PageBreak:
				if (editor != null)
				{
					editor.InsertBlock(block);
					editor.InsertPageBreak();
					block.Clear();
				}
				return;
			default:
				return;
			}
		}

		static void OverrideParagraphProperties(Telerik.Windows.Documents.Fixed.Model.Editing.Lists.ListLevel level, Block block)
		{
			if (block.FirstLineIndent != 0.0)
			{
				level.ParagraphProperties.FirstLineIndent = block.FirstLineIndent;
			}
			if (block.LeftIndent != 0.0)
			{
				level.ParagraphProperties.LeftIndent = block.LeftIndent;
				bool flag = block.FirstLineIndent >= 0.0 && level.ParagraphProperties.FirstLineIndent < 0.0;
				if (flag)
				{
					level.ParagraphProperties.LeftIndent += level.ParagraphProperties.FirstLineIndent;
				}
			}
		}

		static IEnumerable<string> SplitRunOnTabs(string runText)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < runText.Length; i++)
			{
				if (runText[i] == '\t')
				{
					yield return builder.ToString();
					yield return '\t'.ToString();
					builder.Clear();
				}
				else
				{
					builder.Append(runText[i]);
				}
			}
			yield return builder.ToString();
			builder.Clear();
			yield break;
		}

		static void ExportWatermark(SectionInfo sectionInfo, WatermarkCollection watermarks)
		{
			foreach (object obj in watermarks)
			{
				Watermark watermark = (Watermark)obj;
				switch (watermark.WatermarkType)
				{
				case WatermarkType.Image:
				{
					ImageWatermarkSettings imageWatermarkSettings = new ImageWatermarkSettings();
					imageWatermarkSettings.CopyPropertiesFrom(watermark.ImageSettings);
					sectionInfo.InsertWatermark(imageWatermarkSettings);
					break;
				}
				case WatermarkType.Text:
				{
					TextWatermarkSettings textWatermarkSettings = new TextWatermarkSettings();
					textWatermarkSettings.CopyPropertiesFrom(watermark.TextSettings);
					sectionInfo.InsertWatermark(textWatermarkSettings);
					break;
				}
				}
			}
		}

		void ExportDocument(RadFlowDocument document, RadFixedDocumentEditor editor)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<RadFixedDocumentEditor>(editor, "editor");
			foreach (Telerik.Windows.Documents.Flow.Model.Lists.List flowList in document.Lists)
			{
				this.CreateList(flowList);
			}
			foreach (Section section in document.Sections)
			{
				this.currentSection = section;
				this.currentSectionPageNo = this.document.Pages.Count + 1;
				this.ExportSection(section, editor);
			}
		}

		void CreateList(Telerik.Windows.Documents.Flow.Model.Lists.List flowList)
		{
			Telerik.Windows.Documents.Fixed.Model.Editing.Lists.List list = this.Editor.Lists.AddList();
			this.flowToFixedListId[flowList.Id] = list.Id;
			foreach (Telerik.Windows.Documents.Flow.Model.Lists.ListLevel flowLevel in flowList.Levels)
			{
				list.Levels.Add(this.CreateListLevel(flowLevel));
			}
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.Lists.ListLevel CreateListLevel(Telerik.Windows.Documents.Flow.Model.Lists.ListLevel flowLevel)
		{
			Telerik.Windows.Documents.Fixed.Model.Editing.Lists.ListLevel listLevel = new Telerik.Windows.Documents.Fixed.Model.Editing.Lists.ListLevel();
			listLevel.CharacterProperties.CopyPropertiesFrom(this.context, flowLevel.CharacterProperties);
			listLevel.RestartAfterLevel = flowLevel.RestartAfterLevel;
			listLevel.StartIndex = flowLevel.StartIndex;
			double firstLineIndent = Extensions.GetFirstLineIndent(flowLevel.ParagraphProperties);
			listLevel.ParagraphProperties.FirstLineIndent = firstLineIndent;
			listLevel.ParagraphProperties.LeftIndent = Extensions.CalculateActualLeftIndent(flowLevel.ParagraphProperties.LeftIndent.GetActualValue(), firstLineIndent);
			listLevel.BulletNumberingFormat = this.context.Settings.ExtensibilityManager.ListHelper.CreateBulletFormat(flowLevel);
			return listLevel;
		}

		void ExportSection(Section section, RadFixedDocumentEditor editor)
		{
			Guard.ThrowExceptionIfNull<Section>(section, "section");
			Guard.ThrowExceptionIfNull<RadFixedDocumentEditor>(editor, "editor");
			editor.CopyPropertiesFrom(section.Properties);
			editor.InsertSectionBreak();
			foreach (IBlockElement block in this.ExportBlockContainer(section, editor))
			{
				editor.InsertBlock(block);
			}
		}

		IEnumerable<IBlockElement> ExportBlockContainer(BlockContainerBase blockContainer, RadFixedDocumentEditor editor = null)
		{
			Guard.ThrowExceptionIfNull<BlockContainerBase>(blockContainer, "blockContainer");
			int last = blockContainer.Blocks.Count - 1;
			for (int i = 0; i < blockContainer.Blocks.Count; i++)
			{
				IBlockElement blockElement = this.ExportBlock(editor, blockContainer.Blocks[i], (i == last) ? null : blockContainer.Blocks[i + 1], i == 0);
				if (blockElement != null)
				{
					yield return blockElement;
				}
			}
			yield break;
		}

		IBlockElement ExportBlock(RadFixedDocumentEditor editor, BlockBase block, BlockBase nextBlock, bool isFirstBlockInContainer)
		{
			Guard.ThrowExceptionIfNull<BlockBase>(block, "block");
			IBlockElement result = null;
			switch (block.Type)
			{
			case DocumentElementType.Paragraph:
				result = this.ExportParagraph(editor, (Paragraph)block, nextBlock as Paragraph, isFirstBlockInContainer);
				break;
			case DocumentElementType.Table:
				result = this.ExportTable((Telerik.Windows.Documents.Flow.Model.Table)block);
				break;
			}
			return result;
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.Tables.Table ExportTable(Telerik.Windows.Documents.Flow.Model.Table table)
		{
			Telerik.Windows.Documents.Fixed.Model.Editing.Tables.Table table2 = new Telerik.Windows.Documents.Fixed.Model.Editing.Tables.Table();
			table2.Borders = this.ExportTableBorders(table.Borders);
			table2.Background = table.Properties.BackgroundColor.ToColor(this.context);
			Telerik.Windows.Documents.Fixed.Model.Editing.Border border = this.ExportBorder(table.Borders.InsideHorizontal);
			Telerik.Windows.Documents.Fixed.Model.Editing.Border border2 = this.ExportBorder(table.Borders.InsideVertical);
			int num = table.Rows.Count - 1;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				Telerik.Windows.Documents.Flow.Model.TableRow tableRow = table.Rows[i];
				Telerik.Windows.Documents.Fixed.Model.Editing.Tables.TableRow tableRow2 = table2.Rows.AddTableRow();
				Telerik.Windows.Documents.Fixed.Model.Editing.Border innerHorizontalBorder = ((i < num) ? border : null);
				for (int j = 0; j < tableRow.Cells.Count; j++)
				{
					int num2 = tableRow.Cells.Count - 1;
					Telerik.Windows.Documents.Flow.Model.TableCell cell = tableRow.Cells[j];
					Telerik.Windows.Documents.Fixed.Model.Editing.Border innerVerticalBorder = ((j < num2) ? border2 : null);
					this.ExportTableCell(cell, tableRow2, innerHorizontalBorder, innerVerticalBorder);
				}
			}
			double availableContentWidth = this.Editor.GetAvailableContentWidth();
			table2.CopyPropertiesFrom(this.context, table.Properties, availableContentWidth);
			return table2;
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.TableBorders ExportTableBorders(Telerik.Windows.Documents.Flow.Model.Styles.TableBorders tableBorders)
		{
			return new Telerik.Windows.Documents.Fixed.Model.Editing.TableBorders(this.ExportBorder(tableBorders.Left), this.ExportBorder(tableBorders.Top), this.ExportBorder(tableBorders.Right), this.ExportBorder(tableBorders.Bottom));
		}

		void ExportTableCell(Telerik.Windows.Documents.Flow.Model.TableCell cell, Telerik.Windows.Documents.Fixed.Model.Editing.Tables.TableRow tableRow, Telerik.Windows.Documents.Fixed.Model.Editing.Border innerHorizontalBorder, Telerik.Windows.Documents.Fixed.Model.Editing.Border innerVerticalBorder)
		{
			Telerik.Windows.Documents.Fixed.Model.Editing.Tables.TableCell tableCell = tableRow.Cells.AddTableCell();
			tableCell.Background = cell.Properties.BackgroundColor.ToColor(this.context);
			tableCell.Borders = this.ExportTableCellBorders(cell.Borders, innerHorizontalBorder, innerVerticalBorder);
			tableCell.ColumnSpan = cell.ColumnSpan;
			tableCell.RowSpan = cell.RowSpan;
			tableCell.Padding = new Thickness?(cell.Padding.ToThickness());
			tableCell.PreferredWidth = cell.PreferredWidth.ToPreferedWidth(this.Editor.GetAvailableContentWidth());
			foreach (IBlockElement item in this.ExportBlockContainer(cell, null))
			{
				tableCell.Blocks.Add(item);
			}
			if (!tableCell.Blocks.Any<IBlockElement>())
			{
				Block block = this.CreateDefaultPropertiesBlock();
				block.Insert(new SpaceLayoutElement(block.TextProperties));
				tableCell.Blocks.Add(block);
			}
		}

		Block CreateDefaultPropertiesBlock()
		{
			Block block = new Block();
			block.CopyPropertiesFrom(this.context, this.context.DefaultValues.CharacterProperties);
			block.CopyPropertiesFrom(this.context, this.context.DefaultValues.ParagraphProperties);
			return block;
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.Tables.TableCellBorders ExportTableCellBorders(Telerik.Windows.Documents.Flow.Model.Styles.TableCellBorders tableCellBorders, Telerik.Windows.Documents.Fixed.Model.Editing.Border innerHorizontalBorder, Telerik.Windows.Documents.Fixed.Model.Editing.Border innerVerticalBorder)
		{
			return new Telerik.Windows.Documents.Fixed.Model.Editing.Tables.TableCellBorders(this.ExportBorder(tableCellBorders.Left), this.ExportBorder(tableCellBorders.Top), this.ExportBorder(tableCellBorders.Right, innerVerticalBorder), this.ExportBorder(tableCellBorders.Bottom, innerHorizontalBorder), this.ExportBorder(tableCellBorders.DiagonalUp), this.ExportBorder(tableCellBorders.DiagonalDown));
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.Border ExportBorder(Telerik.Windows.Documents.Flow.Model.Styles.Border border)
		{
			return new Telerik.Windows.Documents.Fixed.Model.Editing.Border(border.Thickness, border.Style.ToBorderStyle(), border.Color.ToColor(this.context));
		}

		Telerik.Windows.Documents.Fixed.Model.Editing.Border ExportBorder(Telerik.Windows.Documents.Flow.Model.Styles.Border border, Telerik.Windows.Documents.Fixed.Model.Editing.Border inheritedBorder)
		{
			if (border.Style == Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle.Inherit && inheritedBorder != null)
			{
				return inheritedBorder;
			}
			return new Telerik.Windows.Documents.Fixed.Model.Editing.Border(border.Thickness, border.Style.ToBorderStyle(), border.Color.ToColor(this.context));
		}

		Block ExportParagraph(RadFixedDocumentEditor editor, Paragraph paragraph, Paragraph nextParagraph, bool isFirstBlockInContainer)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			if (!isFirstBlockInContainer && paragraph.PageBreakBefore)
			{
				editor.InsertPageBreak();
			}
			Block block = new Block();
			block.CopyPropertiesFrom(this.context, paragraph.Properties);
			this.paragraphSpacingCalculator.SetBlockSpacingBeforeAndAfter(block, paragraph, nextParagraph);
			foreach (HyperlinkInfo hyperlinkInfo in this.hyperlinksStack.Reverse<HyperlinkInfo>())
			{
				this.ExportHyperlinkStart(hyperlinkInfo, block);
			}
			if (paragraph.ListId > -1)
			{
				int listId = this.GetListId(paragraph.ListId);
				int num = ((paragraph.ListLevel > -1) ? paragraph.ListLevel : FixedDocumentDefaults.FirstListLevelIndex);
				Telerik.Windows.Documents.Fixed.Model.Editing.Lists.List list = this.Editor.Lists[listId];
				Telerik.Windows.Documents.Fixed.Model.Editing.Lists.ListLevel listLevel = list.Levels[num];
				using (listLevel.ParagraphProperties.Save())
				{
					PdfExporter.OverrideParagraphProperties(listLevel, block);
					block.SetBullet(list, num);
					this.ExportInlines(editor, paragraph, block);
					return block;
				}
			}
			this.ExportInlines(editor, paragraph, block);
			return block;
		}

		void ExportInlines(RadFixedDocumentEditor editor, Paragraph paragraph, Block block)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			if (paragraph.Inlines.Any<InlineBase>())
			{
				using (IEnumerator<InlineBase> enumerator = paragraph.Inlines.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InlineBase inline = enumerator.Current;
						this.ExportInline(editor, inline, block);
					}
					return;
				}
			}
			block.Insert(new SpaceLayoutElement(block.TextProperties));
		}

		void ExportInline(RadFixedDocumentEditor editor, InlineBase inline, Block block)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			if (this.skipUntilNextFieldCharacter)
			{
				this.skipUntilNextFieldCharacter = inline.Type == DocumentElementType.FieldCharacter;
				if (inline.Type == DocumentElementType.FieldCharacter)
				{
					this.ExportFieldCharacter((FieldCharacter)inline, block);
					return;
				}
			}
			else
			{
				switch (inline.Type)
				{
				case DocumentElementType.Run:
					this.ExportRun((Run)inline, block);
					return;
				case DocumentElementType.Header:
				case DocumentElementType.Footer:
				case DocumentElementType.FloatingImage:
					break;
				case DocumentElementType.FieldCharacter:
					this.ExportFieldCharacter((FieldCharacter)inline, block);
					break;
				case DocumentElementType.ImageInline:
					this.ExportImageInline((ImageInline)inline, block);
					return;
				case DocumentElementType.Break:
					PdfExporter.ExportBreak(editor, (Break)inline, block);
					return;
				default:
					return;
				}
			}
		}

		void ExportFieldCharacter(FieldCharacter fieldCharacter, Block block)
		{
			Hyperlink hyperlink = fieldCharacter.FieldInfo.Field as Hyperlink;
			if (fieldCharacter.FieldCharacterType == FieldCharacterType.Start)
			{
				this.skipUntilNextFieldCharacter = true;
				HyperlinkInfo hyperlinkInfo;
				if (hyperlink != null && PdfExporter.TryCreateHyperlinkInfo(hyperlink, out hyperlinkInfo))
				{
					this.ExportHyperlinkStart(hyperlinkInfo, block);
					return;
				}
			}
			else
			{
				if (fieldCharacter.FieldCharacterType == FieldCharacterType.End && hyperlink != null)
				{
					this.ExportHyperlinkEnd(block);
					return;
				}
				string a;
				if (fieldCharacter.FieldCharacterType == FieldCharacterType.Separator && (a = fieldCharacter.FieldInfo.GetCode().Trim().ToLowerInvariant()) != null)
				{
					if (!(a == "page"))
					{
						return;
					}
					block.InsertText(this.GetCurrentPageNumber());
					this.skipUntilNextFieldCharacter = true;
				}
			}
		}

		string GetCurrentPageNumber()
		{
			int num = this.document.Pages.Count;
			if (this.currentSection.PageNumberingSettings.StartingPageNumber != null)
			{
				num += this.currentSection.PageNumberingSettings.StartingPageNumber.Value - 1;
			}
			if (this.currentSection.PageNumberingSettings.PageNumberFormat != null)
			{
				return this.context.Settings.ExtensibilityManager.ListHelper.GetNumberText(this.currentSection.PageNumberingSettings.PageNumberFormat.Value, num);
			}
			return num.ToString();
		}

		void ExportHyperlinkEnd(Block block)
		{
			if (this.CurrentHyperlink != null && this.CurrentHyperlink.Uri != null)
			{
				block.InsertHyperlinkEnd();
				this.hyperlinksStack.Pop();
			}
		}

		void ExportHyperlinkStart(HyperlinkInfo hyperlinkInfo, Block block)
		{
			if (hyperlinkInfo.Uri != null)
			{
				block.InsertHyperlinkStart(hyperlinkInfo.Uri);
			}
			this.hyperlinksStack.Push(hyperlinkInfo);
		}

		void ExportImageInline(ImageInline imageInline, Block block)
		{
			Guard.ThrowExceptionIfNull<ImageInline>(imageInline, "imageInline");
			Guard.ThrowExceptionIfNull<Block>(block, "editor");
			block.InsertImage(this.context.GetImageSource(imageInline.Image.ImageSource), imageInline.Image.Size);
		}

		void ExportRun(Run run, Block block)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			Guard.ThrowExceptionIfNull<Block>(block, "editor");
			block.CopyPropertiesFrom(this.context, run.Properties);
			foreach (string text in PdfExporter.SplitRunOnTabs(run.Text))
			{
				if (text == '\t'.ToString())
				{
					block.InsertTab();
				}
				else
				{
					block.InsertText(text);
				}
			}
		}

		int GetListId(int flowListId)
		{
			return this.flowToFixedListId[flowListId];
		}

		double ExportHeader(SectionInfo sectionInfo, Header header)
		{
			double num = 0.0;
			sectionInfo.Properties.PageMargins = new Padding(this.currentSection.PageMargins.Left, this.currentSection.HeaderTopMargin, this.currentSection.PageMargins.Right, 0.0);
			sectionInfo.UpdateAvailableSize();
			foreach (IBlockElement blockElement in this.ExportBlockContainer(header, null))
			{
				sectionInfo.InsertBlockElement(blockElement);
				num += blockElement.DesiredSize.Height;
			}
			return num + this.currentSection.HeaderTopMargin;
		}

		double ExportFooter(SectionInfo sectionInfo, Footer footer)
		{
			Size availableSize = new Size(this.currentSection.PageSize.Width - this.currentSection.PageMargins.Left - this.currentSection.PageMargins.Right, double.PositiveInfinity);
			double num = 0.0;
			List<IBlockElement> list = new List<IBlockElement>();
			foreach (IBlockElement blockElement in this.ExportBlockContainer(footer, null))
			{
				list.Add(blockElement);
				blockElement.Measure(availableSize);
				num += blockElement.DesiredSize.Height;
			}
			sectionInfo.Properties.PageMargins = new Padding(this.currentSection.PageMargins.Left, this.currentSection.PageSize.Height - num - this.currentSection.FooterBottomMargin, this.currentSection.PageMargins.Right, this.currentSection.FooterBottomMargin);
			sectionInfo.UpdateAvailableSize();
			foreach (IBlockElement blockElement2 in list)
			{
				sectionInfo.InsertBlockElement(blockElement2);
			}
			return num + this.currentSection.FooterBottomMargin;
		}

		HeaderFooterType GetCurrentHeaderFooterType(int pageNo)
		{
			if (this.IsHeaderFooterFirst(pageNo))
			{
				return HeaderFooterType.First;
			}
			if (this.IsHeaderFooterEven(pageNo))
			{
				return HeaderFooterType.Even;
			}
			return HeaderFooterType.Default;
		}

		bool IsHeaderFooterEven(int pageNo)
		{
			return this.context.Document.HasDifferentEvenOddPageHeadersFooters && pageNo % 2 == 0;
		}

		bool IsHeaderFooterFirst(int pageNo)
		{
			return this.currentSection.HasDifferentFirstPageHeaderFooter && pageNo == this.currentSectionPageNo;
		}

		void Editor_PageCreated(object sender, RadFixedPageCreatedEventArgs e)
		{
			int count = this.document.Pages.Count;
			this.FillCurrentHeaderFooters();
			HeaderFooterType currentHeaderFooterType = this.GetCurrentHeaderFooterType(count);
			double val = 0.0;
			Header lastUsedHeader = this.headerFootersInfo.GetLastUsedHeader(currentHeaderFooterType);
			if (lastUsedHeader != null)
			{
				val = this.ExportHeader(e.Section, lastUsedHeader);
			}
			double val2 = 0.0;
			Footer lastUsedFooter = this.headerFootersInfo.GetLastUsedFooter(currentHeaderFooterType);
			if (lastUsedFooter != null)
			{
				val2 = this.ExportFooter(e.Section, lastUsedFooter);
			}
			e.Section.Properties.PageMargins = new Padding(this.currentSection.PageMargins.Left, Math.Max(val, this.currentSection.PageMargins.Top), this.currentSection.PageMargins.Right, Math.Max(val2, this.currentSection.PageMargins.Bottom));
			e.Section.UpdateAvailableSize();
			if (lastUsedHeader != null)
			{
				PdfExporter.ExportWatermark(e.Section, lastUsedHeader.Watermarks);
			}
		}

		void FillCurrentHeaderFooters()
		{
			Headers headers = this.currentSection.Headers;
			if (headers.Default != null)
			{
				this.headerFootersInfo.SetLastUsedHeader(HeaderFooterType.Default, headers.Default);
			}
			if (headers.Even != null)
			{
				this.headerFootersInfo.SetLastUsedHeader(HeaderFooterType.Even, headers.Even);
			}
			if (headers.First != null)
			{
				this.headerFootersInfo.SetLastUsedHeader(HeaderFooterType.First, headers.First);
			}
			Footers footers = this.currentSection.Footers;
			if (footers.Default != null)
			{
				this.headerFootersInfo.SetLastUsedFooter(HeaderFooterType.Default, footers.Default);
			}
			if (footers.Even != null)
			{
				this.headerFootersInfo.SetLastUsedFooter(HeaderFooterType.Even, footers.Even);
			}
			if (footers.First != null)
			{
				this.headerFootersInfo.SetLastUsedFooter(HeaderFooterType.First, footers.First);
			}
		}

		const char Tab = '\t';

		readonly Dictionary<int, int> flowToFixedListId;

		readonly PdfExportContext context;

		readonly Stack<HyperlinkInfo> hyperlinksStack;

		readonly ParagraphSpacingCalculator paragraphSpacingCalculator;

		readonly HeadersFootersInfo headerFootersInfo;

		RadFixedDocument document;

		Section currentSection;

		int currentSectionPageNo;

		bool skipUntilNextFieldCharacter;
	}
}
