using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Pi : FunctionBase
	{
		public override string Name
		{
			get
			{
				return Pi.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Pi.Info;
			}
		}

		static Pi()
		{
			string description = "Returns the value of Pi, 3.14159265358979, accurate to 15 digits.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Pi_Info";
			Pi.Info = new FunctionInfo(Pi.FunctionName, FunctionCategory.MathTrig, description, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(3.141592653589793);
		}

		public static readonly string FunctionName = "PI";

		static readonly FunctionInfo Info;
	}
}
