using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class HtmlElement : HtmlElementBase
	{
		public HtmlElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override XmlNamespace Namespace
		{
			get
			{
				return HtmlNamespaces.XHtmlNamespace;
			}
		}

		public override string Name
		{
			get
			{
				return "html";
			}
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			HeadElement head = base.CreateElement<HeadElement>("head");
			yield return head;
			BodyElement body = base.CreateElement<BodyElement>("body");
			yield return body;
			yield break;
		}
	}
}
