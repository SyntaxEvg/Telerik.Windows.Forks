using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class RoundFunctionExpression : TwoArgumentFunctionExpression
	{
		public RoundFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double num = base.FirstArgument.GetResult().Value;
			int num2 = (int)base.SecondArgument.GetResult().Value;
			int num3 = Math.Sign(num);
			num *= (double)num3;
			double value = (double)num3 * Math.Floor(num * Math.Pow(10.0, (double)num2)) / Math.Pow(10.0, (double)num2);
			return new ExpressionResult(value);
		}
	}
}
