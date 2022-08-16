using System;
using System.Collections.Generic;
using System.Text;

namespace CsQuery.HtmlParser
{
	static class HtmlEncoding
	{
		static Dictionary<string, EncodingInfo> Encodings
		{
			get
			{
				if (HtmlEncoding._Encodings == null)
				{
					HtmlEncoding._Encodings = new Dictionary<string, EncodingInfo>(StringComparer.CurrentCultureIgnoreCase);
					foreach (EncodingInfo encodingInfo in Encoding.GetEncodings())
					{
						HtmlEncoding._Encodings[encodingInfo.Name] = encodingInfo;
					}
				}
				return HtmlEncoding._Encodings;
			}
		}

		public static bool TryGetEncoding(string encodingName, out Encoding encoding)
		{
			EncodingInfo encodingInfo;
			if (HtmlEncoding.Encodings.TryGetValue(encodingName, out encodingInfo))
			{
				encoding = encodingInfo.GetEncoding();
				return true;
			}
			encoding = null;
			return false;
		}

		public static Encoding GetEncoding(string encodingName)
		{
			Encoding result;
			if (HtmlEncoding.TryGetEncoding(encodingName, out result))
			{
				return result;
			}
			return null;
		}

		static Dictionary<string, EncodingInfo> _Encodings;
	}
}
