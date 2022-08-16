using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sech : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sech.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sech.Info;
			}
		}

		static Sech()
		{
			string description = "Returns the hyperbolic secant of an angle specified in radians.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sech_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the hyperbolic secant.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sech_NumberInfo")
			};
			Sech.Info = new FunctionInfo(Sech.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double value = context.Arguments[0];
			if (Math.Abs(value) >= MathUtility.CotangentLimit)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 / Math.Cosh(value));
		}

		public static readonly string FunctionName = "SECH";

		static readonly FunctionInfo Info;
	}
}
