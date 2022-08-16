using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public abstract class DataValidationRuleContextBase
	{
		public bool ShowInputMessage { get; set; }

		public string InputMessageTitle { get; set; }

		public string InputMessageContent { get; set; }

		public bool ShowErrorMessage { get; set; }

		public ErrorStyle ErrorStyle { get; set; }

		public string ErrorAlertTitle { get; set; }

		public string ErrorAlertContent { get; set; }

		protected DataValidationRuleContextBase()
		{
			this.ShowInputMessage = true;
			this.InputMessageTitle = string.Empty;
			this.InputMessageContent = string.Empty;
			this.ShowErrorMessage = true;
			this.ErrorStyle = ErrorStyle.Stop;
			this.ErrorAlertTitle = string.Empty;
			this.ErrorAlertContent = string.Empty;
		}
	}
}
