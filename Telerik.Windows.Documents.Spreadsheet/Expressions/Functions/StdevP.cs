﻿using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class StdevP : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NaryIgnoreIndirectNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return StdevP.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return StdevP.Info;
			}
		}

		static StdevP()
		{
			string description = "Calculates standard deviation based on the entire population given as arguments. The standard deviation is a measure of how widely values are dispersed from the average value (the mean).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_StdevP_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 number arguments corresponding to a population. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_StdevP_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 number arguments corresponding to a population. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_StdevP_NumberInfo")
			};
			StdevP.Info = new FunctionInfo(StdevP.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			if (context.Arguments.Length < 2)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = MathUtility.StandardDeviation(context.Arguments, true);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "STDEVP";

		static readonly FunctionInfo Info;
	}
}
