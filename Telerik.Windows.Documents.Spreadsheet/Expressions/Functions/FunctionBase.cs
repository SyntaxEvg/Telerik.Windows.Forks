using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class FunctionBase
	{
		public abstract string Name { get; }

		public abstract FunctionInfo FunctionInfo { get; }

		public virtual ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NumberFunctionConversion;
			}
		}

		public RadExpression Evaluate(FunctionEvaluationContext<RadExpression> context)
		{
			return this.EvaluateOverride(context);
		}

		protected virtual RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			throw new NotSupportedException();
		}

		public bool IsArgumentNumberValid(int argumentsCount)
		{
			int optionalArgumentsCount = this.FunctionInfo.OptionalArgumentsCount;
			int num = ((this.FunctionInfo.OptionalArgumentsRepetitionCount > 0) ? this.FunctionInfo.OptionalArgumentsRepetitionCount : 1);
			int requiredArgumentsCount = this.FunctionInfo.RequiredArgumentsCount;
			int num2 = requiredArgumentsCount + optionalArgumentsCount * num;
			bool flag = argumentsCount >= requiredArgumentsCount && argumentsCount <= num2;
			if (optionalArgumentsCount > 1 && this.FunctionInfo.OptionalArgumentsRepetitionCount > 1)
			{
				flag &= (argumentsCount - requiredArgumentsCount) % optionalArgumentsCount == 0;
			}
			return flag;
		}
	}
}
