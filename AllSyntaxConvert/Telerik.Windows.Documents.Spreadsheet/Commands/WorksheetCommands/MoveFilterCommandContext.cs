using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class MoveFilterCommandContext : WorksheetCommandContextBase
	{
		public IFilter NewFilter
		{
			get
			{
				return this.newFilter;
			}
			set
			{
				this.newFilter = value;
			}
		}

		public IFilter Filter
		{
			get
			{
				return this.filter;
			}
		}

		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		public int OldIndex
		{
			get
			{
				return this.oldIndex;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public MoveFilterCommandContext(Worksheet worksheet, IFilter filter, int newIndex)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			this.filter = filter;
			this.newIndex = newIndex;
			this.oldIndex = filter.RelativeColumnIndex;
		}

		readonly IFilter filter;

		IFilter newFilter;

		readonly int newIndex;

		readonly int oldIndex;
	}
}
