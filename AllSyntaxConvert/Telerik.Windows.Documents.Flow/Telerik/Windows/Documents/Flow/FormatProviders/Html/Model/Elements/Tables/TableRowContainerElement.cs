using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	abstract class TableRowContainerElement : BodyElementBase
	{
		public TableRowContainerElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		protected abstract RowType RowType { get; }

		protected override void OnBeforeReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			base.OnBeforeReadChildElement(reader, context, innerElement);
			string name;
			if ((name = innerElement.Name) != null)
			{
				if (!(name == "tr"))
				{
					return;
				}
				((TableRowElement)innerElement).SetRowType(this.RowType);
			}
		}
	}
}
