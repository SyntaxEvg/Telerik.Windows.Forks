using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	abstract class FunctionExpression : Expression
	{
		internal FunctionExpression(Expression[] arguments)
		{
			Guard.ThrowExceptionIfNull<Expression[]>(arguments, "arguments");
			this.arguments = Array.AsReadOnly<Expression>(arguments);
		}

		public ReadOnlyCollection<Expression> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		readonly ReadOnlyCollection<Expression> arguments;
	}
}
