using System;
using System.Web;

namespace Telerik.UrlRewriter.Errors
{
	public class DefaultErrorHandler : IRewriteErrorHandler
	{
		public DefaultErrorHandler(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this._url = url;
		}

		public void HandleError(HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Server.Execute(this._url);
		}

		string _url;
	}
}
