using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Fact : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Fact.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Fact.Info;
			}
		}

		static Fact()
		{
			string description = "Returns the factorial of a number. The factorial of a number is equal to 1*2*3*...* number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Fact_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the nonnegative number you want the factorial of. If number is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Fact_NumberInfo")
			};
			Fact.Info = new FunctionInfo(Fact.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			int num = (int)context.Arguments[0];
			if (num < 0 || num > 170)
			{
				return ErrorExpressions.NumberError;
			}
			double number = MathUtility.Factorial((double)num);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "FACT";

		static readonly FunctionInfo Info;
	}
}
