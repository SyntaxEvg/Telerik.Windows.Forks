using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfException : Exception
	{
		public RtfException()
		{
		}

		public RtfException(string message)
			: base(message)
		{
		}

		public RtfException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
