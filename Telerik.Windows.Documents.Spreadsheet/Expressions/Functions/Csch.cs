using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Csch : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Csch.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Csch.Info;
			}
		}

		static Csch()
		{
			string description = "Return the hyperbolic cosecant of an angle specified in radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Csch_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the hyperbolic cosecant.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Csch_NumberInfo")
			};
			Csch.Info = new FunctionInfo(Csch.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (Math.Abs(num) >= MathUtility.CotangentLimit)
			{
				return ErrorExpressions.NumberError;
			}
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 / Math.Sinh(num));
		}

		public static readonly string FunctionName = "CSCH";

		static readonly FunctionInfo Info;
	}
}
