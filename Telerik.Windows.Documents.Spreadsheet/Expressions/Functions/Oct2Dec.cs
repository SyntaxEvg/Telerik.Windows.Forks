using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Oct2Dec : NumbersInFunction
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
				return Oct2Dec.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Oct2Dec.Info;
			}
		}

		static Oct2Dec()
		{
			string description = "Converts an octal number to decimal.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Oct2Dec_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The octal number you want to convert. Number may not contain more than 10 octal characters (30 bits). The most significant bit of number is the sign bit. The remaining 29 bits are magnitude bits. Negative numbers are represented using two's-complement notation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Oct2Dec_NumberInfo")
			};
			Oct2Dec.Info = new FunctionInfo(Oct2Dec.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
				long num = EngineeringFunctions.OCT2DEC(text);
				result = NumberExpression.CreateValidNumberOrErrorExpression((double)num);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "OCT2DEC";

		static readonly FunctionInfo Info;
	}
}
