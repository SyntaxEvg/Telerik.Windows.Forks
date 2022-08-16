using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Mod : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Mod.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Mod.Info;
			}
		}

		static Mod()
		{
			string description = "Returns the remainder after number is divided by divisor. The result has the same sign as divisor.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Mod_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number for which you want to find the remainder.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Mod_NumberInfo"),
				new ArgumentInfo("Divisor", "is the number by which you want to divide number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Divisor", "Spreadsheet_Functions_Mod_DivisorInfo")
			};
			Mod.Info = new FunctionInfo(Mod.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = context.Arguments[0];
			double num = context.Arguments[1];
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Mod(number, num));
		}

		public static readonly string FunctionName = "MOD";

		static readonly FunctionInfo Info;
	}
}
