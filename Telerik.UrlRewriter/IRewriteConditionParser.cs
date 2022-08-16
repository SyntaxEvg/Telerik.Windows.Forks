using System;
using System.Xml;

namespace Telerik.UrlRewriter
{
	public interface IRewriteConditionParser
	{
		IRewriteCondition Parse(XmlNode node);
	}
}
