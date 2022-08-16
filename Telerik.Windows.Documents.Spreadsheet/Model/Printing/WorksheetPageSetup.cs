using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class WorksheetPageSetup : SheetPageSetup<WorksheetPrintOptions>
	{
		public PageOrder PageOrder
		{
			get
			{
				return this.pageOrder;
			}
			set
			{
				if (this.pageOrder != value)
				{
					this.pageOrder = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public bool CenterHorizontally
		{
			get
			{
				return this.centerHorizontally;
			}
			set
			{
				if (this.centerHorizontally != value)
				{
					this.centerHorizontally = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public bool CenterVertically
		{
			get
			{
				return this.centerVertically;
			}
			set
			{
				if (this.centerVertically != value)
				{
					this.centerVertically = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public PrintArea PrintArea
		{
			get
			{
				return this.printArea;
			}
		}

		public PageBreaks PageBreaks
		{
			get
			{
				return this.pageBreaks;
			}
		}

		public Size ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				if (this.scaleFactor != value)
				{
					Guard.ThrowExceptionIfOutOfRange<double>(SpreadsheetDefaultValues.MinPageScaleFactor, SpreadsheetDefaultValues.MaxPageScaleFactor, value.Width, "scaleFactor");
					Guard.ThrowExceptionIfOutOfRange<double>(SpreadsheetDefaultValues.MinPageScaleFactor, SpreadsheetDefaultValues.MaxPageScaleFactor, value.Height, "scaleFactor");
					this.scaleFactor = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public int FitToPagesTall
		{
			get
			{
				return this.fitToPagesTall;
			}
			set
			{
				if (this.fitToPagesTall != value)
				{
					Guard.ThrowExceptionIfOutOfRange<int>(0, 32767, value, "fitToPagesTall");
					this.fitToPagesTall = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public int FitToPagesWide
		{
			get
			{
				return this.fitToPagesWide;
			}
			set
			{
				if (this.fitToPagesWide != value)
				{
					Guard.ThrowExceptionIfOutOfRange<int>(0, 32767, value, "fitToPagesWide");
					this.fitToPagesWide = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public bool FitToPages
		{
			get
			{
				return this.fitToPages;
			}
			set
			{
				if (this.fitToPages != value)
				{
					this.fitToPages = value;
					base.OnPageSetupChanged();
				}
			}
		}

		public override WorksheetPrintOptions PrintOptions
		{
			get
			{
				return this.printOptions;
			}
		}

		internal WorksheetPageSetup(Worksheet worksheet)
			: base(worksheet)
		{
			this.ScaleFactor = SpreadsheetDefaultValues.DefaultScaleFactorSize;
			this.printOptions = new WorksheetPrintOptions(this);
			this.fitToPages = false;
			this.fitToPagesTall = 1;
			this.fitToPagesWide = 1;
			this.printArea = new PrintArea(worksheet);
			this.pageBreaks = new PageBreaks(worksheet);
			this.AttachToEvents();
			this.PageOrder = PageOrder.DownThenOver;
		}

		internal override void CopyFrom(SheetPageSetupBase fromSheetPageSetupBase)
		{
			base.CopyFrom(fromSheetPageSetupBase);
			WorksheetPageSetup worksheetPageSetup = (WorksheetPageSetup)fromSheetPageSetupBase;
			this.printOptions.CopyFrom(worksheetPageSetup.PrintOptions);
			this.printArea.CopyFrom(worksheetPageSetup.PrintArea);
			this.pageBreaks.CopyFrom(worksheetPageSetup.PageBreaks);
			this.pageOrder = worksheetPageSetup.PageOrder;
			this.centerHorizontally = worksheetPageSetup.CenterHorizontally;
			this.centerVertically = worksheetPageSetup.CenterVertically;
			this.scaleFactor = worksheetPageSetup.ScaleFactor;
			base.RotatedPageSize = worksheetPageSetup.RotatedPageSize;
			this.fitToPages = worksheetPageSetup.FitToPages;
			this.fitToPagesTall = worksheetPageSetup.FitToPagesTall;
			this.fitToPagesWide = worksheetPageSetup.FitToPagesWide;
		}

		internal void Clear()
		{
			this.PrintArea.Clear();
			this.PageBreaks.Clear();
			base.HeaderFooterSettings.Clear();
		}

		void AttachToEvents()
		{
			this.PrintArea.PrintAreaChanged += this.PrintAreaChanged;
			this.PageBreaks.PageBreaksChanged += this.PageBreaksChanged;
		}

		void PrintAreaChanged(object sender, EventArgs args)
		{
			using (new UpdateScope(new Action(base.SuspendSheetLayoutUpdate), new Action(base.ResumeSheetLayoutUpdate)))
			{
				if (!this.PrintArea.HasPrintAreaRanges)
				{
					this.PageBreaks.MakeAllPageBreaksInfinite();
				}
				base.InvalidateSheetLayout();
			}
		}

		void PageBreaksChanged(object sender, EventArgs args)
		{
			base.InvalidateSheetLayout();
		}

		bool fitToPages;

		int fitToPagesTall;

		int fitToPagesWide;

		Size scaleFactor;

		readonly PrintArea printArea;

		readonly PageBreaks pageBreaks;

		readonly WorksheetPrintOptions printOptions;

		PageOrder pageOrder;

		bool centerHorizontally;

		bool centerVertically;
	}
}
