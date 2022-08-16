using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	[Flags]
	public enum TableLooks
	{
		None = 0,
		FirstRow = 1,
		LastRow = 2,
		FirstColumn = 4,
		LastColumn = 8,
		BandedRows = 16,
		BandedColumns = 32
	}
}
