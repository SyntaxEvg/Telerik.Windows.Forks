using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfHexEncodingException : RtfEncodingException
	{
		public RtfHexEncodingException()
		{
		}

		public RtfHexEncodingException(string message)
			: base(message)
		{
		}

		public RtfHexEncodingException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfHexEncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
