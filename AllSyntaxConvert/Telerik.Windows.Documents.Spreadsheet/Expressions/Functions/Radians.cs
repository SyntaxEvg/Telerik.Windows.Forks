using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Radians : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Radians.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Radians.Info;
			}
		}

		static Radians()
		{
			string description = "Converts degrees to radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Radians_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Angle", "is an angle in degrees that you want to convert.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Angle", "Spreadsheet_Functions_Radians_AngleInfo")
			};
			Radians.Info = new FunctionInfo(Radians.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = context.Arguments[0] * 3.141592653589793 / 180.0;
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "RADIANS";

		static readonly FunctionInfo Info;
	}
}
