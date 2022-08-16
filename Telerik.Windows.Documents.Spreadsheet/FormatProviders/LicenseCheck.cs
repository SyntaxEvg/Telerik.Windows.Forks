using System;
using System.Linq;
using Telerik.Windows.Documents.Licensing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	static class LicenseCheck
	{
		public static void Validate(CsvWriter writer)
		{
			string text;
			switch (TelerikLicense.GetLicensingMode())
			{
			case LicensingMode.Trial:
				text = TrialMessages.DocumentProcessingTrialMessage;
				goto IL_2E;
			case LicensingMode.Unlicensed:
				text = TrialMessages.DocumentProcessingUnlicencedMessage;
				goto IL_2E;
			}
			text = null;
			IL_2E:
			if (text != null)
			{
				writer.WriteRecord(Enumerable.Repeat<string>(text, 1));
			}
		}
	}
}
