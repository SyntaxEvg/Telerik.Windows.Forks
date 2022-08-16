using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sqrt : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sqrt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sqrt.Info;
			}
		}

		static Sqrt()
		{
			string description = "Returns the square root of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sqrt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number for which you want the square root.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sqrt_NumberInfo")
			};
			Sqrt.Info = new FunctionInfo(Sqrt.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number = Math.Sqrt(num);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SQRT";

		static readonly FunctionInfo Info;
	}
}
