using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Hex2Dec : StringsInFunction
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
				return Hex2Dec.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Hex2Dec.Info;
			}
		}

		static Hex2Dec()
		{
			string description = "Converts a hexadecimal number to decimal.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Hex2Dec_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The hexadecimal number you want to convert. Number cannot contain more than 10 characters (40 bits). The most significant bit of number is the sign bit. The remaining 39 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Hex2Dec_NumberInfo")
			};
			Hex2Dec.Info = new FunctionInfo(Hex2Dec.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			RadExpression result;
			try
			{
				double number = (double)EngineeringFunctions.HEX2DEC(text);
				result = NumberExpression.CreateValidNumberOrErrorExpression(number);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "HEX2DEC";

		static readonly FunctionInfo Info;
	}
}
