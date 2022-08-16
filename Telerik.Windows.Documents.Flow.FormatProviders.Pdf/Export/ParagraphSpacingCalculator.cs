using System;
using System.Linq;
using CsQuery.ExtensionMethods;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	class ParagraphSpacingCalculator
	{
		public ParagraphSpacingCalculator()
		{
			this.hasSpacingBefore = true;
		}

		public void SetBlockSpacingBeforeAndAfter(Block block, Paragraph paragraph, Paragraph nextParagraph)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			if (nextParagraph != null && paragraph.ContextualSpacing && ParagraphSpacingCalculator.HaveSameStyle(paragraph, nextParagraph))
			{
				block.SpacingAfter = 0.0;
				this.hasSpacingBefore = false;
			}
			else
			{
				block.SpacingAfter = ParagraphSpacingCalculator.GetSpacingAfter(paragraph);
				this.hasSpacingBefore = true;
			}
			if (this.hasSpacingBefore)
			{
				block.SpacingBefore = ParagraphSpacingCalculator.GetSpacingBefore(paragraph);
				return;
			}
			block.SpacingBefore = 0.0;
		}

		static double GetSpacingBefore(Paragraph paragraph)
		{
			if (paragraph.Spacing.AutomaticSpacingBefore)
			{
				return ParagraphSpacingCalculator.GetAutomaticSpacingBefore(paragraph);
			}
			return paragraph.Spacing.SpacingBefore;
		}

		static double GetSpacingAfter(Paragraph paragraph)
		{
			if (paragraph.Spacing.AutomaticSpacingAfter)
			{
				return ParagraphSpacingCalculator.GetAutomaticSpacingAfter(paragraph);
			}
			return paragraph.Spacing.SpacingAfter;
		}

		static double GetAutomaticSpacingBefore(Paragraph paragraph)
		{
			if (!ParagraphSpacingCalculator.IsFirstParagraphIsBlock(paragraph) && ParagraphSpacingCalculator.IsFirstParagraphInList(paragraph))
			{
				return ParagraphSpacingCalculator.largeSpacing;
			}
			return ParagraphSpacingCalculator.smallSpacing;
		}

		static double GetAutomaticSpacingAfter(Paragraph paragraph)
		{
			if (ParagraphSpacingCalculator.IsInList(paragraph))
			{
				if (ParagraphSpacingCalculator.IsLastParagraphInList(paragraph))
				{
					return ParagraphSpacingCalculator.largeSpacing;
				}
				return ParagraphSpacingCalculator.smallSpacing;
			}
			else
			{
				if (ParagraphSpacingCalculator.IsLastParagraphInTableCell(paragraph))
				{
					return ParagraphSpacingCalculator.smallSpacing;
				}
				return ParagraphSpacingCalculator.largeSpacing;
			}
		}

		static bool IsFirstParagraphIsBlock(Paragraph paragraph)
		{
			return paragraph.Parent.Children.First<DocumentElementBase>() == paragraph;
		}

		static bool IsFirstParagraphInList(Paragraph paragraph)
		{
			if (!ParagraphSpacingCalculator.IsInList(paragraph))
			{
				return false;
			}
			if (ParagraphSpacingCalculator.IsFirstParagraphIsBlock(paragraph))
			{
				return true;
			}
			Paragraph previousParagraph = ParagraphSpacingCalculator.GetPreviousParagraph(paragraph);
			return previousParagraph == null || !ParagraphSpacingCalculator.IsInList(previousParagraph) || paragraph.ListId != previousParagraph.ListId;
		}

		static bool IsLastParagraphIsBlock(Paragraph paragraph)
		{
			return paragraph.Parent.Children.Last<DocumentElementBase>() == paragraph;
		}

		static bool IsLastParagraphInList(Paragraph paragraph)
		{
			if (!ParagraphSpacingCalculator.IsInList(paragraph))
			{
				return false;
			}
			if (ParagraphSpacingCalculator.IsLastParagraphIsBlock(paragraph))
			{
				return true;
			}
			Paragraph nextParagraph = ParagraphSpacingCalculator.GetNextParagraph(paragraph);
			return nextParagraph == null || nextParagraph == null || !ParagraphSpacingCalculator.IsInList(nextParagraph) || paragraph.ListId != nextParagraph.ListId;
		}

		static bool IsLastParagraphInTableCell(Paragraph paragraph)
		{
			TableCell tableCell = paragraph.Parent as TableCell;
			return tableCell != null && tableCell.Blocks.Last<BlockBase>() == paragraph;
		}

		static bool IsInList(Paragraph paragraph)
		{
			RadFlowDocument document = paragraph.Document;
			return document.Lists.GetList(paragraph.ListId) != null;
		}

		static Paragraph GetPreviousParagraph(Paragraph paragraph)
		{
			int num = paragraph.Parent.Children.IndexOf(paragraph);
			if (num > 0)
			{
				return paragraph.Parent.Children.ElementAt(num - 1) as Paragraph;
			}
			return null;
		}

		static Paragraph GetNextParagraph(Paragraph paragraph)
		{
			int num = paragraph.Parent.Children.IndexOf(paragraph);
			if (num < paragraph.Parent.Children.Count<DocumentElementBase>() - 1)
			{
				return paragraph.Parent.Children.ElementAt(num + 1) as Paragraph;
			}
			return null;
		}

		static bool HaveSameStyle(Paragraph p1, Paragraph p2)
		{
			return p1.StyleId == p2.StyleId;
		}

		const double LargeSpacingInPoints = 14.0;

		const double SmallSpacingInPoints = 0.0;

		static readonly double largeSpacing = Unit.PointToDip(14.0);

		static readonly double smallSpacing = Unit.PointToDip(0.0);

		bool hasSpacingBefore;
	}
}
