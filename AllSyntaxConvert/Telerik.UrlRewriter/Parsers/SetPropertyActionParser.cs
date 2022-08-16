using System;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Actions;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class SetPropertyActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "set";
			}
		}

		public override bool AllowsNestedActions
		{
			get
			{
				return false;
			}
		}

		public override bool AllowsAttributes
		{
			get
			{
				return true;
			}
		}

		public override IRewriteAction Parse(XmlNode node, object config)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			XmlNode namedItem = node.Attributes.GetNamedItem("property");
			if (namedItem == null)
			{
				return null;
			}
			XmlNode namedItem2 = node.Attributes.GetNamedItem("value");
			if (namedItem2 == null)
			{
				throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "value" }), node);
			}
			return new SetPropertyAction(namedItem.Value, namedItem2.Value);
		}
	}
}
