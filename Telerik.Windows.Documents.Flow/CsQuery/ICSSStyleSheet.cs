using System;
using System.Collections.Generic;

namespace CsQuery
{
	interface ICSSStyleSheet
	{
		bool Disabled { get; set; }

		string Href { get; set; }

		IDomElement OwnerNode { get; }

		string Type { get; }

		IList<ICSSRule> CssRules { get; }
	}
}
