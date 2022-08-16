using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class ProductFunctionExpression : FunctionExpression
	{
		public ProductFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = (double)((base.Arguments.Count > 0) ? 1 : 0);
			foreach (Expression expression in base.Arguments)
			{
				num *= expression.GetResult().Value;
				if (num == 0.0)
				{
					break;
				}
			}
			return new ExpressionResult(num);
		}
	}
}
