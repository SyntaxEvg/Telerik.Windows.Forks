using System;
using System.IO;
using System.Net;

namespace HttpWebAdapters
{
	interface IHttpWebResponse : IDisposable
	{
		string GetResponseHeader(string headerName);

		CookieCollection Cookies { get; set; }

		string ContentEncoding { get; }

		string CharacterSet { get; }

		string Server { get; }

		DateTime LastModified { get; }

		HttpStatusCode StatusCode { get; }

		string StatusDescription { get; }

		Version ProtocolVersion { get; }

		string Method { get; }

		void Close();

		Stream GetResponseStream();

		bool IsFromCache { get; }

		bool IsMutuallyAuthenticated { get; }

		long ContentLength { get; set; }

		string ContentType { get; set; }

		Uri ResponseUri { get; }

		WebHeaderCollection Headers { get; }
	}
}
