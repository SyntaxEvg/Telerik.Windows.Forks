using System;
using System.Net;

namespace HttpWebAdapters
{
	class WebResponseStub : WebResponse, IHttpWebResponse, IDisposable
	{
		public string GetResponseHeader(string headerName)
		{
			throw new NotImplementedException();
		}

		public CookieCollection Cookies
		{
			get
			{
				return this.cookies;
			}
			set
			{
				this.cookies = value;
			}
		}

		public string ContentEncoding
		{
			get
			{
				return this.contentEncoding;
			}
			set
			{
				this.contentEncoding = value;
			}
		}

		public string CharacterSet
		{
			get
			{
				return this.characterSet;
			}
			set
			{
				this.characterSet = value;
			}
		}

		public string Server
		{
			get
			{
				return this.server;
			}
			set
			{
				this.server = value;
			}
		}

		public DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			set
			{
				this.lastModified = value;
			}
		}

		public HttpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.statusCode = value;
			}
		}

		public string StatusDescription
		{
			get
			{
				return this.statusDescription;
			}
			set
			{
				this.statusDescription = value;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				return this.protocolVersion;
			}
			set
			{
				this.protocolVersion = value;
			}
		}

		public string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				this.method = value;
			}
		}

		CookieCollection cookies;

		string contentEncoding;

		string characterSet;

		string server;

		DateTime lastModified;

		HttpStatusCode statusCode;

		string statusDescription;

		Version protocolVersion;

		string method;
	}
}
