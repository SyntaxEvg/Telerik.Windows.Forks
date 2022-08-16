using System;
using System.Xml;

namespace CsQuery.ExtensionMethods.Xml
{
	class CqXmlDocument : XmlDocument
	{
		public CqXmlDocument(IDomDocument document)
		{
			CqXmlNode cqXmlNode = null;
			foreach (IDomObject element in document.ChildNodes)
			{
				CqXmlNode cqXmlNode2 = new CqXmlNode(this, element);
				if (cqXmlNode2.NodeType == XmlNodeType.DocumentType)
				{
					this.AppendChild(cqXmlNode2);
				}
				else
				{
					if (cqXmlNode == null)
					{
						cqXmlNode = new CqXmlNode(this, document);
						this.AppendChild(cqXmlNode);
					}
					cqXmlNode.AppendChild(cqXmlNode2);
				}
			}
		}
	}
}
