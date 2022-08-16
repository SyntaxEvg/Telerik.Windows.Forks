using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sec : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sec.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sec.Info;
			}
		}

		static Sec()
		{
			string description = "Returns the secant of an angle specified in radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sec_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the secant.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sec_NumberInfo")
			};
			Sec.Info = new FunctionInfo(Sec.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (Math.Abs(num) >= MathUtility.CotangentLimit)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 / Math.Cos(num));
		}

		public static readonly string FunctionName = "SEC";

		static readonly FunctionInfo Info;
	}
}
