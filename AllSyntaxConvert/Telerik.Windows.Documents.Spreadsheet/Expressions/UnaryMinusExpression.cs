using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class UnaryMinusExpression : UnaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.UnaryMinus;
			}
		}

		public UnaryMinusExpression(RadExpression operand)
			: base(operand)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new UnaryMinusExpression(base.Operand.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double operand)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 * operand);
		}
	}
}
