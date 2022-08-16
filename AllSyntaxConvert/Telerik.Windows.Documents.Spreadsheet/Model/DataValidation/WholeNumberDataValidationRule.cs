using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public sealed class WholeNumberDataValidationRule : NumberDataValidationRuleBase
	{
		protected override bool RequireWholeNumbers
		{
			get
			{
				return true;
			}
		}

		public WholeNumberDataValidationRule(NumberDataValidationRuleContext context)
			: base(context)
		{
		}
	}
}
