using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedFontException : NotSupportedFeatureException
	{
		public NotSupportedFontException()
			: base("Font type not supported.")
		{
		}

		public NotSupportedFontException(string fontType)
			: this(fontType, string.Format("{0} font is not supported.", fontType))
		{
		}

		public NotSupportedFontException(string fontType, string message)
			: base(message)
		{
			this.FontType = fontType;
		}

		public NotSupportedFontException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedFontException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.FontType = info.GetString("FontType");
		}

		public string FontType { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FontType", this.FontType);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Font type not supported.";

		const string FontTypeNotSupportedMessage = "{0} font is not supported.";
	}
}
