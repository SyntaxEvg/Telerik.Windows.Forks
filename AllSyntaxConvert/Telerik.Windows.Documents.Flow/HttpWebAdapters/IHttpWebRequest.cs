using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace HttpWebAdapters
{
	interface IHttpWebRequest
	{
		HttpWebRequestMethod Method { get; set; }

		IHttpWebResponse GetResponse();

		Stream GetRequestStream();

		void Abort();

		void AddRange(int from, int to);

		void AddRange(int range);

		void AddRange(string rangeSpecifier, int from, int to);

		void AddRange(string rangeSpecifier, int range);

		bool AllowAutoRedirect { get; set; }

		bool AllowWriteStreamBuffering { get; set; }

		bool HaveResponse { get; }

		bool KeepAlive { get; set; }

		bool Pipelined { get; set; }

		bool PreAuthenticate { get; set; }

		bool UnsafeAuthenticatedConnectionSharing { get; set; }

		bool SendChunked { get; set; }

		DecompressionMethods AutomaticDecompression { get; set; }

		int MaximumResponseHeadersLength { get; set; }

		X509CertificateCollection ClientCertificates { get; set; }

		CookieContainer CookieContainer { get; set; }

		Uri RequestUri { get; }

		long ContentLength { get; set; }

		int Timeout { get; set; }

		int ReadWriteTimeout { get; set; }

		Uri Address { get; }

		ServicePoint ServicePoint { get; }

		int MaximumAutomaticRedirections { get; set; }

		ICredentials Credentials { get; set; }

		bool UseDefaultCredentials { get; set; }

		string ConnectionGroupName { get; set; }

		WebHeaderCollection Headers { get; set; }

		IWebProxy Proxy { get; set; }

		Version ProtocolVersion { get; set; }

		string ContentType { get; set; }

		string MediaType { get; set; }

		string TransferEncoding { get; set; }

		string Connection { get; set; }

		string Accept { get; set; }

		string Referer { get; set; }

		string UserAgent { get; set; }

		string Expect { get; set; }

		DateTime IfModifiedSince { get; set; }

		IAsyncResult BeginGetResponse(AsyncCallback callback, object state);

		IHttpWebResponse EndGetResponse(IAsyncResult result);

		IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);

		Stream EndGetRequestStream(IAsyncResult result);
	}
}
