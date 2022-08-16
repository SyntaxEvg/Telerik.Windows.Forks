using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSum : StringsInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ImSum.argumentConversionRules;
			}
		}

		public override string Name
		{
			get
			{
				return ImSum.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImSum.Info;
			}
		}

		static ImSum()
		{
			string description = "Returns the sum of one or more complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSum_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to sum.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSum_InumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to sum.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSum_InumberInfo")
			};
			ImSum.Info = new FunctionInfo(ImSum.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string complexSymbol;
				if (MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					Complex left = default(Complex);
					for (int i = 0; i < context.Arguments.Length; i++)
					{
						Complex right = context.Arguments[i].ToComplex();
						left = Complex.Add(left, right);
					}
					string value = EngineeringFunctions.COMPLEX(left.Real, left.Imaginary, complexSymbol);
					result = new StringExpression(value);
				}
				else
				{
					result = ErrorExpressions.ValueError;
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMSUM";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules argumentConversionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseAllElements);
	}
}
