using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class FactDouble : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return FactDouble.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return FactDouble.Info;
			}
		}

		static FactDouble()
		{
			string description = "Returns the double factorial of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Factdouble_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value for which to return the double factorial. If number is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Factdouble_NumberInfo")
			};
			FactDouble.Info = new FunctionInfo(FactDouble.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			int num = (int)context.Arguments[0];
			if (num < -1 || num > 300)
			{
				return ErrorExpressions.NumberError;
			}
			int num2 = 1;
			int num3 = num % 2 + 2;
			for (int i = num3; i <= num; i += 2)
			{
				num2 *= i;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num2);
		}

		public static readonly string FunctionName = "FACTDOUBLE";

		static readonly FunctionInfo Info;
	}
}
