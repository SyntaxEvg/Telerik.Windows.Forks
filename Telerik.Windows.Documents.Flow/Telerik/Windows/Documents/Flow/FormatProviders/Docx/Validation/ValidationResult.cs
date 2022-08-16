using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation
{
	public class ValidationResult
	{
		internal ValidationResult(IEnumerable<ValidationError> validationErrors)
			: this(ValidationResultType.Error, validationErrors)
		{
		}

		internal ValidationResult()
			: this(ValidationResultType.Success, Enumerable.Empty<ValidationError>())
		{
		}

		ValidationResult(ValidationResultType resultType, IEnumerable<ValidationError> validationErrors)
		{
			this.resultType = resultType;
			this.validationErrors = validationErrors;
		}

		public ValidationResultType ResultType
		{
			get
			{
				return this.resultType;
			}
		}

		public IEnumerable<ValidationError> ValidationErrors
		{
			get
			{
				return this.validationErrors;
			}
		}

		readonly ValidationResultType resultType;

		readonly IEnumerable<ValidationError> validationErrors;
	}
}
