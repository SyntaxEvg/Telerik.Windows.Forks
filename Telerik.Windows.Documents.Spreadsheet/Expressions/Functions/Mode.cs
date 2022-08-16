using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Mode : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return Mode.ConvertionRules;
			}
		}

		public override string Name
		{
			get
			{
				return Mode.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Mode.Info;
			}
		}

		static Mode()
		{
			ArgumentInterpretation emptyIndirectArgument = ArgumentInterpretation.Ignore;
			ArgumentInterpretation emptyDirectArgument = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation numberDirectArgument = ArgumentInterpretation.UseAsIs;
			ArgumentInterpretation boolIndirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation boolDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberDirectArgument = ArgumentInterpretation.TreatAsError;
			ArgumentInterpretation textNumberIndirectArgument = ArgumentInterpretation.TreatAsError;
			Mode.ConvertionRules = new ArgumentConversionRules(emptyDirectArgument, numberDirectArgument, boolDirectArgument, textNumberDirectArgument, ArgumentInterpretation.TreatAsError, emptyIndirectArgument, ArgumentInterpretation.UseAsIs, boolIndirectArgument, textNumberIndirectArgument, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseFirstElement);
			string description = "Returns the most frequently occurring, or repetitive, value in an array or range of data.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Mode_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 arguments for which you want to calculate the mode. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Mode_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 arguments for which you want to calculate the mode. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Mode_NumberInfo")
			};
			Mode.Info = new FunctionInfo(Mode.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			Dictionary<double, int> dictionary = new Dictionary<double, int>();
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (dictionary.ContainsKey(context.Arguments[i]))
				{
					Dictionary<double, int> dictionary2;
					double key;
					(dictionary2 = dictionary)[key = context.Arguments[i]] = dictionary2[key] + 1;
				}
				else
				{
					dictionary.Add(context.Arguments[i], 1);
				}
			}
			double? num = null;
			int num2 = 0;
			foreach (KeyValuePair<double, int> keyValuePair in dictionary)
			{
				if (num == null || keyValuePair.Value > num2)
				{
					num = new double?(keyValuePair.Key);
					num2 = keyValuePair.Value;
				}
			}
			if (num2 == 1)
			{
				return ErrorExpressions.NotAvailableError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num.Value);
		}

		public static readonly string FunctionName = "MODE";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules ConvertionRules;
	}
}
