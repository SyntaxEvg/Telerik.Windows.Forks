using System;

namespace CsQuery.Implementation
{
	class CSSStyleRule : CSSRule, ICSSStyleRule, ICSSRule
	{
		public CSSStyleRule(ICSSStyleSheet parentStyleSheet, ICSSRule parentRule)
			: base(parentStyleSheet, parentRule)
		{
			base.ParentStyleSheet = parentStyleSheet;
			base.ParentRule = parentRule;
		}

		public string SelectorText { get; set; }

		public ICSSStyleDeclaration Style { get; set; }

		public override string CssText
		{
			get
			{
				return this.SelectorText + " " + this.Style.ToString();
			}
			set
			{
				int num = value.IndexOf("{");
				if (num > 0)
				{
					throw new NotImplementedException();
				}
			}
		}
	}
}
