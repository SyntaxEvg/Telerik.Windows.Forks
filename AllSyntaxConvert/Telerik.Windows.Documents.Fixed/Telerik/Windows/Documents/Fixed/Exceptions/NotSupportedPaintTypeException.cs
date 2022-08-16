using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedPaintTypeException : NotSupportedFeatureException
	{
		public NotSupportedPaintTypeException()
			: base("Paint type is not supported.")
		{
		}

		public NotSupportedPaintTypeException(string message)
			: base(message)
		{
		}

		public NotSupportedPaintTypeException(int paintType)
			: this(paintType, string.Format("Paint type {0} is not supported.", paintType))
		{
		}

		public NotSupportedPaintTypeException(int paintType, string message)
			: base(message)
		{
			this.PaintType = paintType;
		}

		public NotSupportedPaintTypeException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedPaintTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.PaintType = info.GetInt32("PaintType");
		}

		public int PaintType { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("PaintType", this.PaintType);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "Paint type is not supported.";

		const string PaintTypeNotSupportedMessage = "Paint type {0} is not supported.";
	}
}
