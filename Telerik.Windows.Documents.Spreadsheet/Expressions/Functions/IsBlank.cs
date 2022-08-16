using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsBlank : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsBlank.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsBlank.Info;
			}
		}

		static IsBlank()
		{
			string description = "Returns the logical value TRUE if the value argument is a reference to an empty cell; otherwise it returns FALSE";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsBlank_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "The value that you want tested. The value argument can be a blank (empty cell), error, logical value, text, number, or reference value, or a name referring to any of these.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_IsBlank_ValueInfo")
			};
			IsBlank.Info = new FunctionInfo(IsBlank.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression radExpression = context.Arguments[0];
			RadExpression valueAsConstantExpression = radExpression.GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool value = radExpression is EmptyExpression || valueAsConstantExpression is EmptyExpression;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISBLANK";

		static readonly FunctionInfo Info;
	}
}
