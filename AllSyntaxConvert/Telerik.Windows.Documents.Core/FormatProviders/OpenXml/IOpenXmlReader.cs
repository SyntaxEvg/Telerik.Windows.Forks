using System;
using System.Xml;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml
{
	interface IOpenXmlReader
	{
		bool IsEmptyElement();

		bool HasAttributes();

		bool MoveToFirstAttribute();

		bool MoveToNextAttribute();

		XmlNodeType GetNodeType();

		bool IsElementOfType(XmlNodeType type);

		bool Read();

		void SkipElement();

		bool GetElementValue(out string value);

		bool GetElementName(out string elementName);

		bool GetAttributeNameAndValue(out string attributeName, out string value);
	}
}
