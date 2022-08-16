using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	abstract class UnaryOperatorExpression : OperatorExpression
	{
		protected UnaryOperatorExpression(Expression operand)
		{
			Guard.ThrowExceptionIfNull<Expression>(operand, "operand");
			this.operand = operand;
		}

		public Expression Operand
		{
			get
			{
				return this.operand;
			}
		}

		readonly Expression operand;
	}
}
