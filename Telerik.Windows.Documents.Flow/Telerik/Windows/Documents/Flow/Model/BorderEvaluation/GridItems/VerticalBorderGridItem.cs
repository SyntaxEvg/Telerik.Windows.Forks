using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.BorderEvaluation.GridItems
{
	class VerticalBorderGridItem : BorderGridItemBase
	{
		public VerticalBorderGridItem()
		{
			base.Border = new Border(0.0, BorderStyle.Inherit, new ThemableColor(Colors.Transparent));
			this.IsInsideCell = true;
		}

		public bool IsInsideCell { get; set; }

		public int RowSpan { get; set; }
	}
}
