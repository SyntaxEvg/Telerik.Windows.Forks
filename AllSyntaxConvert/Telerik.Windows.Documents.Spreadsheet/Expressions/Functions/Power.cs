using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Power : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Power.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Power.Info;
			}
		}

		static Power()
		{
			string description = "Returns the result of a number raised to a power.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Power_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the base number, any real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Power_NumberInfo"),
				new ArgumentInfo("Power", "is the exponent, to which the base number is raised.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Power", "Spreadsheet_Functions_Power_PowerInfo")
			};
			Power.Info = new FunctionInfo(Power.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num == 0.0)
			{
				if (num2 == 0.0)
				{
					return ErrorExpressions.NumberError;
				}
				if (num2 < 0.0)
				{
					return ErrorExpressions.DivisionByZero;
				}
			}
			double number = Math.Pow(num, num2);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "POWER";

		static readonly FunctionInfo Info;
	}
}
