using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class NumberBinaryOperatorExpression : BinaryOperatorExpression<double>
	{
		public override ArgumentType OperandsType
		{
			get
			{
				return ArgumentType.Number;
			}
		}

		protected NumberBinaryOperatorExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}
	}
}
