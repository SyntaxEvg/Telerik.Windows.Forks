using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfMultiByteEncodingException : RtfEncodingException
	{
		public RtfMultiByteEncodingException()
		{
		}

		public RtfMultiByteEncodingException(string message)
			: base(message)
		{
		}

		public RtfMultiByteEncodingException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfMultiByteEncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
