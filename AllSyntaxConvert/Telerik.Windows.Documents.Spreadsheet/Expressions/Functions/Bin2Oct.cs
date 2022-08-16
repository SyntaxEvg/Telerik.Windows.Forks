﻿using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Bin2Oct : NumbersInFunction
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
				return Bin2Oct.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Bin2Oct.Info;
			}
		}

		static Bin2Oct()
		{
			string description = "Converts a binary number to octal.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Bin2Oct_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The binary number you want to convert. Number cannot contain more than 10 characters (10 bits). The most significant bit of number is the sign bit. The remaining 9 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Bin2Oct_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Places", "The number of characters to use. If places is omitted, BIN2OCT uses the minimum number of characters necessary. Places is useful for padding the return value with leading 0s (zeros).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Places", "Spreadsheet_Functions_Bin2Oct_PlacesInfo")
			};
			Bin2Oct.Info = new FunctionInfo(Bin2Oct.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
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
					else if (EngineeringFunctions.BIN2DEC(text) > 0L && (long)EngineeringFunctions.BIN2OCT(text).Length > num)
					{
						result = ErrorExpressions.NumberError;
					}
					else
					{
						string value = EngineeringFunctions.BIN2OCT(text, (int)num);
						result = new StringExpression(value);
					}
				}
				else
				{
					string value2 = EngineeringFunctions.BIN2OCT(text);
					result = new StringExpression(value2);
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "BIN2OCT";

		static readonly FunctionInfo Info;
	}
}
