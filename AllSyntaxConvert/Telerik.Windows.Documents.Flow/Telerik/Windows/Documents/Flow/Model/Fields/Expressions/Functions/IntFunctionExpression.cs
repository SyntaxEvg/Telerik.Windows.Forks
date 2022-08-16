using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	class IntFunctionExpression : SingleArgumentFunctionExpression
	{
		public IntFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult((double)((int)base.Argument.GetResult().Value));
		}
	}
}
