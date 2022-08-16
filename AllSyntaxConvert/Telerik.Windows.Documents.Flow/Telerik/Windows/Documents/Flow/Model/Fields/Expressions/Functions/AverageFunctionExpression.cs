using System;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class AverageFunctionExpression : FunctionExpression
	{
		public AverageFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = base.Arguments.Sum((Expression arg) => arg.GetResult().Value);
			num /= (double)base.Arguments.Count;
			return new ExpressionResult(num);
		}
	}
}
