using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	interface IValidationRule
	{
		string ErrorMessage { get; }

		bool IsDocumentChanging { get; }

		bool IsCompliant(DocumentElementBase documentElement);
	}
}
