using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class SignFunctionExpression : SingleArgumentFunctionExpression
	{
		public SignFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = (double)Math.Sign(base.Argument.GetResult().Value);
			return new ExpressionResult(value);
		}
	}
}
