using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class ComparisonOperatorExpression : BinaryOperatorExpression<ConstantExpression>
	{
		public override ArgumentType OperandsType
		{
			get
			{
				return ArgumentType.Any;
			}
		}

		protected ComparisonOperatorExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		protected override RadExpression GetValueOverride(ConstantExpression[] operands)
		{
			ConstantExpression constantExpression = ((operands[0] is EmptyExpression) ? NumberExpression.Zero : operands[0]);
			ConstantExpression constantExpression2 = ((operands[1] is EmptyExpression) ? NumberExpression.Zero : operands[1]);
			bool value;
			if (constantExpression.GetType().Equals(constantExpression2.GetType()))
			{
				value = this.CompareSameTypeExpressions(constantExpression, constantExpression2);
			}
			else
			{
				value = this.CompareDifferentTypeExpressions(constantExpression, constantExpression2);
			}
			return value.ToBooleanExpression();
		}

		bool CompareSameTypeExpressions(RadExpression leftOperandValue, RadExpression rightOperandValue)
		{
			NumberExpression numberExpression = leftOperandValue as NumberExpression;
			NumberExpression numberExpression2 = rightOperandValue as NumberExpression;
			if (numberExpression != null && numberExpression2 != null)
			{
				return this.CompareNumberExpressions(numberExpression, numberExpression2);
			}
			StringExpression stringExpression = leftOperandValue as StringExpression;
			StringExpression stringExpression2 = rightOperandValue as StringExpression;
			if (stringExpression != null && stringExpression2 != null)
			{
				return this.CompareStringExpressions(stringExpression, stringExpression2);
			}
			BooleanExpression booleanExpression = leftOperandValue as BooleanExpression;
			BooleanExpression booleanExpression2 = rightOperandValue as BooleanExpression;
			if (booleanExpression != null && booleanExpression2 != null)
			{
				return this.CompareBooleanExpressions(booleanExpression, booleanExpression2);
			}
			EmptyExpression emptyExpression = leftOperandValue as EmptyExpression;
			EmptyExpression emptyExpression2 = rightOperandValue as EmptyExpression;
			return emptyExpression != null && emptyExpression2 != null;
		}

		internal int GetExpressionTypeNumber(ConstantExpression expression)
		{
			int result = 0;
			if (expression is NumberExpression)
			{
				result = 1;
			}
			else if (expression is StringExpression)
			{
				result = 2;
			}
			else if (expression is BooleanExpression)
			{
				result = 4;
			}
			return result;
		}

		protected abstract bool CompareStringExpressions(StringExpression left, StringExpression right);

		protected abstract bool CompareNumberExpressions(NumberExpression left, NumberExpression right);

		protected abstract bool CompareBooleanExpressions(BooleanExpression left, BooleanExpression right);

		protected abstract bool CompareDifferentTypeExpressions(ConstantExpression leftOperandValue, ConstantExpression rightOperandValue);
	}
}
