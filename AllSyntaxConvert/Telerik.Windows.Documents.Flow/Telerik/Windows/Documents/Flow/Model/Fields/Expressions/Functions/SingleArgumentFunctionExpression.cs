using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	abstract class SingleArgumentFunctionExpression : FunctionExpression
	{
		public SingleArgumentFunctionExpression(Expression[] arguments)
			: base(arguments)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 1, arguments.Length, "arguments");
		}

		protected Expression Argument
		{
			get
			{
				return base.Arguments[0];
			}
		}
	}
}
