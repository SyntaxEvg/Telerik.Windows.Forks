using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfStructureException : RtfParserException
	{
		public RtfStructureException()
		{
		}

		public RtfStructureException(string message)
			: base(message)
		{
		}

		public RtfStructureException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfStructureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
