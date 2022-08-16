using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfParserException : RtfException
	{
		public RtfParserException()
		{
		}

		public RtfParserException(string message)
			: base(message)
		{
		}

		public RtfParserException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfParserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
