using System;
using System.Collections;
using System.Configuration;
using System.Xml;
using Telerik.UrlRewriter.Actions;
using Telerik.UrlRewriter.Configuration;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Parsers
{
	public class IfConditionActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "if";
			}
		}

		public override bool AllowsNestedActions
		{
			get
			{
				return true;
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
			RewriterConfiguration config2 = config as RewriterConfiguration;
			ConditionalAction conditionalAction = new ConditionalAction();
			bool negative = node.LocalName == "unless";
			base.ParseConditions(node, conditionalAction.Conditions, negative, config);
			IfConditionActionParser.ReadAttributeActions(node, false, conditionalAction.Actions, config2);
			IfConditionActionParser.ReadActions(node, false, conditionalAction.Actions, config2);
			IfConditionActionParser.ReadAttributeActions(node, true, conditionalAction.Actions, config2);
			IfConditionActionParser.ReadActions(node, true, conditionalAction.Actions, config2);
			return conditionalAction;
		}

		static void ReadAttributeActions(XmlNode node, bool allowFinal, IList actions, RewriterConfiguration config)
		{
			int i = 0;
			while (i < node.Attributes.Count)
			{
				XmlNode xmlNode = node.Attributes[i++];
				IList parsers = config.ActionParserFactory.GetParsers(string.Format("{0}-{1}", node.LocalName, xmlNode.LocalName));
				if (parsers != null)
				{
					foreach (object obj in parsers)
					{
						IRewriteActionParser rewriteActionParser = (IRewriteActionParser)obj;
						IRewriteAction rewriteAction = rewriteActionParser.Parse(node, config);
						if (rewriteAction != null && (rewriteAction.Processing == RewriteProcessing.ContinueProcessing || allowFinal))
						{
							actions.Add(rewriteAction);
							break;
						}
					}
				}
			}
		}

		static void ReadActions(XmlNode node, bool allowFinal, IList actions, RewriterConfiguration config)
		{
			for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					IList parsers = config.ActionParserFactory.GetParsers(xmlNode.LocalName);
					if (parsers != null)
					{
						bool flag = false;
						foreach (object obj in parsers)
						{
							IRewriteActionParser rewriteActionParser = (IRewriteActionParser)obj;
							IRewriteAction rewriteAction = rewriteActionParser.Parse(xmlNode, config);
							if (rewriteAction != null)
							{
								flag = true;
								if ((rewriteAction.Processing == RewriteProcessing.ContinueProcessing) ^ allowFinal)
								{
									actions.Add(rewriteAction);
									break;
								}
							}
						}
						if (!flag)
						{
							throw new ConfigurationErrorsException(MessageProvider.FormatString(Message.ElementNotAllowed, new object[] { node.FirstChild.Name }), node);
						}
					}
				}
			}
		}
	}
}
