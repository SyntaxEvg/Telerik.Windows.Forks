using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class True : FunctionBase
	{
		public override string Name
		{
			get
			{
				return True.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return True.Info;
			}
		}

		static True()
		{
			string description = "Returns the logical value TRUE.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_True_Info";
			True.Info = new FunctionInfo(True.FunctionName, FunctionCategory.Logical, description, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			return BooleanExpression.True;
		}

		public static readonly string FunctionName = "TRUE";

		static readonly FunctionInfo Info;
	}
}
