using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class OrFunctionExpression : TwoArgumentFunctionExpression
	{
		public OrFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = (double)((base.FirstArgument.GetResult().Value > 0.0 || base.SecondArgument.GetResult().Value > 0.0) ? 1 : 0);
			return new ExpressionResult(value);
		}
	}
}
