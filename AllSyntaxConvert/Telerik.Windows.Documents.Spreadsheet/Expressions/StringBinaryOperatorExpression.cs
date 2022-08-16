using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class StringBinaryOperatorExpression : BinaryOperatorExpression<string>
	{
		public override ArgumentType OperandsType
		{
			get
			{
				return ArgumentType.Text;
			}
		}

		protected StringBinaryOperatorExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}
	}
}
