using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	[Serializable]
	public class InvalidDocumentException : Exception
	{
		public InvalidDocumentException()
			: this(string.Empty)
		{
		}

		public InvalidDocumentException(string message)
			: this(message, null)
		{
		}

		public InvalidDocumentException(string message, Exception innerException)
			: base(message, innerException)
		{
			this.validationErrors = Enumerable.Empty<ValidationError>();
		}

		public InvalidDocumentException(IEnumerable<ValidationError> validationErrors)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<ValidationError>>(validationErrors, "validationErrors");
			this.validationErrors = validationErrors;
		}

		protected InvalidDocumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.validationErrors = Enumerable.Empty<ValidationError>();
		}

		public IEnumerable<ValidationError> ValidationErrors
		{
			get
			{
				return this.validationErrors;
			}
		}

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		readonly IEnumerable<ValidationError> validationErrors;
	}
}
