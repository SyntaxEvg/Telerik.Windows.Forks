using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Cot : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Cot.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Cot.Info;
			}
		}

		static Cot()
		{
			string description = "Return the cotangent of an angle specified in radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Cot_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the cotangent.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Cot_NumberInfo")
			};
			Cot.Info = new FunctionInfo(Cot.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 / Math.Tan(num));
		}

		public static readonly string FunctionName = "COT";

		static readonly FunctionInfo Info;
	}
}
