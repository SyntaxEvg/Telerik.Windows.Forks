using System;
using System.Text.RegularExpressions;

namespace Telerik.UrlRewriter.Conditions
{
	public sealed class UrlMatchCondition : IRewriteCondition
	{
		public UrlMatchCondition(string pattern)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			this._pattern = pattern;
		}

		public bool IsMatch(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (this._regex == null)
			{
				lock (this)
				{
					if (this._regex == null)
					{
						this._regex = new Regex(context.ResolveLocation(this.Pattern), RegexOptions.IgnoreCase);
					}
				}
			}
			Match match = this._regex.Match(context.Location);
			if (match.Success)
			{
				context.LastMatch = match;
				return true;
			}
			return false;
		}

		public string Pattern
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

		Regex _regex;

		string _pattern;
	}
}
