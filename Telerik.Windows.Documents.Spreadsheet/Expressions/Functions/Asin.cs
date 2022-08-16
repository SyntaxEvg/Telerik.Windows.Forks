using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Asin : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Asin.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Asin.Info;
			}
		}

		static Asin()
		{
			string description = "Returns the arcsine, or inverse sine, of a number. The arcsine is the angle whose sine is number. The returned angle is given in radians in the range -pi/2 to pi/2.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Asin_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the sine of the angle you want and must be from -1 to 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Asin_NumberInfo")
			};
			Asin.Info = new FunctionInfo(Asin.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num > 1.0 || num < -1.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(Math.Asin(num));
		}

		public static readonly string FunctionName = "ASIN";

		static readonly FunctionInfo Info;
	}
}
