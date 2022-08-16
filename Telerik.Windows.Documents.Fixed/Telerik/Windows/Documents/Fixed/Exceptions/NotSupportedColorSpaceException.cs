using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedColorSpaceException : NotSupportedFeatureException
	{
		public NotSupportedColorSpaceException()
			: base("Color space is not supported.")
		{
		}

		public NotSupportedColorSpaceException(string colorSpace)
			: this(colorSpace, string.Format("{0} color space is not supported.", colorSpace))
		{
		}

		public NotSupportedColorSpaceException(string colorSpace, string message)
			: base(message)
		{
			this.ColorSpace = colorSpace;
		}

		public NotSupportedColorSpaceException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedColorSpaceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.ColorSpace = info.GetString("ColorSpace");
		}

		public string ColorSpace { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ColorSpace", this.ColorSpace);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Color space is not supported.";

		const string ColorSpaceNotSupportedMessage = "{0} color space is not supported.";
	}
}
