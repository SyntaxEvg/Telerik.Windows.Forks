using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class BodyElement : BlockContainerElementBase
	{
		public BodyElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "body";
			}
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (Section section in context.Document.Sections)
			{
				foreach (HtmlElementBase element in base.ContentManager.ExportBlockContainer(context, section))
				{
					yield return element;
				}
			}
			yield break;
		}
	}
}
