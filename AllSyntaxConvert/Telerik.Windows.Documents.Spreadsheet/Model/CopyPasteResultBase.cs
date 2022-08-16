using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CopyPasteResultBase
	{
		public bool Success
		{
			get
			{
				return this.success;
			}
		}

		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
		}

		internal string ErrorMessageLocalizationKey
		{
			get
			{
				return this.errorMessageLocalizationKey;
			}
		}

		protected CopyPasteResultBase(bool success, string errorMessage = "", string errorMessageLocalizationKey = "")
		{
			this.success = success;
			this.errorMessage = errorMessage;
			this.errorMessageLocalizationKey = errorMessageLocalizationKey;
		}

		readonly bool success;

		readonly string errorMessage;

		readonly string errorMessageLocalizationKey;
	}
}
