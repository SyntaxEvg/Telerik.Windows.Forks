using System;
using System.Xml;
using Telerik.UrlRewriter.Conditions;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class MethodConditionParser : IRewriteConditionParser
	{
		public IRewriteCondition Parse(XmlNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			XmlNode namedItem = node.Attributes.GetNamedItem("method");
			if (namedItem != null)
			{
				return new MethodCondition(namedItem.Value);
			}
			return null;
		}
	}
}
