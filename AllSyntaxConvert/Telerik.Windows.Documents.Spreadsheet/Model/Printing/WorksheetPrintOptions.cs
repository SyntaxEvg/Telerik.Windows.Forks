using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class WorksheetPrintOptions : SheetPrintOptionsBase
	{
		public bool PrintGridlines
		{
			get
			{
				return this.printGridlines;
			}
			set
			{
				if (this.printGridlines != value)
				{
					this.printGridlines = value;
					base.OnPrintOptionChanged();
				}
			}
		}

		public bool PrintRowColumnHeadings
		{
			get
			{
				return this.printRowColumnHeadings;
			}
			set
			{
				if (this.printRowColumnHeadings != value)
				{
					this.printRowColumnHeadings = value;
					base.OnPrintOptionChanged();
				}
			}
		}

		internal ErrorsPrintStyle ErrorsPrintStyle
		{
			get
			{
				return this.errorsPrintStyle;
			}
			set
			{
				if (this.errorsPrintStyle != value)
				{
					this.errorsPrintStyle = value;
					base.OnPrintOptionChanged();
				}
			}
		}

		internal PrintLocation Comments
		{
			get
			{
				return this.printLocation;
			}
			set
			{
				if (this.printLocation != value)
				{
					this.printLocation = value;
					base.OnPrintOptionChanged();
				}
			}
		}

		internal bool PrintGridlinesOutline
		{
			get
			{
				return this.PrintGridlines || this.PrintRowColumnHeadings;
			}
		}

		internal WorksheetPrintOptions(WorksheetPageSetup pageSetup)
			: base(pageSetup)
		{
		}

		internal override void CopyFrom(SheetPrintOptionsBase fromSheetPrintOptionsBase)
		{
			base.CopyFrom(fromSheetPrintOptionsBase);
			WorksheetPrintOptions worksheetPrintOptions = (WorksheetPrintOptions)fromSheetPrintOptionsBase;
			this.errorsPrintStyle = worksheetPrintOptions.ErrorsPrintStyle;
			this.printGridlines = worksheetPrintOptions.PrintGridlines;
			this.printRowColumnHeadings = worksheetPrintOptions.PrintRowColumnHeadings;
			this.printLocation = worksheetPrintOptions.Comments;
		}

		bool printGridlines;

		bool printRowColumnHeadings;

		ErrorsPrintStyle errorsPrintStyle;

		PrintLocation printLocation;
	}
}
