using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class False : FunctionBase
	{
		public override string Name
		{
			get
			{
				return False.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return False.Info;
			}
		}

		static False()
		{
			string description = "Returns the logical value FALSE.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_False_Info";
			False.Info = new FunctionInfo(False.FunctionName, FunctionCategory.Logical, description, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			return BooleanExpression.False;
		}

		public static readonly string FunctionName = "FALSE";

		static readonly FunctionInfo Info;
	}
}
