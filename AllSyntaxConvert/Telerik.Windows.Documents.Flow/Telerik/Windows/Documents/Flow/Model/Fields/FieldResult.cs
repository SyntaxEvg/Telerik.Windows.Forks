using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	class FieldResult
	{
		public FieldResult(string result)
			: this(result, false)
		{
		}

		public FieldResult(string result, bool isError)
		{
			Guard.ThrowExceptionIfNull<string>(result, "result");
			this.isError = isError;
			this.result = result;
		}

		public string Result
		{
			get
			{
				return this.result;
			}
		}

		public bool IsError
		{
			get
			{
				return this.isError;
			}
		}

		readonly string result;

		readonly bool isError;
	}
}
