using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	interface IHtmlWriter : IXmlWriter
	{
		void WriteDocumentStart();

		void WriteElementEnd(bool forceFullEndElement);

		void WriteValue(string value, bool preserveWhiteSpaces);
	}
}
