using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Abs : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Abs.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Abs.Info;
			}
		}

		static Abs()
		{
			string description = "Returns the absolute value of a number. The absolute value of a number is the number without its sign.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Abs_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the real number for which you want the absolute value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Abs_NumberInfo")
			};
			Abs.Info = new FunctionInfo(Abs.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Abs(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ABS";

		static readonly FunctionInfo Info;
	}
}
