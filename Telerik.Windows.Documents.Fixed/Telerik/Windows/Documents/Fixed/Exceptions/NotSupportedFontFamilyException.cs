using System;
using System.Runtime.Serialization;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedFontFamilyException : NotSupportedFeatureException
	{
		public NotSupportedFontFamilyException()
			: base("The FontFamily is not supported.")
		{
		}

		public NotSupportedFontFamilyException(string message)
			: base(message)
		{
		}

		public NotSupportedFontFamilyException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedFontFamilyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		const string DefaultMessage = "The FontFamily is not supported.";
	}
}
