using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class StyleElement : HtmlElementBase
	{
		public StyleElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.typeAttribute = base.RegisterAttribute<string>("type", false);
		}

		public string Type
		{
			get
			{
				return this.typeAttribute.Value;
			}
			set
			{
				this.typeAttribute.Value = value;
			}
		}

		public override string Name
		{
			get
			{
				return "style";
			}
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			this.Type = "text/css";
			base.InnerText = StyleExporter.Export(context);
		}

		protected override void OnBeforeRead(IHtmlReader reader, IHtmlImportContext context)
		{
			base.OnBeforeRead(reader, context);
			base.InnerText = string.Empty;
		}

		protected override void OnAfterReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			if (innerElement is TextElement)
			{
				base.InnerText += innerElement.InnerText;
			}
		}

		readonly HtmlAttribute<string> typeAttribute;
	}
}
