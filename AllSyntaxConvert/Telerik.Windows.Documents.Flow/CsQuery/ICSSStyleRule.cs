using System;

namespace CsQuery
{
	interface ICSSStyleRule : ICSSRule
	{
		string SelectorText { get; set; }

		ICSSStyleDeclaration Style { get; }
	}
}
