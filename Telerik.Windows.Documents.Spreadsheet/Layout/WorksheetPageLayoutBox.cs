using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	class WorksheetPageLayoutBox : LayoutBox
	{
		public WorksheetPageLayoutBox(Worksheet worksheet, CellRange range, Rect viewportRectangle, Size scaleFactor)
			: base(viewportRectangle)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<CellRange>(range, "range");
			this.Worksheet = worksheet;
			this.Range = range;
			this.RowColumnHeadingsSize = WorksheetPageLayoutBox.CalculateRowColumnHeadings(worksheet, range, scaleFactor);
			this.PageContentBox = WorksheetPageLayoutBox.CalculatePageContentBox(this.Worksheet.WorksheetPageSetup, this.RowColumnHeadingsSize, base.Size);
			this.ScaleFactor = scaleFactor;
		}

		internal WorksheetPageLayoutBox(WorksheetPageLayoutBox other)
			: base(new Rect(other.Left, other.Top, other.Width, other.Height))
		{
			this.Worksheet = other.Worksheet;
			this.Range = other.Range;
			this.RowColumnHeadingsSize = other.RowColumnHeadingsSize;
			this.PageContentBox = other.PageContentBox;
			this.ScaleFactor = other.ScaleFactor;
		}

		public Size RowColumnHeadingsSize { get; set; }

		public Worksheet Worksheet { get; set; }

		public CellRange Range { get; set; }

		public Rect PageContentBox { get; set; }

		public Size ScaleFactor { get; set; }

		public HeaderFooterRenderContext HeaderFooterContext
		{
			get
			{
				return this.headerFooterContext;
			}
		}

		public void PrepareHeaderFooterContext(int pageNumber, int numberOfPages, DateTime dateAndTime)
		{
			Guard.ThrowExceptionIfNotNull<HeaderFooterRenderContext>(this.headerFooterContext, "headerFooterContext");
			this.headerFooterContext = new HeaderFooterRenderContext(this.Worksheet, dateAndTime, pageNumber, numberOfPages);
		}

		static Rect CalculatePageContentBox(WorksheetPageSetup pageSetup, Size rowColumnHeadings, Size viewportSize)
		{
			double num = viewportSize.Width + rowColumnHeadings.Width;
			double num2 = viewportSize.Height + rowColumnHeadings.Height;
			PageMargins margins = pageSetup.Margins;
			Size rotatedPageSize = pageSetup.RotatedPageSize;
			double num3 = (pageSetup.CenterHorizontally ? ((rotatedPageSize.Width - margins.Left - margins.Right - num) / 2.0) : 0.0);
			double num4 = (pageSetup.CenterVertically ? ((rotatedPageSize.Height - margins.Top - margins.Bottom - num2) / 2.0) : 0.0);
			double x = margins.Left + num3;
			double y = margins.Top + num4;
			return new Rect(x, y, num, num2);
		}

		static Size CalculateRowColumnHeadings(Worksheet worksheet, CellRange range, Size scaleFactor)
		{
			if (!worksheet.WorksheetPageSetup.PrintOptions.PrintRowColumnHeadings)
			{
				return default(Size);
			}
			RadWorksheetLayout worksheetLayout = worksheet.Workbook.GetWorksheetLayout(worksheet, true);
			double num = 0.0;
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				num = Math.Max(num, worksheetLayout.GetRowHeadingSize(range, i).Width * scaleFactor.Width);
			}
			double num2 = 0.0;
			for (int j = range.FromIndex.ColumnIndex; j <= range.ToIndex.ColumnIndex; j++)
			{
				num2 = Math.Max(num2, worksheetLayout.GetColumnHeadingSize(range.ToIndex.ColumnIndex).Height * scaleFactor.Height);
			}
			return new Size(num, num2);
		}

		public override bool Equals(object obj)
		{
			WorksheetPageLayoutBox worksheetPageLayoutBox = obj as WorksheetPageLayoutBox;
			return worksheetPageLayoutBox != null && (this.RowColumnHeadingsSize.Equals(worksheetPageLayoutBox.RowColumnHeadingsSize) && base.BoundingRectangle.Equals(worksheetPageLayoutBox.BoundingRectangle) && this.Worksheet.Name.Equals(worksheetPageLayoutBox.Worksheet.Name) && this.Range.Equals(worksheetPageLayoutBox.Range) && this.HeaderFooterContext.DateTime.Equals(worksheetPageLayoutBox.HeaderFooterContext.DateTime) && this.HeaderFooterContext.NumberOfPages.Equals(worksheetPageLayoutBox.HeaderFooterContext.NumberOfPages)) && this.HeaderFooterContext.PageNumber.Equals(worksheetPageLayoutBox.HeaderFooterContext.PageNumber);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.RowColumnHeadingsSize.GetHashCode(), base.BoundingRectangle.GetHashCode(), this.Worksheet.Name.GetHashCode(), this.Range.GetHashCode(), this.HeaderFooterContext.DateTime.GetHashCode(), this.HeaderFooterContext.NumberOfPages.GetHashCode(), this.HeaderFooterContext.PageNumber.GetHashCode());
		}

		HeaderFooterRenderContext headerFooterContext;
	}
}
