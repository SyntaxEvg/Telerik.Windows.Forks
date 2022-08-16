using System;
using System.Text.RegularExpressions;

namespace Telerik.UrlRewriter.Conditions
{
	public abstract class MatchCondition : IRewriteCondition
	{
		protected MatchCondition(string pattern)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			this._pattern = new Regex(pattern, RegexOptions.IgnoreCase);
		}

		public Regex Pattern
		{
			get
			{
				return this._pattern;
			}
			set
			{
				this._pattern = value;
			}
		}

		public abstract bool IsMatch(RewriteContext context);

		Regex _pattern;
	}
}
