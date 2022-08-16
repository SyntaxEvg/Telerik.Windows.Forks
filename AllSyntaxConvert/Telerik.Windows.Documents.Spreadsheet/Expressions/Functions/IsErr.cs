using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsErr : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsErr.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsErr.Info;
			}
		}

		static IsErr()
		{
			string description = "Returns TRUE if the value is any error value except #N/A.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsErr_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_IsErr_ValueInfo")
			};
			IsErr.Info = new FunctionInfo(IsErr.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression valueAsConstantExpression = context.Arguments[0].GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool value = valueAsConstantExpression is ErrorExpression && valueAsConstantExpression != ErrorExpressions.NotAvailableError;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISERR";

		static readonly FunctionInfo Info;
	}
}
