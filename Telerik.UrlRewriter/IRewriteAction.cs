using System;

namespace Telerik.UrlRewriter
{
	public interface IRewriteAction
	{
		void Execute(RewriteContext context);

		RewriteProcessing Processing { get; }
	}
}
