using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Log10 : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Log10.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Log10.Info;
			}
		}

		static Log10()
		{
			string description = "Returns the base-10 logarithm of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Log10_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the positive real number for which you want the base-10 logarithm.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Log10_NumberInfo")
			};
			Log10.Info = new FunctionInfo(Log10.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num <= 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(Math.Log10(num));
		}

		public static readonly string FunctionName = "LOG10";

		static readonly FunctionInfo Info;
	}
}
