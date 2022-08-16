using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class BlockElement : BodyElementBase
	{
		public BlockElement(HtmlContentManager contentManager, string name)
			: base(contentManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		protected override void OnBeforeRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			context.EndParagraph();
		}

		readonly string name;
	}
}
