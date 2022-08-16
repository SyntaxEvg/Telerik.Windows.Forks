using System;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Actions;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class AddHeaderActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "add";
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
			XmlNode namedItem = node.Attributes.GetNamedItem("header");
			if (namedItem == null)
			{
				throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "header" }), node);
			}
			XmlNode namedItem2 = node.Attributes.GetNamedItem("value");
			if (namedItem2 == null)
			{
				throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "value" }), node);
			}
			return new AddHeaderAction(namedItem.Value, namedItem2.Value);
		}
	}
}
