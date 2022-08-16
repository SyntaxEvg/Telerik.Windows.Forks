using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Time : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Time.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Time.Info;
			}
		}

		static Time()
		{
			string description = "Returns the decimal number for a particular time. If the cell format was General before the function was entered, the result is formatted as a date.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Time_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Hour", "is a number from 0 (zero) to 32767 representing the hour. Any value greater than 23 will be divided by 24 and the remainder will be treated as the hour value. For example, TIME(27,0,0) = TIME(3,0,0) = .125 or 3:00 AM.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Hour", "Spreadsheet_Functions_Time_HourInfo"),
				new ArgumentInfo("Minute", "is a number from 0 to 32767 representing the minute. Any value greater than 59 will be converted to hours and minutes. For example, TIME(0,750,0) = TIME(12,30,0) = .520833 or 12:30 PM.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Minute", "Spreadsheet_Functions_Time_MinuteInfo"),
				new ArgumentInfo("Second", "is a number from 0 to 32767 representing the second. Any value greater than 59 will be converted to hours, minutes, and seconds. For example, TIME(0,0,2000) = TIME(0,33,22) = .023148 or 12:33:20 AM", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Second", "Spreadsheet_Functions_Time_SecondInfo")
			};
			Time.Info = new FunctionInfo(Time.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number;
			if (MathUtility.GetTimeSerialNumber(context.Arguments[0], context.Arguments[1], context.Arguments[2], out number))
			{
				return NumberExpression.CreateValidNumberOrErrorExpression(number);
			}
			return ErrorExpressions.NumberError;
		}

		public static readonly string FunctionName = "TIME";

		static readonly FunctionInfo Info;
	}
}
