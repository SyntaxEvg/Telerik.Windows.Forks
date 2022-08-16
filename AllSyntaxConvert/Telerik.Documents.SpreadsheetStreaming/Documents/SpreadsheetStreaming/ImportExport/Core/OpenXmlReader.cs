using System;
using System.IO;
using System.Xml;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class OpenXmlReader
	{
		public OpenXmlReader(Stream stream)
		{
			this.reader = XmlReader.Create(stream);
		}

		public bool IsEndOfElement()
		{
			return this.IsElementOfType(XmlNodeType.EndElement);
		}

		public XmlNodeType GetNodeType()
		{
			if (this.reader == null)
			{
				return XmlNodeType.None;
			}
			return this.reader.NodeType;
		}

		public bool IsEmptyElement()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.IsEmptyElement;
		}

		public bool HasAttributes()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.HasAttributes;
		}

		public bool MoveToFirstAttribute()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.MoveToFirstAttribute();
		}

		public bool MoveToNextAttribute()
		{
			Guard.ThrowExceptionIfNull<XmlReader>(this.reader, "currentPartReader");
			return this.reader.MoveToNextAttribute();
		}

		public bool IsElementOfType(XmlNodeType type)
		{
			return this.reader != null && this.reader.NodeType == type;
		}

		public bool Read()
		{
			return this.reader != null && this.reader.Read();
		}

		public void SkipElement()
		{
			if (this.reader == null)
			{
				return;
			}
			this.reader.Skip();
		}

		public bool GetElementName(out string elementName)
		{
			elementName = string.Empty;
			if (this.reader == null)
			{
				return false;
			}
			if (this.IsElementOfType(XmlNodeType.Element))
			{
				elementName = this.reader.LocalName;
				return true;
			}
			return false;
		}

		public bool GetElementValue(out string value)
		{
			if (this.reader == null)
			{
				value = string.Empty;
				return false;
			}
			value = this.reader.Value;
			return true;
		}

		public bool GetAttributeNameAndValue(out string attributeName, out string value)
		{
			attributeName = string.Empty;
			value = string.Empty;
			if (this.reader == null)
			{
				return false;
			}
			if (this.IsElementOfType(XmlNodeType.Attribute))
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
