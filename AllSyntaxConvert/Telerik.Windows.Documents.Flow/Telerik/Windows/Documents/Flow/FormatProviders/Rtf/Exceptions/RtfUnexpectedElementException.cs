using System;
using System.Runtime.Serialization;
using System.Security;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions
{
	[Serializable]
	public class RtfUnexpectedElementException : RtfException
	{
		public RtfUnexpectedElementException()
		{
		}

		public RtfUnexpectedElementException(string message)
			: base(message)
		{
		}

		public RtfUnexpectedElementException(string message, Exception cause)
			: base(message, cause)
		{
		}

		public RtfUnexpectedElementException(string expected, string actual)
			: this(expected, actual, null)
		{
		}

		public RtfUnexpectedElementException(string expected, string actual, string message)
			: base(message)
		{
			this.Expected = expected;
			this.Actual = actual;
		}

		protected RtfUnexpectedElementException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Expected = info.GetString("Expected");
			this.Actual = info.GetString("Actual");
		}

		public string Expected { get; set; }

		public string Actual { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Expected", this.Expected);
			info.AddValue("Actual", this.Actual);
			base.GetObjectData(info, context);
		}
	}
}
