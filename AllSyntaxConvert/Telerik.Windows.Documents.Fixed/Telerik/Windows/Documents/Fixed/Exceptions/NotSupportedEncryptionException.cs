using System;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Fixed.Exceptions
{
	[Serializable]
	public class NotSupportedEncryptionException : NotSupportedFeatureException
	{
		public NotSupportedEncryptionException()
			: base("The encryption method is not supported.")
		{
		}

		public NotSupportedEncryptionException(int encryptionCode)
			: this(encryptionCode, string.Format("The encryption method with code {0} is not supported.", encryptionCode))
		{
		}

		public NotSupportedEncryptionException(int encryptionCode, string message)
			: base(message)
		{
			this.EncryptionCode = encryptionCode;
		}

		public NotSupportedEncryptionException(string message, Exception cause)
			: base(message, cause)
		{
		}

		protected NotSupportedEncryptionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.EncryptionCode = info.GetInt32("EncryptionCode");
		}

		public int EncryptionCode { get; set; }

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("EncryptionCode", this.EncryptionCode);
			base.GetObjectData(info, context);
		}

		const string DefaultMessage = "The encryption method is not supported.";

		const string FilterTypeNotSupportedMessage = "The encryption method with code {0} is not supported.";
	}
}
