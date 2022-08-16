using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSum : global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.StringsInFunction
	{
		public override global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImSum.argumentConversionRules;
			}
		}

		public override string Name
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImSum.FunctionName;
			}
		}

		public override global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo FunctionInfo
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImSum.Info;
			}
		}

		static ImSum()
		{
			string description = "Returns the sum of one or more complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSum_Info";
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo> requiredArgumentsInfos = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo[]
			{
				new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to sum.", global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSum_InumberInfo")
			};
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo> optionalArgumentsInfos = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo[]
			{
				new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo("Inumber", "Inumber1, Inumber2,... are the complex numbers to sum.", global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSum_InumberInfo")
			};
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImSum.Info = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImSum.FunctionName, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression EvaluateOverride(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionEvaluationContext<string> context)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression result;
			try
			{
				string complexSymbol;
				if (global::Telerik.Windows.Documents.Spreadsheet.Expressions.MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					global::System.Numerics.Complex left = default(global::System.Numerics.Complex);
					for (int i = 0; i < context.Arguments.Length; i++)
					{
						global::System.Numerics.Complex right = context.Arguments[i].ToComplex();
						left = global::System.Numerics.Complex.Add(left, right);
					}
					string value = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.COMPLEX(left.Real, left.Imaginary, complexSymbol);
					result = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.StringExpression(value);
				}
				else
				{
					result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.ValueError;
				}
			}
			catch (global::System.Exception)
			{
				result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMSUM";

		private static readonly global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo Info;

		private static readonly global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentConversionRules argumentConversionRules = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentConversionRules(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.ConvertToDefault, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.TreatAsError, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.UseAsIs, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInterpretation.TreatAsError, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArrayArgumentInterpretation.UseAllElements);
	}
}
