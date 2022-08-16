using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class CountFunctionExpression : FunctionExpression
	{
		public CountFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult((double)base.Arguments.Count);
		}
	}
}
