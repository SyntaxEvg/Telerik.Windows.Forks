using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Exact : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Exact.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Exact.Info;
			}
		}

		static Exact()
		{
			string description = "Compares two text strings and returns TRUE if they are exactly the same, FALSE otherwise. EXACT is case-sensitive but ignores formatting differences. Use EXACT to test text being entered into a document.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Exact_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text1", "is the first text string.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Exact_Text1Info"),
				new ArgumentInfo("Text2", "is the second text string.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Exact_Text2Info")
			};
			Exact.Info = new FunctionInfo(Exact.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = context.Arguments[0];
			string strB = context.Arguments[1];
			bool value = false;
			if (text.CompareTo(strB) == 0)
			{
				value = true;
			}
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "EXACT";

		static readonly FunctionInfo Info;
	}
}
