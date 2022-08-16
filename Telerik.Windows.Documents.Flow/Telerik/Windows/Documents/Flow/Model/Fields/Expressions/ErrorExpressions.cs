using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	static class ErrorExpressions
	{
		static ErrorExpressions()
		{
			ErrorExpressions.InitializeErrorExpressions();
		}

		public static ExpressionResult GetExpressionResult(ExpressionErrorType error)
		{
			return ErrorExpressions.errorExpressions[error];
		}

		static void InitializeErrorExpressions()
		{
			Func<string, ExpressionResult> func = (string message) => new ExpressionResult(new ExpressionException(message));
			ErrorExpressions.errorExpressions.Add(ExpressionErrorType.UndefinedBookmark, func("Undefined bookmark."));
			ErrorExpressions.errorExpressions.Add(ExpressionErrorType.DivisionByZero, func("You cannot divide by zero."));
			ErrorExpressions.errorExpressions.Add(ExpressionErrorType.ExponentNotAnInteger, func("Exponent is not an integer."));
		}

		static readonly Dictionary<ExpressionErrorType, ExpressionResult> errorExpressions = new Dictionary<ExpressionErrorType, ExpressionResult>();
	}
}
