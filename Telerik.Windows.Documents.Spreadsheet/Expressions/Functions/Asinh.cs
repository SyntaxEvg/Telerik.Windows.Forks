using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Asinh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Asinh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Asinh.Info;
			}
		}

		static Asinh()
		{
			string description = "Returns the inverse hyperbolic sine of a number. The inverse hyperbolic sine is the value whose hyperbolic sine is number, so ASINH(SINH(number)) equals number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Asinh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number equal to or greater than 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Asinh_NumberInfo")
			};
			Asinh.Info = new FunctionInfo(Asinh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = MathUtility.Asinh(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ASINH";

		static readonly FunctionInfo Info;
	}
}
