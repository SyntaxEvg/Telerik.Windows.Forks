using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model
{
	class HtmlElementsPool : ElementsPoolBase<HtmlElementBase>
	{
		public HtmlElementsPool(HtmlContentManager contentManager)
		{
			Guard.ThrowExceptionIfNull<HtmlContentManager>(contentManager, "contentManager");
			this.contentManager = contentManager;
		}

		protected override HtmlElementBase CreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			HtmlElementBase result;
			HtmlElementsFactory.TryCreateInstance(this.contentManager, elementName, out result);
			return result;
		}

		readonly HtmlContentManager contentManager;
	}
}
