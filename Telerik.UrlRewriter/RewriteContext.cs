using System;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter
{
	public sealed class RewriteContext
	{
		internal RewriteContext(RewriterEngine engine, string rawUrl, string httpMethod, MapPath mapPath, NameValueCollection serverVariables, NameValueCollection headers, HttpCookieCollection cookies)
		{
			this._engine = engine;
			this._location = rawUrl;
			this._method = httpMethod;
			this._mapPath = mapPath;
			foreach (object obj in serverVariables)
			{
				string name = (string)obj;
				this._properties.Add(name, serverVariables[name]);
			}
			foreach (object obj2 in headers)
			{
				string name2 = (string)obj2;
				this._properties.Add(name2, headers[name2]);
			}
			foreach (object obj3 in cookies)
			{
				string name3 = (string)obj3;
				this._properties.Add(name3, cookies[name3].Value);
			}
		}

		public string MapPath(string url)
		{
			return this._mapPath(url);
		}

		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				this._location = value;
			}
		}

		public string Method
		{
			get
			{
				return this._method;
			}
		}

		public NameValueCollection Properties
		{
			get
			{
				return this._properties;
			}
		}

		public NameValueCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		public HttpStatusCode StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				this._statusCode = value;
			}
		}

		public HttpCookieCollection Cookies
		{
			get
			{
				return this._cookies;
			}
		}

		public Match LastMatch
		{
			get
			{
				return this._lastMatch;
			}
			set
			{
				this._lastMatch = value;
			}
		}

		public string Expand(string input)
		{
			return this._engine.Expand(this, input);
		}

		public string ResolveLocation(string location)
		{
			return this._engine.ResolveLocation(location);
		}

		RewriterEngine _engine;

		string _method = string.Empty;

		HttpStatusCode _statusCode = HttpStatusCode.OK;

		string _location;

		NameValueCollection _properties = new NameValueCollection();

		NameValueCollection _headers = new NameValueCollection();

		HttpCookieCollection _cookies = new HttpCookieCollection();

		Match _lastMatch;

		MapPath _mapPath;
	}
}
