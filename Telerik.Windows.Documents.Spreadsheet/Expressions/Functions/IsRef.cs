using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsRef : FunctionBase
	{
		public override string Name
		{
			get
			{
				return IsRef.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsRef.Info;
			}
		}

		static IsRef()
		{
			string description = "Returns TRUE if the value is a reference.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsRef_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the value you want tested. Value can be a blank (empty cell), error, logical, text, number, or reference value, or a name referring to any of these, that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Is_ValueInfo")
			};
			IsRef.Info = new FunctionInfo(IsRef.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			bool value = context.Arguments[0] is CellReferenceRangeExpression;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISREF";

		static readonly FunctionInfo Info;
	}
}
