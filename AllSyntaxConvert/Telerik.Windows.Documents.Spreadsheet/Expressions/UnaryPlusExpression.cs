using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class UnaryPlusExpression : UnaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.UnaryPlus;
			}
		}

		public UnaryPlusExpression(RadExpression operand)
			: base(operand)
		{
		}

		protected override RadExpression GetValueOverride(double operand)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(operand);
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new UnaryPlusExpression(base.Operand.CloneAndTranslate(context));
		}
	}
}
