using System;

namespace HttpWebAdapters
{
	class HttpWebRequestMethod
	{
		HttpWebRequestMethod(string m)
		{
			this.method = m;
		}

		public override string ToString()
		{
			return this.method;
		}

		public static HttpWebRequestMethod Parse(string s)
		{
			if (s == HttpWebRequestMethod.SGET)
			{
				return HttpWebRequestMethod.GET;
			}
			return HttpWebRequestMethod.POST;
		}

		string method;

		static readonly string SGET = "GET";

		static readonly string SPOST = "POST";

		public static readonly HttpWebRequestMethod GET = new HttpWebRequestMethod(HttpWebRequestMethod.SGET);

		public static readonly HttpWebRequestMethod POST = new HttpWebRequestMethod(HttpWebRequestMethod.SPOST);
	}
}
