using System;
using Telerik.Windows.Documents.Licensing;

namespace Telerik.Documents.SpreadsheetStreaming.Licensing
{
	static class LicenseCheck
	{
		internal static void Validate(IWorkbookExporter workbookExporter)
		{
			string licenseMessage;
			if (LicenseCheck.TryGetLicenseMessage(out licenseMessage))
			{
				using (IWorksheetExporter worksheetExporter = workbookExporter.CreateWorksheetExporter(LicenseCheck.licenseSheetName))
				{
					LicenseCheck.ExportLicenseMessage(licenseMessage, worksheetExporter);
				}
			}
		}

		internal static void Validate(IWorksheetExporter worksheetExporter)
		{
			string licenseMessage;
			if (LicenseCheck.TryGetLicenseMessage(out licenseMessage))
			{
				LicenseCheck.ExportLicenseMessage(licenseMessage, worksheetExporter);
			}
		}

		static void ExportLicenseMessage(string licenseMessage, IWorksheetExporter worksheetExporter)
		{
			using (IRowExporter rowExporter = worksheetExporter.CreateRowExporter())
			{
				using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
				{
					cellExporter.SetFormat(new SpreadCellFormat
					{
						ForeColor = new SpreadThemableColor(new SpreadColor(byte.MaxValue, 0, 0))
					});
					cellExporter.SetValue(licenseMessage);
				}
			}
		}

		static bool TryGetLicenseMessage(out string licenseMessage)
		{
			switch (TelerikLicense.GetLicensingMode())
			{
			case LicensingMode.Trial:
				licenseMessage = TrialMessages.DocumentProcessingTrialMessage;
				return true;
			case LicensingMode.Unlicensed:
				licenseMessage = TrialMessages.DocumentProcessingUnlicencedMessage;
				return true;
			}
			licenseMessage = null;
			return false;
		}

		static readonly string licenseSheetName = "RadSpreadStreamProcessing";
	}
}
