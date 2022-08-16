using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class ErrorExpression : Expression
	{
		public ErrorExpression(ExpressionException error)
		{
			this.error = error;
		}

		public string ErrorMessage
		{
			get
			{
				return this.error.Message;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(this.error);
		}

		readonly ExpressionException error;
	}
}
