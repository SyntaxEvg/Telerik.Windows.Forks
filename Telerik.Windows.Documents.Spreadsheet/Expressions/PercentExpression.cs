using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class PercentExpression : UnaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Percent;
			}
		}

		public PercentExpression(RadExpression operand)
			: base(operand)
		{
		}

		protected override RadExpression GetValueOverride(double operand)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(operand / 100.0);
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new PercentExpression(base.Operand.CloneAndTranslate(context));
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.GetOperandAsString(base.Operand, cultureInfo) + this.OperatorInfo.Symbol;
		}
	}
}
