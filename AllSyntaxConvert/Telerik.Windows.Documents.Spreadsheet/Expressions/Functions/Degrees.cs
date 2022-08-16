using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Degrees : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Degrees.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Degrees.Info;
			}
		}

		static Degrees()
		{
			string description = "Converts radians into degrees.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Degrees_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Angle", "is the angle in radians that you want to convert.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Angle", "Spreadsheet_Functions_Degrees_AngleInfo")
			};
			Degrees.Info = new FunctionInfo(Degrees.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = context.Arguments[0] * 180.0 / 3.141592653589793;
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DEGREES";

		static readonly FunctionInfo Info;
	}
}
