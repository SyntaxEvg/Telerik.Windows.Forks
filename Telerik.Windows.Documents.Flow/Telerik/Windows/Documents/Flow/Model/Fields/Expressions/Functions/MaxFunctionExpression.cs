using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class MaxFunctionExpression : FunctionExpression
	{
		public MaxFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = ((base.Arguments.Count > 0) ? double.MinValue : 0.0);
			for (int i = 0; i < base.Arguments.Count; i++)
			{
				num = Math.Max(num, base.Arguments[i].GetResult().Value);
			}
			return new ExpressionResult(num);
		}
	}
}
