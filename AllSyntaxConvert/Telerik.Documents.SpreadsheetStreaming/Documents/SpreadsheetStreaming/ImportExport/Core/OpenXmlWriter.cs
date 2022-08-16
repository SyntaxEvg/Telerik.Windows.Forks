using System;
using System.IO;
using System.Text;
using System.Xml;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class OpenXmlWriter
	{
		public OpenXmlWriter(Stream stream)
		{
			this.currentPartWriter = XmlWriter.Create(stream, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8
			});
		}

		public void WriteElementStart(ElementBase element)
		{
			if (this.currentPartWriter == null || element == null)
			{
				return;
			}
			if (element.Namespace != null)
			{
				this.currentPartWriter.WriteStartElement(element.Namespace.LocalName, element.ElementName, element.Namespace.Value);
				return;
			}
			this.currentPartWriter.WriteStartElement(element.ElementName);
		}

		public void WriteAttribute(OpenXmlAttributeBase attribute)
		{
			if (this.currentPartWriter == null || attribute == null)
			{
				return;
			}
			if (attribute.Namespace == null)
			{
				this.currentPartWriter.WriteAttributeString(attribute.Name, attribute.GetValueAsString());
				return;
			}
			this.currentPartWriter.WriteAttributeString(attribute.Namespace.LocalName, attribute.Name, null, attribute.GetValueAsString());
		}

		public void WriteNamespace(OpenXmlNamespace ns)
		{
			if (this.currentPartWriter == null || ns == null)
			{
				return;
			}
			if (ns.Prefix == null)
			{
				this.currentPartWriter.WriteAttributeString(ns.LocalName, ns.Value);
				return;
			}
			this.currentPartWriter.WriteAttributeString(ns.Prefix, ns.LocalName, null, ns.Value);
		}

		public void WriteValue(string value)
		{
			if (this.currentPartWriter == null || string.IsNullOrEmpty(value))
			{
				return;
			}
			value = IllegalXmlCharHelper.RemoveInvalidCharacters(value);
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			this.currentPartWriter.WriteValue(value);
		}

		public void WriteElementEnd()
		{
			if (this.currentPartWriter == null)
			{
				return;
			}
			this.currentPartWriter.WriteEndElement();
		}

		public void Flush()
		{
			this.currentPartWriter.Flush();
		}

		readonly XmlWriter currentPartWriter;
	}
}
