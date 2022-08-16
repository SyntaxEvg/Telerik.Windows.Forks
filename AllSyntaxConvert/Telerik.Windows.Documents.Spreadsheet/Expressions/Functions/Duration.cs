﻿using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Duration : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return Duration.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Duration.Info;
			}
		}

		static Duration()
		{
			string description = "Returns the Macauley duration for an assumed par value of $100. Duration is defined as the weighted average of the present value of the cash flows and is used as a measure of a bond price's response to changes in yield.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Duration_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_Duration_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the security's maturity date. The maturity date is the date when the security expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_Duration_MaturityInfo"),
				new ArgumentInfo("Coupon", "is the security's annual coupon rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Coupon", "Spreadsheet_Functions_Duration_CouponInfo"),
				new ArgumentInfo("Yld", "is the security's annual yield.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Yld", "Spreadsheet_Functions_Duration_YldInfo"),
				new ArgumentInfo("Frequency", "is the number of coupon payments per year. For annual payments, frequency = 1; for semiannual, frequency = 2; for quarterly, frequency = 4.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Frequency", "Spreadsheet_Functions_Duration_FrequencyInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_Duration_BasisInfo")
			};
			Duration.Info = new FunctionInfo(Duration.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			double num = context.Arguments[2];
			double num2 = context.Arguments[3];
			int num3 = (int)context.Arguments[4];
			int num4 = 0;
			if (context.Arguments.Length == 6)
			{
				num4 = (int)context.Arguments[5];
			}
			if (dateTime == null || dateTime2 == null || (dateTime >= dateTime2 || num < 0.0 || num2 < 0.0 || (num3 != 1 && num3 != 2 && num3 != 4)) || num4 < 0 || num4 > 4)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.DURATION(dateTime.Value, dateTime2.Value, num, num2, num3, num4);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DURATION";

		static readonly FunctionInfo Info;
	}
}
