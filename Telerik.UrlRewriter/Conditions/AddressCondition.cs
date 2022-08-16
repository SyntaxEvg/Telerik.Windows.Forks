using System;
using System.Net;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Conditions
{
	public sealed class AddressCondition : IRewriteCondition
	{
		public AddressCondition(string pattern)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			this._range = IPRange.Parse(pattern);
		}

		public bool IsMatch(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			string text = context.Properties["REMOTE_ADDR"];
			return text != null && this._range.InRange(IPAddress.Parse(text));
		}

		IPRange _range;
	}
}
