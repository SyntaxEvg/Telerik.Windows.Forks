using System;
using System.Text.RegularExpressions;

namespace Telerik.UrlRewriter.Conditions
{
	public sealed class MethodCondition : MatchCondition
	{
		public MethodCondition(string pattern)
			: base(MethodCondition.GetMethodPattern(pattern))
		{
		}

		public override bool IsMatch(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return base.Pattern.IsMatch(context.Method);
		}

		static string GetMethodPattern(string method)
		{
			return string.Format("^{0}$", Regex.Replace(method, "[^a-zA-Z,\\*]+", "").Replace(",", "|").Replace("*", ".+"));
		}
	}
}
