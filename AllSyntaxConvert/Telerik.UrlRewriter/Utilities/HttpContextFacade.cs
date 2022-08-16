using System;
using System.Collections.Specialized;
using System.Web;

namespace Telerik.UrlRewriter.Utilities
{
	class HttpContextFacade : IContextFacade
	{
		public HttpContextFacade()
		{
			this._mapPath = new MapPath(this.InternalMapPath);
		}

		public MapPath MapPath
		{
			get
			{
				return this._mapPath;
			}
		}

		public string GetApplicationPath()
		{
			return HttpContext.Current.Request.ApplicationPath;
		}

		public string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public Uri GetRequestUrl()
		{
			return HttpContext.Current.Request.Url;
		}

		string InternalMapPath(string url)
		{
			return HttpContext.Current.Server.MapPath(url);
		}

		public void SetStatusCode(int code)
		{
			HttpContext.Current.Response.StatusCode = code;
		}

		public void RewritePath(string url)
		{
			HttpContext.Current.RewritePath(url, true);
		}

		public void SetRedirectLocation(string url)
		{
			HttpContext.Current.Response.RedirectLocation = url;
		}

		public void AppendHeader(string name, string value)
		{
			HttpContext.Current.Response.AppendHeader(name, value);
		}

		public void AppendCookie(HttpCookie cookie)
		{
			HttpContext.Current.Response.AppendCookie(cookie);
		}

		public void HandleError(IRewriteErrorHandler handler)
		{
			handler.HandleError(HttpContext.Current);
		}

		public void SetItem(object item, object value)
		{
			HttpContext.Current.Items[item] = value;
		}

		public object GetItem(object item)
		{
			return HttpContext.Current.Items[item];
		}

		public string GetHttpMethod()
		{
			return HttpContext.Current.Request.HttpMethod;
		}

		public NameValueCollection GetServerVariables()
		{
			return HttpContext.Current.Request.ServerVariables;
		}

		public NameValueCollection GetHeaders()
		{
			return HttpContext.Current.Request.Headers;
		}

		public HttpCookieCollection GetCookies()
		{
			return HttpContext.Current.Request.Cookies;
		}

		MapPath _mapPath;
	}
}
