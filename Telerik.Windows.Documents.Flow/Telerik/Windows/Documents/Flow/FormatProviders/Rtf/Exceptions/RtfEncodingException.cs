using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfEncodingException : RtfParserException
	{
		public RtfEncodingException()
		{
		}

		public RtfEncodingException(string message)
			: base(message)
		{
		}

		public RtfEncodingException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfEncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
