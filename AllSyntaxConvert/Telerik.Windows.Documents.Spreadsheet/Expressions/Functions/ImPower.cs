using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImPower : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return ImPower.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImPower.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ImPower.conversionRules;
			}
		}

		static ImPower()
		{
			ArgumentInterpretation nonTextNumberDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation nonTextNumberIndirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberIndirectArgument = ArgumentInterpretation.TreatAsError;
			ImPower.conversionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, textNumberDirectArgument, nonTextNumberDirectArgument, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, textNumberIndirectArgument, nonTextNumberIndirectArgument, ArrayArgumentInterpretation.UseFirstElement);
			string description = "Returns a complex number in x + yi or x + yj text format raised to a power.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImPower_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number you want to raise to a power.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImPower_InumberInfo"),
				new ArgumentInfo("Number", "The power to which you want to raise the complex number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_ImPower_NumberInfo")
			};
			ImPower.Info = new FunctionInfo(ImPower.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			RadExpression result;
			try
			{
				string text = (string)context.Arguments[0];
				double power = (double)context.Arguments[1];
				if (EngineeringFunctions.IMABS(text) == 0.0)
				{
					result = ErrorExpressions.NumberError;
				}
				else
				{
					string imaginarySymbol = MathUtility.GetImaginarySymbol(text);
					global::System.Numerics.Complex complex = EngineeringFunctions.IMPOWER(text, power);
					string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
					result = new StringExpression(value);
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMPOWER";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules conversionRules;
	}
}
