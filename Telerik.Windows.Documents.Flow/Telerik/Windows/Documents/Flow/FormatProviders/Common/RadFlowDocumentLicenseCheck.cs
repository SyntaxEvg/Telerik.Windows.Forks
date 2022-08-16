using System;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Licensing;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Common
{
	class RadFlowDocumentLicenseCheck : IDisposable
	{
		public RadFlowDocumentLicenseCheck(RadFlowDocument document)
		{
			this.document = document;
			string text;
			switch (TelerikLicense.GetLicensingMode())
			{
			case LicensingMode.Trial:
				text = TrialMessages.DocumentProcessingTrialMessage;
				goto IL_3B;
			case LicensingMode.Unlicensed:
				text = TrialMessages.DocumentProcessingUnlicencedMessage;
				goto IL_3B;
			}
			text = null;
			IL_3B:
			if (text != null)
			{
				if (!this.document.Sections.Any<Section>())
				{
					this.document.Sections.AddSection();
					this.hasAddedSection = true;
				}
				Paragraph paragraph = new Paragraph(this.document);
				paragraph.Inlines.AddRun(text).ForegroundColor = new ThemableColor(Colors.Red);
				this.document.Sections[0].Blocks.Insert(0, paragraph);
				this.hasAddedParagraph = true;
			}
		}

		public void Dispose()
		{
			if (!this.isDisposed)
			{
				if (this.hasAddedSection)
				{
					this.document.Sections.RemoveAt(0);
				}
				else if (this.hasAddedParagraph)
				{
					this.document.Sections[0].Blocks.RemoveAt(0);
				}
				this.isDisposed = true;
			}
		}

		readonly RadFlowDocument document;

		readonly bool hasAddedSection;

		readonly bool hasAddedParagraph;

		bool isDisposed;
	}
}
