using System;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedStreamTypeException : NotSupportedFeatureException
	{
		public NotSupportedStreamTypeException()
			: base("The stream type is not supported.")
		{
		}

		public NotSupportedStreamTypeException(string message)
			: base(message)
		{
		}

		public NotSupportedStreamTypeException(bool supportsRead, bool supportsSeek)
			: this(supportsRead, supportsSeek, NotSupportedStreamTypeException.BuildMessage(supportsRead, supportsSeek))
		{
		}

		public NotSupportedStreamTypeException(bool supportsRead, bool supportsSeek, string message)
			: base(message)
		{
			this.SupportsRead = supportsRead;
			this.SupportsSeek = supportsSeek;
		}

		public NotSupportedStreamTypeException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedStreamTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.SupportsSeek = info.GetBoolean("SupportsSeek");
			this.SupportsRead = info.GetBoolean("SupportsRead");
		}

		public bool SupportsSeek { get; set; }

		public bool SupportsRead { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("SupportsSeek", this.SupportsSeek);
			info.AddValue("SupportsRead", this.SupportsRead);
			base.GetObjectData(info, context);
		}

		static string BuildMessage(bool supportsRead, bool supportsSeek)
		{
			StringBuilder stringBuilder = new StringBuilder("The stream type is not supported.");
			if (!supportsRead)
			{
				stringBuilder.AppendFormat(" {0}", "The input stream should support Read operation.");
			}
			if (!supportsSeek)
			{
				stringBuilder.AppendFormat(" {0}", "The input stream should support Seek operation.");
			}
			return stringBuilder.ToString();
		}

		const string DefaultMessage = "The stream type is not supported.";

		const string CanReadMessage = "The input stream should support Read operation.";

		const string CanSeekMessage = "The input stream should support Seek operation.";
	}
}
