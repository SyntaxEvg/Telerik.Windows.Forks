using System;

namespace CsQuery
{
	interface ICSSRule
	{
		CSSRuleType Type { get; }

		string CssText { get; set; }

		ICSSStyleSheet ParentStyleSheet { get; }

		ICSSRule ParentRule { get; }
	}
}
