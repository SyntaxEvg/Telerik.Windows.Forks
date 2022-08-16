using System;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Text
{
	public class TextFormatProvider
	{
		public string Export(RadFixedDocument document, TextFormatProviderSettings settings)
		{
			TextDocument textDocument;
			if (document.TextDocument == null)
			{
				textDocument = new TextDocument(document);
			}
			else
			{
				textDocument = document.TextDocument;
			}
			return textDocument.ToString(settings.LinesSeparator, settings.PagesSeparator);
		}

		public string Export(RadFixedDocument document)
		{
			return this.Export(document, TextFormatProviderSettings.Default);
		}
	}
}
