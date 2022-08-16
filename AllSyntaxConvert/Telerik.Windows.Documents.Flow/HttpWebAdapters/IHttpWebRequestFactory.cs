using System;

namespace HttpWebAdapters
{
	interface IHttpWebRequestFactory
	{
		IHttpWebRequest Create(Uri url);
	}
}
