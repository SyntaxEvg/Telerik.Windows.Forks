using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class ModFunctionExpression : TwoArgumentFunctionExpression
	{
		public ModFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = base.FirstArgument.GetResult().Value;
			double value2 = base.SecondArgument.GetResult().Value;
			double value3 = value % value2;
			return new ExpressionResult(value3);
		}
	}
}
