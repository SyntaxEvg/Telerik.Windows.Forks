using System;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class FunctionWithSameTypeArguments<T> : FunctionWithArguments
	{
		T[] ConvertArguments(object[] arguments)
		{
			T[] array = new T[arguments.Length];
			for (int i = 0; i < arguments.Length; i++)
			{
				array[i] = (T)((object)arguments[i]);
			}
			return array;
		}

		protected sealed override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			T[] arguments = this.ConvertArguments(context.Arguments.ToArray<object>());
			FunctionEvaluationContext<T> context2 = new FunctionEvaluationContext<T>(arguments, context.Worksheet, context.RowIndex, context.ColumnIndex);
			return this.EvaluateOverride(context2);
		}

		protected virtual RadExpression EvaluateOverride(FunctionEvaluationContext<T> context)
		{
			throw new NotSupportedException();
		}
	}
}
