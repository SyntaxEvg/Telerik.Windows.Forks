using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	public class ValidationError
	{
		internal ValidationError(DocumentElementBase documentElement, string message)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "documentElement");
			Guard.ThrowExceptionIfNullOrEmpty(message, "message");
			this.documentElement = documentElement;
			this.message = message;
		}

		public string Message
		{
			get
			{
				return this.message;
			}
		}

		public DocumentElementBase DocumentElement
		{
			get
			{
				return this.documentElement;
			}
		}

		readonly DocumentElementBase documentElement;

		readonly string message;
	}
}
