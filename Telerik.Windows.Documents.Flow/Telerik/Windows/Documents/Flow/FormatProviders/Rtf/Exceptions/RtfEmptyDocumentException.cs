using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfEmptyDocumentException : RtfStructureException
	{
		public RtfEmptyDocumentException()
		{
		}

		public RtfEmptyDocumentException(string message)
			: base(message)
		{
		}

		public RtfEmptyDocumentException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected RtfEmptyDocumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
