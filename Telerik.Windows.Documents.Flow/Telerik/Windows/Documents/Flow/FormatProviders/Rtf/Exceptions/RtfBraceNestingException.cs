using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfBraceNestingException : RtfStructureException
	{
		public RtfBraceNestingException()
		{
		}

		public RtfBraceNestingException(string message)
			: base(message)
		{
		}

		public RtfBraceNestingException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfBraceNestingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
