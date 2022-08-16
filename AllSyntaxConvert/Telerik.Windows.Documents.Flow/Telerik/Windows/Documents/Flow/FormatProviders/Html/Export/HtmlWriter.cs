using System;
using System.IO;
using System.Text;
using System.Xml;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class HtmlWriter : IHtmlWriter, IXmlWriter, IDisposable
	{
		public HtmlWriter(Stream output, HtmlExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			switch (settings.DocumentExportLevel)
			{
			case DocumentExportLevel.Document:
				xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
				break;
			case DocumentExportLevel.Fragment:
				xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
				break;
			}
			xmlWriterSettings.OmitXmlDeclaration = true;
			xmlWriterSettings.CheckCharacters = true;
			xmlWriterSettings.Encoding = Encoding.UTF8;
			xmlWriterSettings.Indent = settings.IndentDocument;
			xmlWriterSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
			xmlWriterSettings.NewLineHandling = NewLineHandling.None;
			xmlWriterSettings.NewLineOnAttributes = false;
			this.writer = XmlWriter.Create(output, xmlWriterSettings);
		}

		public void WriteDocumentStart()
		{
			this.writer.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);
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

		public void WriteValue(string value, bool preserveWhiteSpaces)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			HtmlTextProcessor textProcessor = HtmlTextProcessor.TextProcessor;
			value = textProcessor.EscapeHtml(value);
			if (preserveWhiteSpaces)
			{
				value = HtmlTextProcessor.PreserveWhiteSpaces(value);
			}
			this.writer.WriteRaw(value);
		}

		public void WriteElementEnd(bool forceFullEndElement)
		{
			if (forceFullEndElement)
			{
				this.writer.WriteFullEndElement();
				return;
			}
			this.writer.WriteEndElement();
		}

		public void Dispose()
		{
			this.writer.Flush();
			this.writer.Close();
		}

		readonly XmlWriter writer;
	}
}
