using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Not : BooleansInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return Not.SingleArgumentBoolFunctionConvertion;
			}
		}

		public override string Name
		{
			get
			{
				return Not.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Not.Info;
			}
		}

		static Not()
		{
			ArgumentInterpretation emptyDirectArgument = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation textNumberIndirectArgument = ArgumentInterpretation.Ignore;
			Not.SingleArgumentBoolFunctionConvertion = new ArgumentConversionRules(emptyDirectArgument, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.Ignore, ArgumentInterpretation.Ignore, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, textNumberIndirectArgument, ArgumentInterpretation.Ignore, ArrayArgumentInterpretation.UseFirstElement);
			string description = "Reverses the value of its argument. Use NOT when you want to make sure a value is not equal to one particular value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Not_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "is a value or expression that can be evaluated to TRUE or FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_Not_LogicalInfo")
			};
			Not.Info = new FunctionInfo(Not.FunctionName, FunctionCategory.Logical, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<bool> context)
		{
			if (!context.Arguments[0])
			{
				return BooleanExpression.True;
			}
			return BooleanExpression.False;
		}

		public static readonly string FunctionName = "NOT";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules SingleArgumentBoolFunctionConvertion;
	}
}
