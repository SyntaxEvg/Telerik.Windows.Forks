using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class GroupingProperties
	{
		public bool SummaryRowIsBelow { get; set; }

		public bool SummaryColumnIsToRight { get; set; }

		internal GroupingProperties()
		{
			this.SummaryRowIsBelow = true;
			this.SummaryColumnIsToRight = true;
		}

		internal void CopyFrom(GroupingProperties fromGroupingProperties)
		{
			this.SummaryColumnIsToRight = fromGroupingProperties.SummaryColumnIsToRight;
			this.SummaryRowIsBelow = fromGroupingProperties.SummaryRowIsBelow;
		}
	}
}
