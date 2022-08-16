using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Licensing;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx
{
	class LicenseCheck : IDisposable
	{
		public LicenseCheck(Workbook workbook)
		{
			this.workbook = workbook;
			LicensingMode licensingMode = TelerikLicense.GetLicensingMode();
			if (licensingMode != LicensingMode.Dev)
			{
				this.activeSheetName = workbook.ActiveSheet.Name;
				Worksheet worksheet = workbook.Worksheets.Add();
				worksheet.Name = this.GetAvailableSheetName();
				string value;
				if (licensingMode == LicensingMode.Trial)
				{
					value = TrialMessages.DocumentProcessingTrialMessage;
				}
				else
				{
					value = TrialMessages.DocumentProcessingUnlicencedMessage;
				}
				worksheet.Cells[0, 0].SetValue(value);
				worksheet.Cells[0, 0].SetForeColor(new ThemableColor(Colors.Red));
				workbook.ActiveSheet = worksheet;
			}
		}

		public void Dispose()
		{
			if (!this.isDisposed)
			{
				if (!string.IsNullOrEmpty(this.activeSheetName))
				{
					this.workbook.Worksheets.RemoveAt(this.workbook.Worksheets.Count - 1);
					this.workbook.ActiveSheet = this.workbook.Sheets[this.activeSheetName];
				}
				this.isDisposed = true;
			}
		}

		string GetAvailableSheetName()
		{
			string text = LicenseCheck.licenseSheetName;
			int num = 1;
			while (this.workbook.Sheets.Contains(text))
			{
				text = LicenseCheck.licenseSheetName + num.ToString();
				num++;
			}
			return text;
		}

		static readonly string licenseSheetName = "License";

		readonly Workbook workbook;

		readonly string activeSheetName;

		bool isDisposed;
	}
}
