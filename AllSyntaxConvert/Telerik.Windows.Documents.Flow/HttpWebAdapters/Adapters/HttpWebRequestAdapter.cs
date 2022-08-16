using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace HttpWebAdapters.Adapters
{
	class HttpWebRequestAdapter : IHttpWebRequest
	{
		public HttpWebRequestAdapter(HttpWebRequest request)
		{
			this.request = request;
		}

		public HttpWebRequestMethod Method
		{
			get
			{
				return HttpWebRequestMethod.Parse(this.request.Method);
			}
			set
			{
				this.request.Method = value.ToString();
			}
		}

		public IHttpWebResponse GetResponse()
		{
			return new HttpWebResponseAdapter(this.request.GetResponse());
		}

		public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return this.request.BeginGetResponse(callback, state);
		}

		public IHttpWebResponse EndGetResponse(IAsyncResult result)
		{
			return new HttpWebResponseAdapter(this.request.EndGetResponse(result));
		}

		public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return this.request.BeginGetRequestStream(callback, state);
		}

		public Stream EndGetRequestStream(IAsyncResult result)
		{
			return this.request.EndGetRequestStream(result);
		}

		public Stream GetRequestStream()
		{
			return this.request.GetRequestStream();
		}

		public void Abort()
		{
			this.request.Abort();
		}

		public void AddRange(int from, int to)
		{
			this.request.AddRange(from, to);
		}

		public void AddRange(int range)
		{
			this.request.AddRange(range);
		}

		public void AddRange(string rangeSpecifier, int from, int to)
		{
			this.request.AddRange(rangeSpecifier, from, to);
		}

		public void AddRange(string rangeSpecifier, int range)
		{
			this.request.AddRange(rangeSpecifier, range);
		}

		public bool AllowAutoRedirect
		{
			get
			{
				return this.request.AllowAutoRedirect;
			}
			set
			{
				this.request.AllowAutoRedirect = value;
			}
		}

		public bool AllowWriteStreamBuffering
		{
			get
			{
				return this.request.AllowWriteStreamBuffering;
			}
			set
			{
				this.request.AllowWriteStreamBuffering = value;
			}
		}

		public bool HaveResponse
		{
			get
			{
				return this.request.HaveResponse;
			}
		}

		public bool KeepAlive
		{
			get
			{
				return this.request.KeepAlive;
			}
			set
			{
				this.request.KeepAlive = value;
			}
		}

		public bool Pipelined
		{
			get
			{
				return this.request.Pipelined;
			}
			set
			{
				this.request.Pipelined = value;
			}
		}

		public bool PreAuthenticate
		{
			get
			{
				return this.request.PreAuthenticate;
			}
			set
			{
				this.request.PreAuthenticate = value;
			}
		}

		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.request.UnsafeAuthenticatedConnectionSharing;
			}
			set
			{
				this.request.UnsafeAuthenticatedConnectionSharing = value;
			}
		}

		public bool SendChunked
		{
			get
			{
				return this.request.SendChunked;
			}
			set
			{
				this.request.SendChunked = value;
			}
		}

		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.request.AutomaticDecompression;
			}
			set
			{
				this.request.AutomaticDecompression = value;
			}
		}

		public int MaximumResponseHeadersLength
		{
			get
			{
				return this.request.MaximumResponseHeadersLength;
			}
			set
			{
				this.request.MaximumResponseHeadersLength = value;
			}
		}

		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.request.ClientCertificates;
			}
			set
			{
				this.request.ClientCertificates = value;
			}
		}

		public CookieContainer CookieContainer
		{
			get
			{
				return this.request.CookieContainer;
			}
			set
			{
				this.request.CookieContainer = value;
			}
		}

		public Uri RequestUri
		{
			get
			{
				return this.request.RequestUri;
			}
		}

		public long ContentLength
		{
			get
			{
				return this.request.ContentLength;
			}
			set
			{
				this.request.ContentLength = value;
			}
		}

		public int Timeout
		{
			get
			{
				return this.request.Timeout;
			}
			set
			{
				this.request.Timeout = value;
			}
		}

		public int ReadWriteTimeout
		{
			get
			{
				return this.request.ReadWriteTimeout;
			}
			set
			{
				this.request.ReadWriteTimeout = value;
			}
		}

		public Uri Address
		{
			get
			{
				return this.request.Address;
			}
		}

		public ServicePoint ServicePoint
		{
			get
			{
				return this.request.ServicePoint;
			}
		}

		public int MaximumAutomaticRedirections
		{
			get
			{
				return this.request.MaximumAutomaticRedirections;
			}
			set
			{
				this.request.MaximumAutomaticRedirections = value;
			}
		}

		public ICredentials Credentials
		{
			get
			{
				return this.request.Credentials;
			}
			set
			{
				this.request.Credentials = value;
			}
		}

		public bool UseDefaultCredentials
		{
			get
			{
				return this.request.UseDefaultCredentials;
			}
			set
			{
				this.request.UseDefaultCredentials = value;
			}
		}

		public string ConnectionGroupName
		{
			get
			{
				return this.request.ConnectionGroupName;
			}
			set
			{
				this.request.ConnectionGroupName = value;
			}
		}

		public WebHeaderCollection Headers
		{
			get
			{
				return this.request.Headers;
			}
			set
			{
				this.request.Headers = value;
			}
		}

		public IWebProxy Proxy
		{
			get
			{
				return this.request.Proxy;
			}
			set
			{
				this.request.Proxy = value;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				return this.request.ProtocolVersion;
			}
			set
			{
				this.request.ProtocolVersion = value;
			}
		}

		public string ContentType
		{
			get
			{
				return this.request.ContentType;
			}
			set
			{
				this.request.ContentType = value;
			}
		}

		public string MediaType
		{
			get
			{
				return this.request.MediaType;
			}
			set
			{
				this.request.MediaType = value;
			}
		}

		public string TransferEncoding
		{
			get
			{
				return this.request.TransferEncoding;
			}
			set
			{
				this.request.TransferEncoding = value;
			}
		}

		public string Connection
		{
			get
			{
				return this.request.Connection;
			}
			set
			{
				this.request.Connection = value;
			}
		}

		public string Accept
		{
			get
			{
				return this.request.Accept;
			}
			set
			{
				this.request.Accept = value;
			}
		}

		public string Referer
		{
			get
			{
				return this.request.Referer;
			}
			set
			{
				this.request.Referer = value;
			}
		}

		public string UserAgent
		{
			get
			{
				return this.request.UserAgent;
			}
			set
			{
				this.request.UserAgent = value;
			}
		}

		public string Expect
		{
			get
			{
				return this.request.Expect;
			}
			set
			{
				this.request.Expect = value;
			}
		}

		public DateTime IfModifiedSince
		{
			get
			{
				return this.request.IfModifiedSince;
			}
			set
			{
				this.request.IfModifiedSince = value;
			}
		}

		HttpWebRequest request;
	}
}
