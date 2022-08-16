using System;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Actions;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class RedirectActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "redirect";
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
			XmlNode namedItem = node.Attributes.GetNamedItem("to");
			if (namedItem == null)
			{
				throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "to" }), node);
			}
			bool permanent = true;
			XmlNode namedItem2 = node.Attributes.GetNamedItem("permanent");
			if (namedItem2 != null)
			{
				permanent = Convert.ToBoolean(namedItem2.Value);
			}
			RedirectAction redirectAction = new RedirectAction(namedItem.Value, permanent);
			base.ParseConditions(node, redirectAction.Conditions, false, config);
			return redirectAction;
		}
	}
}
