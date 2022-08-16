using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace CsQuery.ExtensionMethods.Xml
{
	class CqXmlNodeList : XmlNodeList
	{
		public CqXmlNodeList(XmlDocument xmlDocument, INodeList nodeList)
		{
			this.NodeList = nodeList;
			this.XmlDocument = xmlDocument;
		}

		public override int Count
		{
			get
			{
				return this.NodeList.Count;
			}
		}

		public override IEnumerator GetEnumerator()
		{
			return this.Nodes().GetEnumerator();
		}

		IEnumerable<XmlNode> Nodes()
		{
			foreach (IDomObject node in this.NodeList)
			{
				yield return node.ToXml(this.XmlDocument);
			}
			yield break;
		}

		public override XmlNode Item(int index)
		{
			return this.NodeList[index].ToXml();
		}

		INodeList NodeList;

		XmlDocument XmlDocument;
	}
}
