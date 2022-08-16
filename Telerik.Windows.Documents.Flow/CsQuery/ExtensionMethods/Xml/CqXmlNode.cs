using System;
using System.Collections.Generic;
using System.Xml;

namespace CsQuery.ExtensionMethods.Xml
{
	class CqXmlNode : XmlElement
	{
		public CqXmlNode(XmlDocument xmlDocument, IDomObject element)
			: base("", CqXmlNode.GetNodeName(element), "", xmlDocument)
		{
			this.Element = element;
			this.XmlDocument = xmlDocument;
		}

		public override XmlNode CloneNode(bool deep)
		{
			throw new NotImplementedException();
		}

		public override string LocalName
		{
			get
			{
				return base.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				return base.Name;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.NodeTypeMap(this.Element.NodeType);
			}
		}

		public override XmlAttributeCollection Attributes
		{
			get
			{
				if (!this.IsAttributesCreated)
				{
					if (this.Element.HasAttributes)
					{
						foreach (KeyValuePair<string, string> keyValuePair in this.Element.Attributes)
						{
							XmlAttribute xmlAttribute = this.XmlDocument.CreateAttribute(keyValuePair.Key);
							xmlAttribute.Value = keyValuePair.Value;
							base.Attributes.Append(xmlAttribute);
						}
					}
					this.IsAttributesCreated = true;
				}
				return base.Attributes;
			}
		}

		public override XmlNodeList ChildNodes
		{
			get
			{
				if (!this.IsChildListCreated)
				{
					this.InnerChildNodes = new CqXmlNodeList(this.XmlDocument, this.Element.ChildNodes);
					this.IsChildListCreated = true;
				}
				return this.InnerChildNodes;
			}
		}

		protected XmlNodeType NodeTypeMap(NodeType type)
		{
			switch (type)
			{
			case CsQuery.NodeType.ELEMENT_NODE:
				return XmlNodeType.Element;
			case CsQuery.NodeType.TEXT_NODE:
				return XmlNodeType.Text;
			case CsQuery.NodeType.CDATA_SECTION_NODE:
				return XmlNodeType.CDATA;
			case CsQuery.NodeType.COMMENT_NODE:
				return XmlNodeType.Comment;
			case CsQuery.NodeType.DOCUMENT_NODE:
				return XmlNodeType.Element;
			case CsQuery.NodeType.DOCUMENT_TYPE_NODE:
				return XmlNodeType.DocumentType;
			case CsQuery.NodeType.DOCUMENT_FRAGMENT_NODE:
				return XmlNodeType.DocumentFragment;
			}
			throw new NotImplementedException("Unknown node type");
		}

		static string GetNodeName(IDomObject element)
		{
			if (!string.IsNullOrEmpty(element.NodeName))
			{
				return CqXmlNode.CleanXmlNodeName(element.NodeName);
			}
			if (element is IDomFragment)
			{
				return "ROOT";
			}
			if (element is IDomDocument)
			{
				return "ROOT";
			}
			return "UNKNOWN";
		}

		static string CleanXmlNodeName(string name)
		{
			if (name[0] == '#')
			{
				return name.Substring(1);
			}
			return name;
		}

		IDomObject Element;

		XmlDocument XmlDocument;

		XmlNodeList InnerChildNodes;

		bool IsAttributesCreated;

		bool IsChildListCreated;
	}
}
