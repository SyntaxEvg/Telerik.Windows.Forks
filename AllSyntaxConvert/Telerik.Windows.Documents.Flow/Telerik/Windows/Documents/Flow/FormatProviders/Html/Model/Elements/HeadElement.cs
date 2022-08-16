using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class HeadElement : HtmlElementBase
	{
		public HeadElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "head";
			}
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			switch (context.Settings.StylesExportMode)
			{
			case StylesExportMode.External:
			{
				LinkElement link = base.CreateElement<LinkElement>("link");
				yield return link;
				break;
			}
			case StylesExportMode.Embedded:
			{
				StyleElement style = base.CreateElement<StyleElement>("style");
				yield return style;
				break;
			}
			}
			yield return base.CreateElement<TitleElement>("title");
			yield break;
		}
	}
}
