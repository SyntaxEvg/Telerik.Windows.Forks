using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class CopyResult : CopyPasteResultBase
	{
		public WorksheetFragment Fragment
		{
			get
			{
				return this.fragment;
			}
		}

		public CopyResult(bool success, WorksheetFragment fragment = null, string errorMessage = "", string errorMessageLocalizationKey = "")
			: base(success, errorMessage, errorMessageLocalizationKey)
		{
			this.fragment = fragment;
		}

		readonly WorksheetFragment fragment;
	}
}
