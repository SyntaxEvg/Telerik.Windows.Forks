using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public static class ErrorExpressions
	{
		static ErrorExpressions()
		{
			ErrorExpressions.NullError = ErrorExpressions.CreateErrorExpression("#NULL!", SpreadsheetStrings.NullErrorMessage, null);
			ErrorExpressions.DivisionByZero = ErrorExpressions.CreateErrorExpression("#DIV/0!", SpreadsheetStrings.DivideByZeroErrorMessage, null);
			ErrorExpressions.ValueError = ErrorExpressions.CreateErrorExpression("#VALUE!", SpreadsheetStrings.ValueErrorMessage, null);
			ErrorExpressions.NameError = ErrorExpressions.CreateErrorExpression("#NAME?", SpreadsheetStrings.NameErrorMessage, null);
			ErrorExpressions.ReferenceError = ErrorExpressions.CreateErrorExpression("#REF!", SpreadsheetStrings.ReferenceErrorMessage, null);
			ErrorExpressions.NumberError = ErrorExpressions.CreateErrorExpression("#NUM!", SpreadsheetStrings.NumberErrorMessage, null);
			ErrorExpressions.NotAvailableError = ErrorExpressions.CreateErrorExpression("#N/A", SpreadsheetStrings.NotAvailableErrorMessage, null);
			ErrorExpressions.CyclicReference = ErrorExpressions.CreateErrorExpression("0", SpreadsheetStrings.CyclicReferenceMessage, new ExpressionException(SpreadsheetStrings.CyclicReferenceErrorMessage, "Spreadsheet_ErrorExpressions_CyclicReferenceErrorMessage", null));
		}

		internal static ErrorExpression FindErrorExpression(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			return ErrorExpressions.errors[value];
		}

		static ErrorExpression CreateErrorExpression(string value, string message, Exception exception = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			Guard.ThrowExceptionIfNullOrEmpty(message, "message");
			ErrorExpression errorExpression = new ErrorExpression(value, message, exception);
			ErrorExpressions.errors[value] = errorExpression;
			return errorExpression;
		}

		public static readonly ErrorExpression NullError;

		public static readonly ErrorExpression DivisionByZero;

		public static readonly ErrorExpression ValueError;

		public static readonly ErrorExpression NameError;

		public static readonly ErrorExpression ReferenceError;

		public static readonly ErrorExpression NumberError;

		public static readonly ErrorExpression NotAvailableError;

		public static readonly ErrorExpression CyclicReference;

		static readonly Dictionary<string, ErrorExpression> errors = new Dictionary<string, ErrorExpression>();
	}
}
