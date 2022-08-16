using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableFooterElement : TableRowContainerElement
	{
		public TableFooterElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "tfoot";
			}
		}

		protected override RowType RowType
		{
			get
			{
				return RowType.Footer;
			}
		}
	}
}
