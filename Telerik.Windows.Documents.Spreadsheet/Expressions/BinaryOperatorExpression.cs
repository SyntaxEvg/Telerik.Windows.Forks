using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class BinaryOperatorExpression<T> : OperatorExpression
	{
		public virtual ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NumberFunctionConversion;
			}
		}

		public RadExpression Left
		{
			get
			{
				return this.left;
			}
		}

		public RadExpression Right
		{
			get
			{
				return this.right;
			}
		}

		public abstract ArgumentType OperandsType { get; }

		protected BinaryOperatorExpression(RadExpression left, RadExpression right)
		{
			Guard.ThrowExceptionIfNull<RadExpression>(left, "left");
			Guard.ThrowExceptionIfNull<RadExpression>(right, "right");
			this.left = left;
			this.right = right;
			base.AttachToChildEvent(this.Left);
			base.AttachToChildEvent(this.Right);
		}

		protected sealed override RadExpression GetValueOverride()
		{
			List<object> list = new List<object>();
			ErrorExpression errorExpression = FunctionHelper.TryConvertArgument(this.Left, this.ArgumentConversionRules, this.OperandsType, list);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			errorExpression = FunctionHelper.TryConvertArgument(this.Right, this.ArgumentConversionRules, this.OperandsType, list);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			T[] array = new T[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				if (this.OperandsType == ArgumentType.Any)
				{
					RadExpression valueAsConstantExpression = (list[i] as RadExpression).GetValueAsConstantExpression();
					if (valueAsConstantExpression is ErrorExpression)
					{
						return valueAsConstantExpression;
					}
					list[i] = valueAsConstantExpression;
				}
				array[i] = (T)((object)list[i]);
			}
			return this.GetValueOverride(array);
		}

		protected abstract RadExpression GetValueOverride(T[] operands);

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return base.GetOperandAsString(this.Left, cultureInfo) + this.OperatorInfo.Symbol + base.GetOperandAsString(this.Right, cultureInfo);
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
			this.Left.Translate(context);
			this.Right.Translate(context);
		}

		readonly RadExpression left;

		readonly RadExpression right;
	}
}
