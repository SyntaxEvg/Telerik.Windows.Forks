using System;
using System.Xml;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Import
{
	class OpenXmlPartReader : IOpenXmlReader
	{
		public OpenXmlPartReader(XmlReader reader)
		{
			Guard.ThrowExceptionIfNull<XmlReader>(reader, "reader");
			this.reader = reader;
		}

		XmlNodeType IOpenXmlReader.GetNodeType()
		{
			if (this.reader == null)
			{
				return XmlNodeType.None;
			}
			return this.reader.NodeType;
		}

		bool IOpenXmlReader.IsEmptyElement()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.IsEmptyElement;
		}

		bool IOpenXmlReader.HasAttributes()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.HasAttributes;
		}

		bool IOpenXmlReader.MoveToFirstAttribute()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.MoveToFirstAttribute();
		}

		bool IOpenXmlReader.MoveToNextAttribute()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.MoveToNextAttribute();
		}

		bool IOpenXmlReader.IsElementOfType(XmlNodeType type)
		{
			return this.reader != null && this.reader.NodeType == type;
		}

		bool IOpenXmlReader.Read()
		{
			return this.reader != null && this.reader.Read();
		}

		void IOpenXmlReader.SkipElement()
		{
			if (this.reader == null)
			{
				return;
			}
			this.reader.Skip();
		}

		bool IOpenXmlReader.GetElementName(out string elementName)
		{
			elementName = string.Empty;
			if (this.reader == null)
			{
				return false;
			}
			if (((IOpenXmlReader)this).IsElementOfType(XmlNodeType.Element))
			{
				elementName = this.reader.LocalName;
				return true;
			}
			return false;
		}

		bool IOpenXmlReader.GetElementValue(out string value)
		{
			if (this.reader == null)
			{
				value = string.Empty;
				return false;
			}
			value = this.reader.Value;
			return true;
		}

		bool IOpenXmlReader.GetAttributeNameAndValue(out string attributeName, out string value)
		{
			attributeName = string.Empty;
			value = string.Empty;
			if (this.reader == null)
			{
				return false;
			}
			if (((IOpenXmlReader)this).IsElementOfType(XmlNodeType.Attribute))
			{
				attributeName = this.reader.Name;
				value = this.reader.Value;
				return true;
			}
			return false;
		}

		readonly XmlReader reader;
	}
}
