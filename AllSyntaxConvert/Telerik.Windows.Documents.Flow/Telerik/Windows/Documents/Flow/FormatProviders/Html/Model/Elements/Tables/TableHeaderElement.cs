using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableHeaderElement : TableRowContainerElement
	{
		public TableHeaderElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "thead";
			}
		}

		protected override RowType RowType
		{
			get
			{
				return RowType.Header;
			}
		}
	}
}
