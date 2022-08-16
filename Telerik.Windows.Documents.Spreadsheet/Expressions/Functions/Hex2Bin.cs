using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Hex2Bin : StringsInFunction
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
				return Hex2Bin.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Hex2Bin.Info;
			}
		}

		static Hex2Bin()
		{
			string description = "Converts a hexadecimal number to binary.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Hex2Bin_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The hexadecimal number you want to convert. Number cannot contain more than 10 characters. The most significant bit of number is the sign bit (40th bit from the right). The remaining 9 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Hex2Bin_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Places", "The number of characters to use. If places is omitted, HEX2BIN uses the minimum number of characters necessary. Places is useful for padding the return value with leading 0s (zeros).", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Places", "Spreadsheet_Functions_Hex2Bin_PlacesInfo")
			};
			Hex2Bin.Info = new FunctionInfo(Hex2Bin.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = context.Arguments[0];
			if (text.Length > 10)
			{
				return ErrorExpressions.NumberError;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "0";
			}
			bool flag = context.Arguments.Length > 1;
			RadExpression result;
			try
			{
				if (flag)
				{
					long num = double.Parse(context.Arguments[1]).Truncate();
					if (num <= 0L || num > 10L)
					{
						result = ErrorExpressions.NumberError;
					}
					else if (EngineeringFunctions.HEX2DEC(text) > 0L && (long)EngineeringFunctions.HEX2BIN(text).Length > num)
					{
						result = ErrorExpressions.NumberError;
					}
					else
					{
						string value = EngineeringFunctions.HEX2BIN(text, (int)num);
						result = new StringExpression(value);
					}
				}
				else
				{
					string value2 = EngineeringFunctions.HEX2BIN(text);
					result = new StringExpression(value2);
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "HEX2BIN";

		static readonly FunctionInfo Info;
	}
}
