using System;
using System.Xml;

namespace Telerik.UrlRewriter
{
	public interface IRewriteActionParser
	{
		IRewriteAction Parse(XmlNode node, object config);

		string Name { get; }

		bool AllowsNestedActions { get; }

		bool AllowsAttributes { get; }
	}
}
