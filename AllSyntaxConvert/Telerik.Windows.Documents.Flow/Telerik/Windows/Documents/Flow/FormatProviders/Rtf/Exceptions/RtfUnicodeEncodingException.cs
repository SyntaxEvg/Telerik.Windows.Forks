using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfUnicodeEncodingException : RtfEncodingException
	{
		public RtfUnicodeEncodingException()
		{
		}

		public RtfUnicodeEncodingException(string message)
			: base(message)
		{
		}

		public RtfUnicodeEncodingException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfUnicodeEncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
