using System;
using System.Xml;
using Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Utilities;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Export
{
	class OpenXmlPartWriter : IOpenXmlWriter
	{
		public OpenXmlPartWriter(XmlWriter writer)
		{
			Guard.ThrowExceptionIfNull<XmlWriter>(writer, "writer");
			this.currentPartWriter = writer;
		}

		void IOpenXmlWriter.WriteElementStart(OpenXmlElementBase element)
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

		void IOpenXmlWriter.WriteAttribute(OpenXmlAttributeBase attribute)
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

		void IOpenXmlWriter.WriteNamespace(OpenXmlNamespace ns)
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

		void IOpenXmlWriter.WriteValue(string value)
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

		void IOpenXmlWriter.WriteElementEnd()
		{
			if (this.currentPartWriter == null)
			{
				return;
			}
			this.currentPartWriter.WriteEndElement();
		}

		readonly XmlWriter currentPartWriter;
	}
}
