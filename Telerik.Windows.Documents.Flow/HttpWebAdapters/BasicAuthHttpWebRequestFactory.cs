using System;
using System.Net;
using System.Text;
using HttpWebAdapters.Adapters;

namespace HttpWebAdapters
{
	class BasicAuthHttpWebRequestFactory : IHttpWebRequestFactory
	{
		public BasicAuthHttpWebRequestFactory(string username, string password)
		{
			this.username = username;
			this.password = password;
		}

		public IHttpWebRequest Create(string url)
		{
			return this.Create(new Uri(url));
		}

		public IHttpWebRequest Create(Uri url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			string str = Convert.ToBase64String(Encoding.ASCII.GetBytes(this.username + ":" + this.password));
			httpWebRequest.Headers.Add("Authorization", "Basic " + str);
			return new HttpWebRequestAdapter(httpWebRequest);
		}

		readonly string username;

		readonly string password;
	}
}
