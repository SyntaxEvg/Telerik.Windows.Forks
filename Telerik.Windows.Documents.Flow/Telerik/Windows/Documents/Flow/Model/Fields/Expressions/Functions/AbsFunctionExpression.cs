using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class AbsFunctionExpression : SingleArgumentFunctionExpression
	{
		public AbsFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = Math.Abs(base.Argument.GetResult().Value);
			return new ExpressionResult(value);
		}
	}
}
