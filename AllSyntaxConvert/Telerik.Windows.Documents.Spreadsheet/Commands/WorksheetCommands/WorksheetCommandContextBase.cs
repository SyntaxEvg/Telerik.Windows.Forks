using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class WorksheetCommandContextBase : SheetCommandContextBase
	{
		public Worksheet Worksheet
		{
			get
			{
				return (Worksheet)base.Sheet;
			}
		}

		public virtual bool ShouldBringActiveCellIntoView
		{
			get
			{
				return true;
			}
		}

		public WorksheetCommandContextBase(Worksheet worksheet)
			: base(worksheet)
		{
		}
	}
}
