using System;
using System.Text.RegularExpressions;

namespace PreMailer.Net
{
	class CssSelector
	{
		public string Selector { get; protected set; }

		public bool HasNotPseudoClass
		{
			get
			{
				return CssSelector.NotMatcher.IsMatch(this.Selector);
			}
		}

		public string NotPseudoClassContent
		{
			get
			{
				Match match = CssSelector.NotMatcher.Match(this.Selector);
				if (!match.Success)
				{
					return null;
				}
				return match.Groups[1].Value;
			}
		}

		public CssSelector(string selector)
		{
			this.Selector = selector;
		}

		public CssSelector StripNotPseudoClassContent()
		{
			string selector = CssSelector.NotMatcher.Replace(this.Selector, string.Empty);
			return new CssSelector(selector);
		}

		public override string ToString()
		{
			return this.Selector;
		}

		protected static Regex NotMatcher = new Regex(":not\\((.+)\\)", RegexOptions.None);
	}
}
