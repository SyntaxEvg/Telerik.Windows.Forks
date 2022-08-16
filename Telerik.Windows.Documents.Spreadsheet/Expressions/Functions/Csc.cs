using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Csc : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Csc.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Csc.Info;
			}
		}

		static Csc()
		{
			string description = "Returns the secant of an angle specified in radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Csc_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the secant.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Csc_NumberInfo")
			};
			Csc.Info = new FunctionInfo(Csc.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((1.0 / Math.Sin(num)).DoubleWithPrecision());
		}

		public static readonly string FunctionName = "CSC";

		static readonly FunctionInfo Info;
	}
}
