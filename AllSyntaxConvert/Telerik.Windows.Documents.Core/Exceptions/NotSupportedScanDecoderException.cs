using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Exceptions
{
	[Serializable]
	public class NotSupportedScanDecoderException : NotSupportedFeatureException
	{
		public NotSupportedScanDecoderException()
			: base("The scan decoder is not supported.")
		{
		}

		public NotSupportedScanDecoderException(string message)
			: base(message)
		{
		}

		public NotSupportedScanDecoderException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedScanDecoderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		const string DefaultMessage = "The scan decoder is not supported.";
	}
}
