using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sinh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sinh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sinh.Info;
			}
		}

		static Sinh()
		{
			string description = "Returns the hyperbolic sine of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sinh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sinh_NumberInfo")
			};
			Sinh.Info = new FunctionInfo(Sinh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Sinh(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SINH";

		static readonly FunctionInfo Info;
	}
}
