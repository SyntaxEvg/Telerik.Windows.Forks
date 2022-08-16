using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Rri : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Rri.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Rri.Info;
			}
		}

		static Rri()
		{
			string description = "Returns an equivalent interest rate for the growth of an investment.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Rri_PvInfo";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Nper", "is the number of periods for the investment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Rri_NperInfo"),
				new ArgumentInfo("Pv", "is the present value of the investment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Rri_PvInfo"),
				new ArgumentInfo("Fv", "is the future value of the investment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Rri_FvInfo")
			};
			Rri.Info = new FunctionInfo(Rri.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			double num3 = context.Arguments[2];
			if (num <= 0.0 || (num2 == 0.0 && num3 != 0.0) || (num2 > 0.0 && num3 < 0.0) || (num2 < 0.0 && num3 > 0.0))
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.RRI(num, num2, num3);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "RRI";

		static readonly FunctionInfo Info;
	}
}
