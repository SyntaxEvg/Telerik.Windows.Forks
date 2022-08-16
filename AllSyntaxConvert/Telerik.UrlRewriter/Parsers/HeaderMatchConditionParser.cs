using System;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Conditions;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class HeaderMatchConditionParser : IRewriteConditionParser
	{
		public IRewriteCondition Parse(XmlNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			XmlNode namedItem = node.Attributes.GetNamedItem("header");
			if (namedItem == null)
			{
				return null;
			}
			string value = namedItem.Value;
			XmlNode namedItem2 = node.Attributes.GetNamedItem("match");
			if (namedItem2 != null)
			{
				return new PropertyMatchCondition(value, namedItem2.Value);
			}
			throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "match" }), node);
		}
	}
}
