using System;

namespace PreMailer.Net
{
	interface ICssSelectorParser
	{
		int GetSelectorSpecificity(string selector);

		bool IsPseudoElement(string selector);

		bool IsPseudoClass(string selector);
	}
}
