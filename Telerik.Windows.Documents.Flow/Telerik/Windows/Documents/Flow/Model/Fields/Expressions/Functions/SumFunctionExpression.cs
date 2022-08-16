using System;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class SumFunctionExpression : FunctionExpression
	{
		public SumFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = base.Arguments.Sum((Expression arg) => arg.GetResult().Value);
			return new ExpressionResult(value);
		}
	}
}
