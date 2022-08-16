using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	abstract class BinaryOperatorExpression : OperatorExpression
	{
		protected BinaryOperatorExpression(Expression left, Expression right)
		{
			Guard.ThrowExceptionIfNull<Expression>(left, "left");
			Guard.ThrowExceptionIfNull<Expression>(right, "right");
			this.left = left;
			this.right = right;
		}

		public Expression Left
		{
			get
			{
				return this.left;
			}
		}

		public Expression Right
		{
			get
			{
				return this.right;
			}
		}

		readonly Expression left;

		readonly Expression right;
	}
}
