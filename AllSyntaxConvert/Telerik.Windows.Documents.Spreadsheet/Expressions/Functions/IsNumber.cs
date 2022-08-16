using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsNumber : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsNumber.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsNumber.Info;
			}
		}

		static IsNumber()
		{
			string description = "Returns TRUE if the value is a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsNumber_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsNumber.Info = new FunctionInfo(IsNumber.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression valueAsConstantExpression = context.Arguments[0].GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool value = valueAsConstantExpression is NumberExpression;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISNUMBER";

		static readonly FunctionInfo Info;
	}
}
