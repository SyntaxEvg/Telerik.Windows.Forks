using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableBodyElement : TableRowContainerElement
	{
		public TableBodyElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "tbody";
			}
		}

		protected override RowType RowType
		{
			get
			{
				return RowType.Body;
			}
		}
	}
}
