using System;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class AmpersandExpression : StringBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Ampersand;
			}
		}

		public AmpersandExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		protected override RadExpression GetValueOverride(string[] operands)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(operands[0]);
			stringBuilder.Append(operands[1]);
			return new StringExpression(stringBuilder.ToString());
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new AmpersandExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}
	}
}
