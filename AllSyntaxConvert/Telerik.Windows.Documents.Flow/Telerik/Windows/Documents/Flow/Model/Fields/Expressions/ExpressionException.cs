using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	public class ExpressionException : Exception
	{
		public ExpressionException(string message)
			: base(message)
		{
		}

		public ExpressionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
