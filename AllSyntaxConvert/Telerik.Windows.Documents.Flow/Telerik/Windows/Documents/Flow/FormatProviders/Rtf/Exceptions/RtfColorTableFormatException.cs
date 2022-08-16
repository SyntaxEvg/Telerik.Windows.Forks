using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfColorTableFormatException : RtfException
	{
		public RtfColorTableFormatException()
		{
		}

		public RtfColorTableFormatException(string message)
			: base(message)
		{
		}

		public RtfColorTableFormatException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfColorTableFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
