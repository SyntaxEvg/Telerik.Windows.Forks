using System;
using System.IO;
using System.Net;

namespace HttpWebAdapters.Adapters
{
	class HttpWebResponseAdapter : IHttpWebResponse, IDisposable
	{
		public HttpWebResponseAdapter(WebResponse response)
		{
			this.response = response;
		}

		public string GetResponseHeader(string headerName)
		{
			string responseHeader;
			try
			{
				responseHeader = ((HttpWebResponse)this.response).GetResponseHeader(headerName);
			}
			catch (InvalidCastException)
			{
				responseHeader = ((WebResponseStub)this.response).GetResponseHeader(headerName);
			}
			return responseHeader;
		}

		public CookieCollection Cookies
		{
			get
			{
				CookieCollection cookies;
				try
				{
					cookies = ((HttpWebResponse)this.response).Cookies;
				}
				catch (InvalidCastException)
				{
					cookies = ((WebResponseStub)this.response).Cookies;
				}
				return cookies;
			}
			set
			{
				try
				{
					((HttpWebResponse)this.response).Cookies = value;
				}
				catch (InvalidCastException)
				{
					((WebResponseStub)this.response).Cookies = value;
				}
			}
		}

		public string ContentEncoding
		{
			get
			{
				string contentEncoding;
				try
				{
					contentEncoding = ((HttpWebResponse)this.response).ContentEncoding;
				}
				catch (InvalidCastException)
				{
					contentEncoding = ((WebResponseStub)this.response).ContentEncoding;
				}
				return contentEncoding;
			}
		}

		public string CharacterSet
		{
			get
			{
				string characterSet;
				try
				{
					characterSet = ((HttpWebResponse)this.response).CharacterSet;
				}
				catch (InvalidCastException)
				{
					characterSet = ((WebResponseStub)this.response).CharacterSet;
				}
				return characterSet;
			}
		}

		public string Server
		{
			get
			{
				string server;
				try
				{
					server = ((HttpWebResponse)this.response).Server;
				}
				catch (InvalidCastException)
				{
					server = ((WebResponseStub)this.response).Server;
				}
				return server;
			}
		}

		public DateTime LastModified
		{
			get
			{
				DateTime lastModified;
				try
				{
					lastModified = ((HttpWebResponse)this.response).LastModified;
				}
				catch (InvalidCastException)
				{
					lastModified = ((WebResponseStub)this.response).LastModified;
				}
				return lastModified;
			}
		}

		public HttpStatusCode StatusCode
		{
			get
			{
				HttpStatusCode statusCode;
				try
				{
					statusCode = ((HttpWebResponse)this.response).StatusCode;
				}
				catch (InvalidCastException)
				{
					statusCode = ((WebResponseStub)this.response).StatusCode;
				}
				return statusCode;
			}
		}

		public string StatusDescription
		{
			get
			{
				string statusDescription;
				try
				{
					statusDescription = ((HttpWebResponse)this.response).StatusDescription;
				}
				catch (InvalidCastException)
				{
					statusDescription = ((WebResponseStub)this.response).StatusDescription;
				}
				return statusDescription;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				Version protocolVersion;
				try
				{
					protocolVersion = ((HttpWebResponse)this.response).ProtocolVersion;
				}
				catch (InvalidCastException)
				{
					protocolVersion = ((WebResponseStub)this.response).ProtocolVersion;
				}
				return protocolVersion;
			}
		}

		public string Method
		{
			get
			{
				string method;
				try
				{
					method = ((HttpWebResponse)this.response).Method;
				}
				catch (InvalidCastException)
				{
					method = ((WebResponseStub)this.response).Method;
				}
				return method;
			}
		}

		public void Close()
		{
			this.response.Close();
		}

		public Stream GetResponseStream()
		{
			return this.response.GetResponseStream();
		}

		public bool IsFromCache
		{
			get
			{
				return this.response.IsFromCache;
			}
		}

		public bool IsMutuallyAuthenticated
		{
			get
			{
				return this.response.IsMutuallyAuthenticated;
			}
		}

		public long ContentLength
		{
			get
			{
				return this.response.ContentLength;
			}
			set
			{
				this.response.ContentLength = value;
			}
		}

		public string ContentType
		{
			get
			{
				return this.response.ContentType;
			}
			set
			{
				this.response.ContentType = value;
			}
		}

		public Uri ResponseUri
		{
			get
			{
				return this.response.ResponseUri;
			}
		}

		public WebHeaderCollection Headers
		{
			get
			{
				return this.response.Headers;
			}
		}

		public void Dispose()
		{
			this.response.Close();
		}

		WebResponse response;
	}
}
