﻿using System;
using System.Xml;
using Telerik.UrlRewriter.Actions;

namespace Telerik.UrlRewriter.Parsers
{
	public sealed class ForbiddenActionParser : RewriteActionParserBase
	{
		public override string Name
		{
			get
			{
				return "forbidden";
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
				return false;
			}
		}

		public override IRewriteAction Parse(XmlNode node, object config)
		{
			return new ForbiddenAction();
		}
	}
}
