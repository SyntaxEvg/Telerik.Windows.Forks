using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class N : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return N.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return N.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return N.argumentConvertionRules;
			}
		}

		static N()
		{
			string description = "Returns a value converted to a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_N_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "The value you want converted. N converts values listed in the following table.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_N_ValueInfo")
			};
			N.Info = new FunctionInfo(N.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(context.Arguments[0]);
		}

		public static readonly string FunctionName = "N";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules argumentConvertionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.ConvertToDefault, ArrayArgumentInterpretation.UseFirstElement);
	}
}
