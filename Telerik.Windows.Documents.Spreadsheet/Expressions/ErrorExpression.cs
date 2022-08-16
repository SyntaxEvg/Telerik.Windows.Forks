using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class ErrorExpression : ConstantExpression<string>
	{
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}
		}

		internal ErrorExpression(string value, string message, Exception exception = null)
			: base(value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			Guard.ThrowExceptionIfNullOrEmpty(message, "message");
			this.message = message;
			this.exception = exception;
		}

		readonly string message;

		readonly Exception exception;
	}
}
