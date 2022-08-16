using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Convert : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Convert.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Convert.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return Convert.conversionRules;
			}
		}

		static Convert()
		{
			ArgumentInterpretation nonTextNumberDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation nonTextNumberIndirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberIndirectArgument = ArgumentInterpretation.TreatAsError;
			Convert.conversionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, textNumberDirectArgument, nonTextNumberDirectArgument, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, textNumberIndirectArgument, nonTextNumberIndirectArgument, ArrayArgumentInterpretation.UseFirstElement);
			string description = "Converts a number from one measurement system to another. For example, CONVERT can translate a table of distances in miles to a table of distances in kilometers.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Convert_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "A complex number you want to raise to a power.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Convert_NumberInfo"),
				new ArgumentInfo("From_unit", "The units for the Number argument.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_From_unit", "Spreadsheet_Functions_Convert_From_unitInfo"),
				new ArgumentInfo("To_unit", "The units for the CONVERT function's result.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_To_unit", "Spreadsheet_Functions_Convert_To_unitInfo")
			};
			Convert.Info = new FunctionInfo(Convert.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			RadExpression result;
			try
			{
				double number = (double)context.Arguments[0];
				string from_unit = (string)context.Arguments[1];
				string to_unit = (string)context.Arguments[2];
				double number2 = EngineeringFunctions.CONVERT(number, from_unit, to_unit);
				result = NumberExpression.CreateValidNumberOrErrorExpression(number2);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NotAvailableError;
			}
			return result;
		}

		public static readonly string FunctionName = "CONVERT";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules conversionRules;
	}
}
