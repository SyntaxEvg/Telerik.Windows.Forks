using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class ConstantExpression : RadExpression
	{
		protected override RadExpression GetValueOverride()
		{
			return this;
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return this;
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
		}
	}
}
