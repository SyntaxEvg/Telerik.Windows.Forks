using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IfFunction : FunctionBase
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return IfFunction.ConvertionRules;
			}
		}

		public override string Name
		{
			get
			{
				return IfFunction.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IfFunction.Info;
			}
		}

		static IfFunction()
		{
			ArgumentInterpretation emptyDirectArgument = ArgumentInterpretation.ConvertToDefault;
			ArgumentInterpretation emptyIndirectArgument = ArgumentInterpretation.ConvertToDefault;
			IfFunction.ConvertionRules = new ArgumentConversionRules(emptyDirectArgument, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, emptyIndirectArgument, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.TreatAsError, ArrayArgumentInterpretation.UseFirstElement);
			string description = "The IF function returns one value if a condition you specify evaluates to TRUE, and another value if that condition evaluates to FALSE. For example, the formula =IF(A1>10,\"Over 10\",\"10 or less\") returns \"Over 10\" if A1 is greater than 10, and \"10 or less\" if A1 is less than or equal to 10.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_If_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical test", "Any value or expression that can be evaluated to TRUE or FALSE. For example, A10=100 is a logical expression; if the value in cell A10 is equal to 100, the expression evaluates to TRUE. Otherwise, the expression evaluates to FALSE. This argument can use any comparison calculation operator.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_LogicalTest", "Spreadsheet_Functions_If_LogicalTestInfo"),
				new ArgumentInfo("Value if true", "The value that you want to be returned if the logical test argument evaluates to TRUE. For example, if the value of this argument is the text string \"Within budget\" and the logical test argument evaluates to TRUE, the IF function returns the text \"Within budget.\" If logical test evaluates to TRUE and the value if true argument is omitted (that is, there is only a comma following the logical test argument), the IF function returns 0 (zero). To display the word TRUE, use the logical value TRUE for the value if true argument.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_ValueIfTrue", "Spreadsheet_Functions_If_ValueIfTrueInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value if false", "The value that you want to be returned if the logical test argument evaluates to FALSE. For example, if the value of this argument is the text string \"Over budget\" and the logical test argument evaluates to FALSE, the IF function returns the text \"Over budget.\" If logical test evaluates to FALSE and the value if false argument is omitted, (that is, there is no comma following the value if true argument), the IF function returns the logical value FALSE. If logical test evaluates to FALSE and the value of the value if false argument is blank (that is, there is only a comma following the value if true argument), the IF function returns the value 0 (zero).", ArgumentType.Any, true, "Spreadsheet_Functions_Args_ValueIfFalse", "Spreadsheet_Functions_If_ValueIfFalseInfo")
			};
			IfFunction.Info = new FunctionInfo(IfFunction.FunctionName, FunctionCategory.Logical, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			object obj;
			ErrorExpression errorExpression = FunctionHelper.TryConvertFirstArgument(context.Arguments[0], this.ArgumentConversionRules, ArgumentType.Logical, out obj);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			RadExpression radExpression = context.Arguments[1];
			RadExpression radExpression2 = BooleanExpression.False;
			if (context.Arguments.Length > 2)
			{
				radExpression2 = context.Arguments[2];
			}
			RadExpression radExpression3 = (((bool)obj) ? radExpression : radExpression2).GetValueAsConstantExpression();
			if (radExpression3 is EmptyExpression)
			{
				radExpression3 = NumberExpression.Zero;
			}
			return radExpression3;
		}

		public static readonly string FunctionName = "IF";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules ConvertionRules;
	}
}
