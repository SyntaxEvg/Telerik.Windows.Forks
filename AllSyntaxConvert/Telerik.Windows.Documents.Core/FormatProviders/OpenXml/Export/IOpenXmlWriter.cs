using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Export
{
	interface IOpenXmlWriter
	{
		void WriteElementStart(OpenXmlElementBase element);

		void WriteAttribute(OpenXmlAttributeBase attribute);

		void WriteValue(string value);

		void WriteElementEnd();

		void WriteNamespace(OpenXmlNamespace ns);
	}
}
