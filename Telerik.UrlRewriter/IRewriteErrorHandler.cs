using System;
using System.Web;

namespace Telerik.UrlRewriter
{
	public interface IRewriteErrorHandler
	{
		void HandleError(HttpContext context);
	}
}
