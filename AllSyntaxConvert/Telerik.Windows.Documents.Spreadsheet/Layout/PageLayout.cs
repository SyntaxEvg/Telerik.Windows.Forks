using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	class PageLayout
	{
		internal int PagesCount
		{
			get
			{
				return this.pages.Count;
			}
		}

		internal IEnumerable<WorksheetPageLayoutBox> Pages
		{
			get
			{
				return this.pages;
			}
		}

		public PageLayout()
		{
			this.pages = new List<WorksheetPageLayoutBox>();
			this.isPreparingPages = false;
		}

		internal WorksheetPageLayoutBox GetPage(int pageIndex)
		{
			return this.pages[pageIndex];
		}

		internal void PreparePages(IEnumerable<WorksheetPageLayoutBox> pages, int fromPageIndex, int toPageIndex, int copies, bool areOrderedCollated)
		{
			List<WorksheetPageLayoutBox> list = new List<WorksheetPageLayoutBox>(pages);
			List<WorksheetPageLayoutBox> list2 = new List<WorksheetPageLayoutBox>();
			using (this.BeginPrintPagesPreparation())
			{
				if (areOrderedCollated)
				{
					for (int i = 0; i < copies; i++)
					{
						for (int j = fromPageIndex; j <= toPageIndex; j++)
						{
							list2.Add(new WorksheetPageLayoutBox(list[j]));
						}
					}
				}
				else
				{
					for (int k = fromPageIndex; k <= toPageIndex; k++)
					{
						for (int l = 0; l < copies; l++)
						{
							list2.Add(new WorksheetPageLayoutBox(list[k]));
						}
					}
				}
				this.pages.AddRange(list2);
			}
		}

		internal void PreparePages(Worksheet worksheet, IEnumerable<CellRange> selectedRanges)
		{
			using (this.BeginPrintPagesPreparation())
			{
				foreach (CellRange printAreaRange in selectedRanges)
				{
					this.pages.AddRange(this.CalculatePages(worksheet, printAreaRange, true));
				}
			}
		}

		internal void PreparePages(Worksheet worksheet, bool ignorePrintArea)
		{
			using (this.BeginPrintPagesPreparation())
			{
				this.AddPages(worksheet, ignorePrintArea);
			}
		}

		internal void PreparePages(Workbook workbook, bool ignorePrintArea)
		{
			using (this.BeginPrintPagesPreparation())
			{
				foreach (Worksheet worksheet in workbook.Worksheets)
				{
					this.AddPages(worksheet, ignorePrintArea);
				}
			}
		}

		internal Size GetMaximumRowColumnHeadingsContent(RadWorksheetLayout layout, Size scaleFactor)
		{
			CellRange visibleRange = new CellRange(0, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			double width = layout.GetRowHeadingSize(visibleRange, SpreadsheetDefaultValues.RowCount - 1).Width;
			double height = layout.GetColumnHeadingSize(SpreadsheetDefaultValues.ColumnCount - 1).Height;
			return new Size(width * scaleFactor.Width, height * scaleFactor.Height);
		}

		void AddPages(Worksheet worksheet, bool ignorePrintArea)
		{
			WorksheetPageSetup worksheetPageSetup = worksheet.WorksheetPageSetup;
			if (ignorePrintArea || !worksheetPageSetup.PrintArea.HasPrintAreaRanges)
			{
				this.pages.AddRange(this.CalculatePages(worksheet, worksheet.TotalUsedCellRange, true));
				return;
			}
			foreach (CellRange printAreaRange in worksheetPageSetup.PrintArea.Ranges)
			{
				this.pages.AddRange(this.CalculatePages(worksheet, printAreaRange, false));
			}
		}

		IEnumerable<WorksheetPageLayoutBox> CalculatePages(Worksheet worksheet, CellRange printAreaRange, bool intersectPageBreaksAsInfinite)
		{
			WorksheetPageSetup pageSetup = worksheet.WorksheetPageSetup;
			IEnumerable<PageBreak> sortedHorizontalPageBreaks = pageSetup.PageBreaks.SortedHorizontalPageBreaks;
			IEnumerable<PageBreak> sortedVerticalPageBreaks = pageSetup.PageBreaks.SortedVerticalPageBreaks;
			PageOrder pageOrder = pageSetup.PageOrder;
			IEnumerable<CellRange> subPrintAreaRanges = PageLayout.GetSubCellRanges(printAreaRange, sortedHorizontalPageBreaks, sortedVerticalPageBreaks, pageOrder, intersectPageBreaksAsInfinite);
			foreach (CellRange subPrintAreaRange in subPrintAreaRanges)
			{
				foreach (WorksheetPageLayoutBox printPage in this.CalculatePagesIgnoringPageBreaks(worksheet, subPrintAreaRange))
				{
					yield return printPage;
				}
			}
			yield break;
		}

		static IEnumerable<CellRange> GetSubCellRanges(CellRange printAreaRange, IEnumerable<PageBreak> sortedHorizontalPageBreaks, IEnumerable<PageBreak> sortedVerticalPageBreaks, PageOrder pageOrder, bool intersectPageBreaksAsInfinite)
		{
			List<int> horizontalBoundaryIndexes = PageLayout.GetRangeBoundaryIndexes(sortedVerticalPageBreaks, printAreaRange.FromIndex.ColumnIndex, printAreaRange.ToIndex.ColumnIndex, (PageBreak pageBreak) => pageBreak.FromIndex <= printAreaRange.FromIndex.RowIndex && printAreaRange.ToIndex.RowIndex <= pageBreak.ToIndex, intersectPageBreaksAsInfinite).ToList<int>();
			List<int> verticalBoundaryIndexes = PageLayout.GetRangeBoundaryIndexes(sortedHorizontalPageBreaks, printAreaRange.FromIndex.RowIndex, printAreaRange.ToIndex.RowIndex, (PageBreak pageBreak) => pageBreak.FromIndex <= printAreaRange.FromIndex.ColumnIndex && printAreaRange.ToIndex.ColumnIndex <= pageBreak.ToIndex, intersectPageBreaksAsInfinite).ToList<int>();
			if (pageOrder == PageOrder.DownThenOver)
			{
				return PageLayout.GetSubRangesDownThenOver(horizontalBoundaryIndexes, verticalBoundaryIndexes);
			}
			return PageLayout.GetSubRangesOverThenDown(horizontalBoundaryIndexes, verticalBoundaryIndexes);
		}

		static IEnumerable<int> GetRangeBoundaryIndexes(IEnumerable<PageBreak> sortedPageBreaksCollection, int rangeStart, int rangeEnd, Func<PageBreak, bool> isIntersectingPageBreak, bool intersectPageBreaksAsInfinite)
		{
			int lastBoundaryIndex = rangeStart;
			yield return rangeStart;
			foreach (PageBreak pageBreak in sortedPageBreaksCollection)
			{
				int currentIndex = pageBreak.Index;
				if (currentIndex > rangeEnd)
				{
					break;
				}
				bool intersects = currentIndex > lastBoundaryIndex && (intersectPageBreaksAsInfinite || isIntersectingPageBreak(pageBreak));
				if (intersects)
				{
					lastBoundaryIndex = currentIndex;
					yield return lastBoundaryIndex - 1;
					yield return lastBoundaryIndex;
				}
			}
			yield return rangeEnd;
			yield break;
		}

		static IEnumerable<CellRange> GetSubRangesDownThenOver(List<int> horizontalBoundaryIndexes, List<int> verticalBoundaryIndexes)
		{
			for (int horizontalIndex = 0; horizontalIndex < horizontalBoundaryIndexes.Count; horizontalIndex += 2)
			{
				for (int verticalIndex = 0; verticalIndex < verticalBoundaryIndexes.Count; verticalIndex += 2)
				{
					int fromRow = verticalBoundaryIndexes[verticalIndex];
					int toRow = verticalBoundaryIndexes[verticalIndex + 1];
					int fromColumn = horizontalBoundaryIndexes[horizontalIndex];
					int toColumn = horizontalBoundaryIndexes[horizontalIndex + 1];
					yield return new CellRange(fromRow, fromColumn, toRow, toColumn);
				}
			}
			yield break;
		}

		static IEnumerable<CellRange> GetSubRangesOverThenDown(List<int> horizontalBoundaryIndexes, List<int> verticalBoundaryIndexes)
		{
			for (int verticalIndex = 0; verticalIndex < verticalBoundaryIndexes.Count; verticalIndex += 2)
			{
				for (int horizontalIndex = 0; horizontalIndex < horizontalBoundaryIndexes.Count; horizontalIndex += 2)
				{
					int fromRow = verticalBoundaryIndexes[verticalIndex];
					int toRow = verticalBoundaryIndexes[verticalIndex + 1];
					int fromColumn = horizontalBoundaryIndexes[horizontalIndex];
					int toColumn = horizontalBoundaryIndexes[horizontalIndex + 1];
					yield return new CellRange(fromRow, fromColumn, toRow, toColumn);
				}
			}
			yield break;
		}

		IEnumerable<WorksheetPageLayoutBox> CalculatePagesIgnoringPageBreaks(Worksheet worksheet, CellRange cellRange)
		{
			RadWorksheetLayout layout = worksheet.Workbook.GetWorksheetLayout(worksheet, true);
			WorksheetPageSetup worksheetPageSetup = worksheet.WorksheetPageSetup;
			Size scaleFactor = (worksheetPageSetup.FitToPages ? PageScaleFactorCalculator.CalculateScaleAccordingToFitToPages(worksheet) : worksheetPageSetup.ScaleFactor);
			Size minimumContent = PageScaleFactorCalculator.CalculateRowHeadingsWidthColumnHeadingsHeight(worksheet, cellRange, scaleFactor);
			if (!PageMargins.AreValidMarginsWithinPageSize(worksheetPageSetup.Margins, worksheetPageSetup.RotatedPageSize, minimumContent))
			{
				throw new PrintingException("Margins do not fit page size.", "Spreadsheet_ErrorExpressions_MarginsDoNotFitPageSize");
			}
			double maxDimensionLength = worksheetPageSetup.RotatedPageSize.Height - worksheetPageSetup.Margins.Top - worksheetPageSetup.Margins.Bottom - minimumContent.Height;
			List<PageLayout.RangeDimensionInfo> splittedDimensionRanges = PageLayout.GetSplittedDimensionRanges(cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, maxDimensionLength, (int index) => layout.GetRowHeight(index) * scaleFactor.Height);
			double maxDimensionLength2 = worksheetPageSetup.RotatedPageSize.Width - worksheetPageSetup.Margins.Left - worksheetPageSetup.Margins.Right - minimumContent.Width;
			List<PageLayout.RangeDimensionInfo> splittedDimensionRanges2 = PageLayout.GetSplittedDimensionRanges(cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex, maxDimensionLength2, (int index) => layout.GetColumnWidth(index) * scaleFactor.Width);
			Point initialOffset = new Point(layout.GetColumnLeft(cellRange.FromIndex.ColumnIndex), layout.GetRowTop(cellRange.FromIndex.RowIndex));
			if (worksheetPageSetup.PageOrder == PageOrder.DownThenOver)
			{
				return PageLayout.GetPagesDownThenOver(worksheet, splittedDimensionRanges, splittedDimensionRanges2, initialOffset, scaleFactor);
			}
			return PageLayout.GetPagesOverThenDown(worksheet, splittedDimensionRanges, splittedDimensionRanges2, initialOffset, scaleFactor);
		}

		static CellRange RangeDimensionsToCellRange(PageLayout.RangeDimensionInfo rowPrintRange, PageLayout.RangeDimensionInfo columnPrintRange)
		{
			return new CellRange(rowPrintRange.StartIndex, columnPrintRange.StartIndex, rowPrintRange.EndIndex, columnPrintRange.EndIndex);
		}

		static IEnumerable<WorksheetPageLayoutBox> GetPagesOverThenDown(Worksheet worksheet, List<PageLayout.RangeDimensionInfo> viewportHeightDimensions, List<PageLayout.RangeDimensionInfo> viewportWidthDimensions, Point initialOffset, Size scaleFactor)
		{
			double topOffset = initialOffset.Y;
			for (int rowIndex = 0; rowIndex < viewportHeightDimensions.Count; rowIndex++)
			{
				double leftOffset = initialOffset.X;
				for (int columnIndex = 0; columnIndex < viewportWidthDimensions.Count; columnIndex++)
				{
					PageLayout.RangeDimensionInfo rowPrintRange = viewportHeightDimensions[rowIndex];
					PageLayout.RangeDimensionInfo columnPrintRange = viewportWidthDimensions[columnIndex];
					CellRange range = PageLayout.RangeDimensionsToCellRange(rowPrintRange, columnPrintRange);
					Rect viewPortRectangle = new Rect(leftOffset, topOffset, columnPrintRange.Length, rowPrintRange.Length);
					yield return new WorksheetPageLayoutBox(worksheet, range, viewPortRectangle, scaleFactor);
					leftOffset += viewportWidthDimensions[columnIndex].Length / scaleFactor.Width;
				}
				topOffset += viewportHeightDimensions[rowIndex].Length / scaleFactor.Height;
			}
			yield break;
		}

		static IEnumerable<WorksheetPageLayoutBox> GetPagesDownThenOver(Worksheet worksheet, List<PageLayout.RangeDimensionInfo> viewportHeightDimensions, List<PageLayout.RangeDimensionInfo> viewportWidthDimensions, Point initialOffset, Size scaleFactor)
		{
			double leftOffset = initialOffset.X;
			for (int columnIndex = 0; columnIndex < viewportWidthDimensions.Count; columnIndex++)
			{
				double topOffset = initialOffset.Y;
				for (int rowIndex = 0; rowIndex < viewportHeightDimensions.Count; rowIndex++)
				{
					PageLayout.RangeDimensionInfo rowPrintRange = viewportHeightDimensions[rowIndex];
					PageLayout.RangeDimensionInfo columnPrintRange = viewportWidthDimensions[columnIndex];
					CellRange range = PageLayout.RangeDimensionsToCellRange(rowPrintRange, columnPrintRange);
					Rect viewPortRectangle = new Rect(leftOffset, topOffset, columnPrintRange.Length, rowPrintRange.Length);
					yield return new WorksheetPageLayoutBox(worksheet, range, viewPortRectangle, scaleFactor);
					topOffset += viewportHeightDimensions[rowIndex].Length / scaleFactor.Height;
				}
				leftOffset += viewportWidthDimensions[columnIndex].Length / scaleFactor.Width;
			}
			yield break;
		}

		static List<PageLayout.RangeDimensionInfo> GetSplittedDimensionRanges(int startIndex, int endIndex, double maxDimensionLength, Func<int, double> indexToDimensionLength)
		{
			List<PageLayout.RangeDimensionInfo> list = new List<PageLayout.RangeDimensionInfo>();
			double num = 0.0;
			for (int i = startIndex; i <= endIndex; i++)
			{
				double num2 = indexToDimensionLength(i);
				num += num2;
				if (num > maxDimensionLength)
				{
					if (i == startIndex)
					{
						list.Add(new PageLayout.RangeDimensionInfo(startIndex, i, maxDimensionLength));
						startIndex = i + 1;
						num = 0.0;
					}
					else
					{
						list.Add(new PageLayout.RangeDimensionInfo(startIndex, i - 1, num - num2));
						startIndex = i;
						num = num2;
					}
				}
			}
			if (startIndex <= endIndex)
			{
				list.Add(new PageLayout.RangeDimensionInfo(startIndex, endIndex, num));
			}
			return list;
		}

		IDisposable BeginPrintPagesPreparation()
		{
			Guard.ThrowExceptionIfTrue(this.isPreparingPages, "isPreparingPages");
			this.isPreparingPages = true;
			this.pages.Clear();
			return new DisposableObject(new Action(this.EndPrintPagesPreparation));
		}

		void EndPrintPagesPreparation()
		{
			Guard.ThrowExceptionIfFalse(this.isPreparingPages, "isPreparingPages");
			DateTime now = DateTime.Now;
			for (int i = 0; i < this.PagesCount; i++)
			{
				this.pages[i].PrepareHeaderFooterContext(i + 1, this.PagesCount, now);
			}
			this.isPreparingPages = false;
		}

		readonly List<WorksheetPageLayoutBox> pages;

		bool isPreparingPages;

		class RangeDimensionInfo
		{
			public int StartIndex { get; set; }

			public int EndIndex { get; set; }

			public double Length { get; set; }

			public RangeDimensionInfo(int startIndex, int endIndex, double length)
			{
				this.StartIndex = startIndex;
				this.EndIndex = endIndex;
				this.Length = length;
			}
		}
	}
}
