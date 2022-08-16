using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class UnaryOperatorExpression : OperatorExpression
	{
		public RadExpression Operand
		{
			get
			{
				return this.operand;
			}
		}

		public virtual ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NumberFunctionConversion;
			}
		}

		protected UnaryOperatorExpression(RadExpression operand)
		{
			Guard.ThrowExceptionIfNull<RadExpression>(operand, "operand");
			this.operand = operand;
			base.AttachToChildEvent(this.operand);
		}

		protected sealed override RadExpression GetValueOverride()
		{
			List<object> list = new List<object>();
			ErrorExpression errorExpression = FunctionHelper.TryConvertArgument(this.operand, this.ArgumentConversionRules, ArgumentType.Number, list);
			if (errorExpression != null || list.Count < 1 || !(list[0] is double))
			{
				return errorExpression;
			}
			return this.GetValueOverride((double)list[0]);
		}

		protected abstract RadExpression GetValueOverride(double operand);

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.OperatorInfo.Symbol + this.GetOperandAsString(this.Operand, cultureInfo);
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
			this.Operand.Translate(context);
		}

		readonly RadExpression operand;
	}
}
