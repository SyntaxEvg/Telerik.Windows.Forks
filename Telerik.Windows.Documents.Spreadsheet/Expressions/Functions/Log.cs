using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Log : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Log.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Log.Info;
			}
		}

		static Log()
		{
			string description = "Returns the logarithm of a number to the base you specify.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Log_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the positive real number for which you want the logarithm.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Log_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Base", "is the positive real number for which you want the logarithm.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Base", "Spreadsheet_Functions_Log_BaseInfo")
			};
			Log.Info = new FunctionInfo(Log.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num <= 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			if (context.Arguments.Length == 1)
			{
				number = Math.Log(num, 10.0);
			}
			else
			{
				double num2 = context.Arguments[1];
				if (num2 <= 0.0)
				{
					return ErrorExpressions.NumberError;
				}
				if (num2 == 1.0)
				{
					return ErrorExpressions.DivisionByZero;
				}
				number = Math.Log(num, num2);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "LOG";

		static readonly FunctionInfo Info;
	}
}
