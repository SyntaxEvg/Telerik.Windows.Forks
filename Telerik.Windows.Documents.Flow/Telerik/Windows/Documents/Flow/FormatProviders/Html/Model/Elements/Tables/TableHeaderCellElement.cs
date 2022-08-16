using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableHeaderCellElement : TableCellElementBase
	{
		public TableHeaderCellElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "th";
			}
		}

		protected override CellType CellType
		{
			get
			{
				return CellType.Header;
			}
		}
	}
}
