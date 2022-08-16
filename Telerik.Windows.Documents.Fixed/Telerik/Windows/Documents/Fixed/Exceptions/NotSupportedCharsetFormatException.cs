using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedCharsetFormatException : NotSupportedFeatureException
	{
		public NotSupportedCharsetFormatException()
			: base("Charset format is not supported.")
		{
		}

		public NotSupportedCharsetFormatException(int charsetFormat)
			: this(charsetFormat, string.Format("Charset format {0} is not supported.", charsetFormat))
		{
		}

		public NotSupportedCharsetFormatException(int charsetFormat, string message)
			: base(message)
		{
			this.CharsetFormat = charsetFormat;
		}

		public NotSupportedCharsetFormatException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedCharsetFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.CharsetFormat = info.GetInt32("CharsetFormat");
		}

		public int CharsetFormat { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("CharsetFormat", this.CharsetFormat);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Charset format is not supported.";

		const string CharsetFormatNotSupportedMessage = "Charset format {0} is not supported.";
	}
}
