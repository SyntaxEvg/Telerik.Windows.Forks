using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	abstract class TwoArgumentFunctionExpression : FunctionExpression
	{
		public TwoArgumentFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(2, 2, arguments.Length, "arguments");
		}

		protected Expression FirstArgument
		{
			get
			{
				return base.Arguments[0];
			}
		}

		protected Expression SecondArgument
		{
			get
			{
				return base.Arguments[1];
			}
		}
	}
}
