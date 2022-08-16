using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImProduct : StringsInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ImProduct.argumentConversionRules;
			}
		}

		public override string Name
		{
			get
			{
				return ImProduct.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImProduct.Info;
			}
		}

		static ImProduct()
		{
			string description = "Returns the product of 1 to 255 complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImProduct_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to multiply.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImProduct_InumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to multiply.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImProduct_InumberInfo")
			};
			ImProduct.Info = new FunctionInfo(ImProduct.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string complexSymbol;
				if (MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					global::System.Numerics.Complex left = new global::System.Numerics.Complex(1.0, 0.0);
					for (int i = 0; i < context.Arguments.Length; i++)
					{
						global::System.Numerics.Complex right = context.Arguments[i].ToComplex();
						left = global::System.Numerics.Complex.Multiply(left, right);
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

		public static readonly string FunctionName = "IMPRODUCT";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules argumentConversionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseAllElements);
	}
}
