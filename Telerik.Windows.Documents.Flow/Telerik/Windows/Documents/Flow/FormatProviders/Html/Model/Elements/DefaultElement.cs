using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class DefaultElement : BodyElementBase
	{
		public DefaultElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		public override string Name
		{
			get
			{
				return "DEFAULT";
			}
		}

		protected override void OnReadInnerText(IHtmlReader reader, IHtmlImportContext context, string text)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			TextElement textElement = base.CreateElement<TextElement>("TEXT");
			this.OnBeforeReadChildElement(reader, context, textElement);
			textElement.InnerText = text;
			this.OnAfterReadChildElement(reader, context, textElement);
		}
	}
}
