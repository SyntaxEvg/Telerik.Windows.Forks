using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class MinFunctionExpression : FunctionExpression
	{
		public MinFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = ((base.Arguments.Count > 0) ? double.MaxValue : 0.0);
			for (int i = 0; i < base.Arguments.Count; i++)
			{
				num = System.Math.Min(num, base.Arguments[i].GetResult().Value);
			}
			return new ExpressionResult(num);
		}
	}
}
