using System;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Actions;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class RewriteActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "rewrite";
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
			XmlNode xmlNode = node.Attributes["to"];
			if (xmlNode.Value == null)
			{
				throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.AttributeRequired, new object[] { "to" }), node);
			}
			XmlNode xmlNode2 = node.Attributes["processing"];
			RewriteProcessing processing = RewriteProcessing.ContinueProcessing;
			if (xmlNode2 != null)
			{
				if (xmlNode2.Value == "restart")
				{
					processing = RewriteProcessing.RestartProcessing;
				}
				else if (xmlNode2.Value == "stop")
				{
					processing = RewriteProcessing.StopProcessing;
				}
				else if (xmlNode2.Value != "continue")
				{
					throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.ValueOfProcessingAttribute, new object[] { xmlNode2.Value, "continue", "restart", "stop" }), node);
				}
			}
			RewriteAction rewriteAction = new RewriteAction(xmlNode.Value, processing);
			base.ParseConditions(node, rewriteAction.Conditions, false, config);
			return rewriteAction;
		}
	}
}
