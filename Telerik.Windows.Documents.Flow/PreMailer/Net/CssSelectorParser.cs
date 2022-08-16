using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PreMailer.Net
{
	class CssSelectorParser : ICssSelectorParser
	{
		public static int SelectorSpecificity(string selector)
		{
			CssSelectorParser cssSelectorParser = new CssSelectorParser();
			return cssSelectorParser.GetSelectorSpecificity(selector);
		}

		public int GetSelectorSpecificity(string selector)
		{
			return this.CalculateSpecificity(selector).ToInt();
		}

		public CssSpecificity CalculateSpecificity(string selector)
		{
			if (string.IsNullOrWhiteSpace(selector) || selector == "*")
			{
				return CssSpecificity.None;
			}
			CssSelector cssSelector = new CssSelector(selector);
			CssSpecificity first = CssSpecificity.None;
			if (cssSelector.HasNotPseudoClass)
			{
				first += this.CalculateSpecificity(cssSelector.NotPseudoClassContent);
			}
			string selector2 = cssSelector.StripNotPseudoClassContent().ToString();
			int ids = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.IdMatcher, selector2, out selector2);
			int num = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.AttribMatcher, selector2, out selector2);
			int num2 = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.ClassMatcher, selector2, out selector2);
			int num3 = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.PseudoClassMatcher, selector2, out selector2);
			int num4 = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.ElemMatcher, selector2, out selector2);
			int num5 = CssSelectorParser.MatchCountAndStrip(CssSelectorParser.PseudoElemMatcher, selector2, out selector2);
			return first + new CssSpecificity(ids, num2 + num + num3, num4 + num5)
			{
				Classes = num2
			};
		}

		public bool IsPseudoClass(string selector)
		{
			return CssSelectorParser.PseudoClassMatcher.IsMatch(selector);
		}

		public bool IsPseudoElement(string selector)
		{
			return CssSelectorParser.PseudoElemMatcher.IsMatch(selector);
		}

		public bool IsSupportedSelector(string key)
		{
			Regex regex = CssSelectorParser.BuildOrRegex(CssSelectorParser.UnimplementedPseudoSelectors, "::?", null);
			return !regex.IsMatch(key);
		}

		static int MatchCountAndStrip(Regex regex, string selector, out string result)
		{
			MatchCollection matchCollection = regex.Matches(selector);
			result = regex.Replace(selector, string.Empty);
			return matchCollection.Count;
		}

		static string[] PseudoClasses
		{
			get
			{
				return new string[]
				{
					"active", "checked", "default", "dir()", "disabled", "empty", "enabled", "first", "first-child", "first-of-type",
					"fullscreen", "focus", "hover", "indeterminate", "in-range", "invalid", "lang()", "last-child", "last-of-type", "left",
					"link", "not()", "nth-child()", "nth-last-child()", "nth-last-of-type()", "nth-of-type()", "only-child", "only-of-type", "optional", "out-of-range",
					"read-only", "read-write", "required", "right", "root", "scope", "target", "valid", "visited"
				}.Reverse<string>().ToArray<string>();
			}
		}

		static string[] PseudoElements
		{
			get
			{
				return new string[] { "after", "before", "first-letter", "first-line", "selection" };
			}
		}

		static string[] UnimplementedPseudoSelectors
		{
			get
			{
				return new string[] { "link", "hover", "active", "focus", "visited", "target", "first-letter", "first-line", "before", "after" };
			}
		}

		static Regex BuildRegex(string pattern)
		{
			return new Regex(pattern, RegexOptions.None);
		}

		static Regex BuildOrRegex(string[] items, string prefix, Func<string, string> mutator = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(prefix);
			stringBuilder.Append("(");
			for (int i = 0; i < items.Length; i++)
			{
				string text = items[i];
				if (mutator != null)
				{
					text = mutator(text);
				}
				stringBuilder.Append(text);
				if (i < items.Length - 1)
				{
					stringBuilder.Append("|");
				}
			}
			stringBuilder.Append(")");
			return new Regex(stringBuilder.ToString(), RegexOptions.None);
		}

		static readonly Regex IdMatcher = CssSelectorParser.BuildRegex("#([\\w]+)");

		static readonly Regex AttribMatcher = CssSelectorParser.BuildRegex("\\[[\\w=]+\\]");

		static readonly Regex ClassMatcher = CssSelectorParser.BuildRegex("\\.([\\w-]+)");

		static readonly Regex ElemMatcher = CssSelectorParser.BuildRegex("[a-zA-Z]+");

		static readonly Regex PseudoClassMatcher = CssSelectorParser.BuildOrRegex(CssSelectorParser.PseudoClasses, ":", (string x) => x.Replace("()", "\\(\\w+\\)"));

		static readonly Regex PseudoElemMatcher = CssSelectorParser.BuildOrRegex(CssSelectorParser.PseudoElements, "::?", null);
	}
}
