using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableCellElement : TableCellElementBase
	{
		public TableCellElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "td";
			}
		}

		protected override CellType CellType
		{
			get
			{
				return CellType.Body;
			}
		}
	}
}
