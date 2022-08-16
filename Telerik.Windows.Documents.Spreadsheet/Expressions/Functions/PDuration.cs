using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class PDuration : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return PDuration.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return PDuration.Info;
			}
		}

		static PDuration()
		{
			string description = "Returns the number of periods required by an investment to reach a specified value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_PDuration_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_PDuration_RateInfo"),
				new ArgumentInfo("Pv", "is the present value of the investment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_PDuration_PvInfo"),
				new ArgumentInfo("Fv", "is the desired future value of the investment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_PDuration_FvInfo")
			};
			PDuration.Info = new FunctionInfo(PDuration.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			double num3 = context.Arguments[2];
			if (num <= 0.0 || num2 <= 0.0 || num3 <= 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.PDURATION(num, num2, num3);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "PDURATION";

		static readonly FunctionInfo Info;
	}
}
