using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsText : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsText.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsText.Info;
			}
		}

		static IsText()
		{
			string description = "Returns TRUE if the value is text.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsText_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsText.Info = new FunctionInfo(IsText.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression radExpression = context.Arguments[0];
			RadExpression valueAsConstantExpression = radExpression.GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool flag = radExpression is StringExpression;
			if (!flag)
			{
				flag = valueAsConstantExpression is StringExpression;
			}
			return flag.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISTEXT";

		static readonly FunctionInfo Info;
	}
}
