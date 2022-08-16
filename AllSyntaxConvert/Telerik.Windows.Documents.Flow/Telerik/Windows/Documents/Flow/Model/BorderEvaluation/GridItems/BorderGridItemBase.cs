using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems
{
	class BorderGridItemBase
	{
		public int ColumnIndex { get; set; }

		public int RowIndex { get; set; }

		public Border Border { get; set; }

		public bool IsTableBorder { get; set; }
	}
}
