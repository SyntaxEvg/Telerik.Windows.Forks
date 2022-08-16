using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsLogical : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsLogical.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsLogical.Info;
			}
		}

		static IsLogical()
		{
			string description = "Returns TRUE if the value is a logical value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsLogical_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsLogical.Info = new FunctionInfo(IsLogical.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			RadExpression expression = context.Arguments[0];
			bool value = expression.GetValueAsConstantExpression() is BooleanExpression;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISLOGICAL";

		static readonly FunctionInfo Info;
	}
}
