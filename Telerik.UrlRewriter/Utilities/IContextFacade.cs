using System;
using System.Collections.Specialized;
using System.Web;

namespace Telerik.UrlRewriter.Utilities
{
	public interface IContextFacade
	{
		string GetApplicationPath();

		string GetRawUrl();

		Uri GetRequestUrl();

		MapPath MapPath { get; }

		void SetStatusCode(int code);

		void RewritePath(string url);

		void SetRedirectLocation(string url);

		void AppendHeader(string name, string value);

		void AppendCookie(HttpCookie cookie);

		void HandleError(IRewriteErrorHandler handler);

		void SetItem(object item, object value);

		object GetItem(object item);

		string GetHttpMethod();

		NameValueCollection GetServerVariables();

		NameValueCollection GetHeaders();

		HttpCookieCollection GetCookies();
	}
}
