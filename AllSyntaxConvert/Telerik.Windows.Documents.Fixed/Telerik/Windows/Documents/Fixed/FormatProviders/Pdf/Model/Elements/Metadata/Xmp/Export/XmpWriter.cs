using System;
using System.IO;
using System.Text;
using System.Xml;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export
{
	class XmpWriter : IXmpWriter, IXmlWriter, IDisposable
	{
		public XmpWriter(Stream output)
		{
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			this.writer = XmlWriter.Create(output, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				CheckCharacters = true,
				Encoding = new UTF8Encoding(false),
				Indent = true,
				NamespaceHandling = NamespaceHandling.OmitDuplicates,
				NewLineHandling = NewLineHandling.None,
				NewLineOnAttributes = false
			});
		}

		public void WriteElementStart(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.writer.WriteStartElement(name);
		}

		public void WriteElementStart(string prefix, string name, string ns)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNullOrEmpty(ns, "ns");
			this.writer.WriteStartElement(prefix, name, ns);
		}

		public void WriteAttribute(string name, string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.writer.WriteAttributeString(name, value);
		}

		public void WriteAttribute(string ns, string name, string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			Guard.ThrowExceptionIfNullOrEmpty(ns, "ns");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.writer.WriteAttributeString(ns, name, null, value);
		}

		public void WriteValue(string value)
		{
			this.writer.WriteRaw(value);
		}

		public void WriteRaw(string data)
		{
			this.writer.WriteRaw(data);
		}

		public void WriteRawLine()
		{
			this.WriteRaw("\r\n");
		}

		public void WriteElementEnd()
		{
			this.writer.WriteEndElement();
			this.writer.Flush();
		}

		public void Dispose()
		{
			this.writer.Flush();
			this.writer.Close();
		}

		readonly XmlWriter writer;
	}
}
