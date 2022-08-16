using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class NotFunctionExpression : SingleArgumentFunctionExpression
	{
		public NotFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = (double)((Math.Sign(base.Argument.GetResult().Value) != 0) ? 0 : 1);
			return new ExpressionResult(value);
		}
	}
}
