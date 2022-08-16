using System;

namespace CsQuery.Implementation
{
	abstract class CSSRule : ICSSRule
	{
		public CSSRule(ICSSStyleSheet parentStyleSheet, ICSSRule parentRule)
		{
			this.ParentStyleSheet = parentStyleSheet;
			this.ParentRule = parentRule;
		}

		public CSSRuleType Type { get; set; }

		public abstract string CssText { get; set; }

		public ICSSStyleSheet ParentStyleSheet { get; protected set; }

		public ICSSRule ParentRule { get; protected set; }
	}
}
