using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsNonText : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsNonText.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsNonText.Info;
			}
		}

		static IsNonText()
		{
			string description = "Returns TRUE if the value is not text.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsNonText_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsNonText.Info = new FunctionInfo(IsNonText.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			ConstantExpression valueAsConstantExpression = context.Arguments[0].GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool value = !(valueAsConstantExpression is StringExpression);
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISNONTEXT";

		static readonly FunctionInfo Info;
	}
}
