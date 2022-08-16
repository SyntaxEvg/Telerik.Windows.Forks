using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public abstract class SheetPrintOptionsBase
	{
		internal bool PrintBlackAndWhite
		{
			get
			{
				return this.printBlackAndWhite;
			}
			set
			{
				if (this.printBlackAndWhite != value)
				{
					this.printBlackAndWhite = value;
					this.OnPrintOptionChanged();
				}
			}
		}

		internal bool IsDraftQuality
		{
			get
			{
				return this.isDraftQuality;
			}
			set
			{
				if (this.isDraftQuality != value)
				{
					this.isDraftQuality = value;
					this.OnPrintOptionChanged();
				}
			}
		}

		internal SheetPrintOptionsBase(SheetPageSetupBase pageSetup)
		{
			this.pageSetup = pageSetup;
		}

		internal virtual void CopyFrom(SheetPrintOptionsBase fromSheetPrintOptionsBase)
		{
			this.printBlackAndWhite = fromSheetPrintOptionsBase.PrintBlackAndWhite;
			this.isDraftQuality = fromSheetPrintOptionsBase.IsDraftQuality;
		}

		internal void OnPrintOptionChanged()
		{
			this.pageSetup.OnPageSetupChanged();
		}

		readonly SheetPageSetupBase pageSetup;

		bool printBlackAndWhite;

		bool isDraftQuality;
	}
}
