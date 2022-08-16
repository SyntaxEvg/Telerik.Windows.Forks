using System;

namespace Telerik.UrlRewriter
{
	public interface IRewriteCondition
	{
		bool IsMatch(RewriteContext context);
	}
}
