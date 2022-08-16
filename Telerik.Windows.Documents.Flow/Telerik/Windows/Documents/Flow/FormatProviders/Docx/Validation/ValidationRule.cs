using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	class ValidationRule<T> : IValidationRule where T : DocumentElementBase
	{
		public ValidationRule(Func<T, bool> predicate, string errorMessage, bool isDocumentStructureChanging)
		{
			Guard.ThrowExceptionIfNull<Func<T, bool>>(predicate, "predicate");
			Guard.ThrowExceptionIfNullOrEmpty(errorMessage, "errorMessage");
			this.predicate = predicate;
			this.errorMessage = errorMessage;
			this.isDocumentChanging = isDocumentStructureChanging;
		}

		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
		}

		public bool IsDocumentChanging
		{
			get
			{
				return this.isDocumentChanging;
			}
		}

		public bool IsCompliant(DocumentElementBase documentElement)
		{
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "documentElement");
			return this.predicate((T)((object)documentElement));
		}

		readonly Func<T, bool> predicate;

		readonly string errorMessage;

		readonly bool isDocumentChanging;
	}
}
