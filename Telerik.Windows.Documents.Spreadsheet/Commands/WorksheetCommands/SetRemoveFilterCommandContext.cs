using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetRemoveFilterCommandContext : WorksheetCommandContextBase
	{
		public IFilter NewFilter
		{
			get
			{
				return this.newFilter;
			}
		}

		public IFilter OldFilter
		{
			get
			{
				return this.oldFilter;
			}
		}

		public int RelativeColumnIndex
		{
			get
			{
				return this.relativeColumnIndex;
			}
		}

		public ICompressedList<bool> HiddenRowsState
		{
			get
			{
				return this.hiddenRowsState;
			}
			set
			{
				this.hiddenRowsState = value;
			}
		}

		public override bool ShouldBringActiveCellIntoView
		{
			get
			{
				return false;
			}
		}

		public SetRemoveFilterCommandContext(Worksheet worksheet, IFilter newFilter, IFilter oldFilter)
			: base(worksheet)
		{
			if (newFilter == null && oldFilter == null)
			{
				throw new ArgumentException("The new filter and the old filter cannot be both null");
			}
			if (newFilter != null && oldFilter != null)
			{
				Guard.ThrowExceptionIfNotEqual<int>(oldFilter.RelativeColumnIndex, newFilter.RelativeColumnIndex, "newFilter");
			}
			this.relativeColumnIndex = ((newFilter != null) ? newFilter.RelativeColumnIndex : oldFilter.RelativeColumnIndex);
			this.newFilter = newFilter;
			this.oldFilter = oldFilter;
		}

		readonly IFilter newFilter;

		readonly IFilter oldFilter;

		readonly int relativeColumnIndex;

		ICompressedList<bool> hiddenRowsState;
	}
}
