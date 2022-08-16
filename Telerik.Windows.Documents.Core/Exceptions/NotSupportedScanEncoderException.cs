using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Exceptions
{
	[Serializable]
	public class NotSupportedScanEncoderException : NotSupportedFeatureException
	{
		public NotSupportedScanEncoderException()
			: base("The scan encoder is not supported.")
		{
		}

		public NotSupportedScanEncoderException(string message)
			: base(message)
		{
		}

		public NotSupportedScanEncoderException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedScanEncoderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		const string DefaultMessage = "The scan encoder is not supported.";
	}
}
