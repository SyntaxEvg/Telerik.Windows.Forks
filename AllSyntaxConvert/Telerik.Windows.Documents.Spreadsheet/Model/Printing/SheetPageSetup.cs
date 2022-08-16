using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public abstract class SheetPageSetup<T> : SheetPageSetupBase where T : SheetPrintOptionsBase
	{
		public abstract T PrintOptions { get; }

		protected SheetPageSetup(Sheet sheet)
			: base(sheet)
		{
		}
	}
}
