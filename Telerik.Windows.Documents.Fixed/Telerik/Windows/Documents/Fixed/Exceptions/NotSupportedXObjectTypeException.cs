using System;
using System.Runtime.Serialization;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedXObjectTypeException : NotSupportedFeatureException
	{
		public NotSupportedXObjectTypeException()
			: base("XObject is not supported.")
		{
		}

		public NotSupportedXObjectTypeException(string message)
			: base(message)
		{
		}

		public NotSupportedXObjectTypeException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedXObjectTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		const string DefaultMessage = "XObject is not supported.";
	}
}
