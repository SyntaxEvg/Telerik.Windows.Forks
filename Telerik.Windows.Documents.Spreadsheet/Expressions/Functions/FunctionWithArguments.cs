using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class FunctionWithArguments : FunctionBase
	{
		ArgumentInfo GetArgumentInfoByIndex(ArgumentInfo[] argumentsInfo, int requiredArgumentsCount, int optionalArgumentsCount, int index)
		{
			ArgumentInfo result;
			if (index < requiredArgumentsCount)
			{
				result = argumentsInfo[index];
			}
			else
			{
				int num = (index - requiredArgumentsCount) % optionalArgumentsCount + requiredArgumentsCount;
				result = argumentsInfo[num];
			}
			return result;
		}

		protected sealed override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			List<object> list = new List<object>();
			ArgumentInfo[] argumentsInfo = this.FunctionInfo.GetArguments(0).ToArray<ArgumentInfo>();
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				ArgumentInfo argumentInfoByIndex = this.GetArgumentInfoByIndex(argumentsInfo, this.FunctionInfo.RequiredArgumentsCount, this.FunctionInfo.OptionalArgumentsCount, i);
				ErrorExpression errorExpression = FunctionHelper.TryConvertArgument(context.Arguments[i], this.ArgumentConversionRules, argumentInfoByIndex.Type, list);
				if (errorExpression != null)
				{
					return errorExpression;
				}
			}
			if (!this.FunctionInfo.IsDefaultValueFunction && !base.IsArgumentNumberValid(list.Count))
			{
				return ErrorExpressions.ValueError;
			}
			FunctionEvaluationContext<object> context2 = new FunctionEvaluationContext<object>(list.ToArray(), context.Worksheet, context.RowIndex, context.ColumnIndex);
			return this.EvaluateOverride(context2);
		}

		protected virtual RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			throw new NotSupportedException();
		}
	}
}
