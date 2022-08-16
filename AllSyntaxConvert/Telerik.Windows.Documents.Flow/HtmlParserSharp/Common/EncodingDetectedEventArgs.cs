using System;

namespace HtmlParserSharp.Common
{
	class EncodingDetectedEventArgs : EventArgs
	{
		public string Encoding { get; set; }

		public bool AcceptEncoding { get; set; }

		public EncodingDetectedEventArgs(string encoding)
		{
			this.Encoding = encoding;
			this.AcceptEncoding = false;
		}
	}
}
