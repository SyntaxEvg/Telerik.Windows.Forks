using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Dec2Hex : NumbersInFunction
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
				return Dec2Hex.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Dec2Hex.Info;
			}
		}

		static Dec2Hex()
		{
			string description = "Converts a decimal number to hexadecimal.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Dec2Hex_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The decimal integer you want to convert. If number is negative, places is ignored and DEC2HEX returns a 10-character (40-bit) hexadecimal number in which the most significant bit is the sign bit. The remaining 39 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Dec2Hex_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Places", "The number of characters to use. If places is omitted, DEC2HEX uses the minimum number of characters necessary. Places is useful for padding the return value with leading 0s (zeros).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Places", "Spreadsheet_Functions_Dec2Hex_PlacesInfo")
			};
			Dec2Hex.Info = new FunctionInfo(Dec2Hex.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < -549755813888.0 || num > 549755813887.0)
			{
				return ErrorExpressions.NumberError;
			}
			bool flag = context.Arguments.Length > 1;
			if (!flag)
			{
				string value = EngineeringFunctions.DEC2HEX(num);
				return new StringExpression(value);
			}
			long num2 = context.Arguments[1].Truncate();
			if (num2 <= 0L || num2 > 10L)
			{
				return ErrorExpressions.NumberError;
			}
			if (num > 0.0 && (long)EngineeringFunctions.DEC2HEX(num).Length > num2)
			{
				return ErrorExpressions.NumberError;
			}
			string value2 = EngineeringFunctions.DEC2HEX(num, (int)num2);
			return new StringExpression(value2);
		}

		public static readonly string FunctionName = "DEC2HEX";

		static readonly FunctionInfo Info;
	}
}
