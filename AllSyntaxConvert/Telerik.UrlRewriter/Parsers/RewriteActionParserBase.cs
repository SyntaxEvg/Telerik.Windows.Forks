using System;
using System.Collections;
using System.Xml;
using Telerik.UrlRewriter.Conditions;
using Telerik.UrlRewriter.Configuration;

namespace Telerik.UrlRewriter.Parsers
{
	public abstract class RewriteActionParserBase : IRewriteActionParser
	{
		public abstract IRewriteAction Parse(XmlNode node, object config);

		public abstract string Name { get; }

		public abstract bool AllowsNestedActions { get; }

		public abstract bool AllowsAttributes { get; }

		protected void ParseConditions(XmlNode node, IList conditions, bool negative, object config)
		{
			if (config == null)
			{
				return;
			}
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (conditions == null)
			{
				throw new ArgumentNullException("conditions");
			}
			RewriterConfiguration rewriterConfiguration = config as RewriterConfiguration;
			foreach (object obj in rewriterConfiguration.ConditionParserPipeline)
			{
				IRewriteConditionParser rewriteConditionParser = (IRewriteConditionParser)obj;
				IRewriteCondition rewriteCondition = rewriteConditionParser.Parse(node);
				if (rewriteCondition != null)
				{
					if (negative)
					{
						rewriteCondition = new NegativeCondition(rewriteCondition);
					}
					conditions.Add(rewriteCondition);
				}
			}
			XmlNode xmlNode = node.FirstChild;
			while (xmlNode != null)
			{
				if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.LocalName == "and")
				{
					this.ParseConditions(xmlNode, conditions, negative, config);
					XmlNode nextSibling = xmlNode.NextSibling;
					node.RemoveChild(xmlNode);
					xmlNode = nextSibling;
				}
				else
				{
					xmlNode = xmlNode.NextSibling;
				}
			}
		}
	}
}
