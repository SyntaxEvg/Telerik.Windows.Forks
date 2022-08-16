using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Licensing;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class RadFixedDocumentLicenseCheck : IDisposable
	{
		public RadFixedDocumentLicenseCheck(RadFixedDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocument>(document, "document");
			this.document = document;
			string message;
			if (RadFixedDocumentLicenseCheck.TryGetLicenseMessage(out message))
			{
				RadFixedPage radFixedPage = this.document.Pages.FirstOrDefault<RadFixedPage>();
				if (radFixedPage != null)
				{
					int count = radFixedPage.Content.Count;
					RadFixedDocumentLicenseCheck.AddLicenceMessageContent(radFixedPage, message);
					this.additionalFragmentsCount = radFixedPage.Content.Count - count;
				}
			}
		}

		public static bool TryGetLicenseMessage(out string message)
		{
			switch (TelerikLicense.GetLicensingMode())
			{
			case LicensingMode.Trial:
				message = TrialMessages.DocumentProcessingTrialMessage;
				goto IL_31;
			case LicensingMode.Unlicensed:
				message = TrialMessages.DocumentProcessingUnlicencedMessage;
				goto IL_31;
			}
			message = null;
			IL_31:
			return !string.IsNullOrEmpty(message);
		}

		public static void AddLicenceMessageContent(RadFixedPage page, string message)
		{
			FixedContentEditor fixedContentEditor = new FixedContentEditor(page);
			fixedContentEditor.TextProperties.FontSize = 8.0;
			fixedContentEditor.GraphicProperties.FillColor = new RgbColor
			{
				R = byte.MaxValue
			};
			fixedContentEditor.Position.Translate(2.0, 2.0);
			fixedContentEditor.DrawText(message);
		}

		public void Dispose()
		{
			if (!this.isDisposed)
			{
				if (this.additionalFragmentsCount > 0)
				{
					RadFixedPage radFixedPage = this.document.Pages.FirstOrDefault<RadFixedPage>();
					if (radFixedPage != null)
					{
						while (this.additionalFragmentsCount > 0)
						{
							ContentElementBase contentElementBase = radFixedPage.Content.LastOrDefault<ContentElementBase>();
							if (contentElementBase == null)
							{
								break;
							}
							radFixedPage.Content.Remove(contentElementBase);
							this.additionalFragmentsCount--;
						}
					}
				}
				this.isDisposed = true;
			}
		}

		readonly RadFixedDocument document;

		int additionalFragmentsCount;

		bool isDisposed;
	}
}
