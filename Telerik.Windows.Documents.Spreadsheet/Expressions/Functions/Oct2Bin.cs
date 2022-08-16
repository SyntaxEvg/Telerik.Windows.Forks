using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Oct2Bin : NumbersInFunction
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
				return Oct2Bin.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Oct2Bin.Info;
			}
		}

		static Oct2Bin()
		{
			string description = "Converts an octal number to binary.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Oct2Bin_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The octal number you want to convert. Number may not contain more than 10 characters. The most significant bit of number is the sign bit. The remaining 29 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Oct2Bin_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Places", "The number of characters to use. If places is omitted, OCT2BIN uses the minimum number of characters necessary. Places is useful for padding the return value with leading 0s (zeros).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Places", "Spreadsheet_Functions_Oct2Bin_PlacesInfo")
			};
			Oct2Bin.Info = new FunctionInfo(Oct2Bin.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			string text = context.Arguments[0].ToString();
			if (string.IsNullOrEmpty(text) || text.Length > 10)
			{
				return ErrorExpressions.NumberError;
			}
			bool flag = context.Arguments.Length > 1;
			RadExpression result;
			try
			{
				if (flag)
				{
					long num = context.Arguments[1].Truncate();
					if (num <= 0L || num > 10L)
					{
						result = ErrorExpressions.NumberError;
					}
					else if (EngineeringFunctions.OCT2DEC(text) > 0L && (long)EngineeringFunctions.OCT2BIN(text).Length > num)
					{
						result = ErrorExpressions.NumberError;
					}
					else
					{
						string value = EngineeringFunctions.OCT2BIN(text, (int)num);
						result = new StringExpression(value);
					}
				}
				else
				{
					string value2 = EngineeringFunctions.OCT2BIN(text);
					result = new StringExpression(value2);
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "OCT2BIN";

		static readonly FunctionInfo Info;
	}
}
