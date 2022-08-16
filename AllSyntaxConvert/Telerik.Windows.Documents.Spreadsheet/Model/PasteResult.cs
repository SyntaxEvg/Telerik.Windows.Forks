using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class PasteResult : CopyPasteResultBase
	{
		internal bool ShouldDisableHistory
		{
			get
			{
				return this.shouldDisableHistory;
			}
		}

		public PasteResult(bool success, string errorMessage = "")
			: this(success, errorMessage, string.Empty, false)
		{
		}

		internal PasteResult(bool success, string errorMessage = "", string errorMessageLocalizationKey = "", bool shouldDisableHistory = false)
			: base(success, errorMessage, errorMessageLocalizationKey)
		{
			this.shouldDisableHistory = shouldDisableHistory;
		}

		readonly bool shouldDisableHistory;
	}
}
