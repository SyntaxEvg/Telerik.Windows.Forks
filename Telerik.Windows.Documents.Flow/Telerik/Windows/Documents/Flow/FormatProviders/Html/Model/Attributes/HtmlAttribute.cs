using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	abstract class HtmlAttribute : XmlAttribute
	{
		public HtmlAttribute(string name, XmlNamespace ns, bool isRequired)
			: base(name, ns, isRequired)
		{
		}

		public virtual bool ShouldExport(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return this.ShouldExport();
		}
	}
}
