using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Bin2Dec : NumbersInFunction
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
				return Bin2Dec.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Bin2Dec.Info;
			}
		}

		static Bin2Dec()
		{
			string description = "Converts a binary number to decimal.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Bin2Dec_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The binary number you want to convert. Number cannot contain more than 10 characters (10 bits). The most significant bit of number is the sign bit. The remaining 9 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Bin2Dec_NumberInfo")
			};
			Bin2Dec.Info = new FunctionInfo(Bin2Dec.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			string text = context.Arguments[0].ToString();
			if (string.IsNullOrEmpty(text) || text.Length > 10)
			{
				return ErrorExpressions.NumberError;
			}
			RadExpression result;
			try
			{
				long num = EngineeringFunctions.BIN2DEC(text);
				result = NumberExpression.CreateValidNumberOrErrorExpression((double)num);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "BIN2DEC";

		static readonly FunctionInfo Info;
	}
}
