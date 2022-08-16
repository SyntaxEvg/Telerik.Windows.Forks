using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsNA : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsNA.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsNA.Info;
			}
		}

		static IsNA()
		{
			string description = "Returns TRUE if the value is the #N/A error value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsNA_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsNA.Info = new FunctionInfo(IsNA.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression valueAsConstantExpression = context.Arguments[0].GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			bool value = valueAsConstantExpression is ErrorExpression && valueAsConstantExpression == ErrorExpressions.NotAvailableError;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISNA";

		static readonly FunctionInfo Info;
	}
}
