using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export
{
	interface IXmpWriter : IXmlWriter
	{
		void WriteRaw(string data);

		void WriteRawLine();

		void WriteValue(string value);

		void WriteElementEnd();
	}
}
