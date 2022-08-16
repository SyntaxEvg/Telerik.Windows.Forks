using System;
using System.Runtime.Serialization;

namespace Telerik.Windows.Documents.Exceptions
{
	[Serializable]
	public class NotSupportedFeatureException : Exception
	{
		public NotSupportedFeatureException()
		{
		}

		public NotSupportedFeatureException(string message)
			: base(message)
		{
		}

		public NotSupportedFeatureException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedFeatureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
