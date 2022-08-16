using System;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.Text.Internal
{
	class TextDocumentOld : TextDocument
	{
		public TextDocumentOld(RadFixedDocumentInternal document)
		{
			this.document = document;
			this.textRecognizer = new TextRecognizerOld(this);
			base.PagesCount = this.document.Pages.Count;
		}

		protected override TextPage CreateTextPage(int index)
		{
			RadFixedPageInternal page = this.document.Pages[index];
			return this.textRecognizer.CreateTextPage(page);
		}

		readonly RadFixedDocumentInternal document;

		readonly TextRecognizerOld textRecognizer;
	}
}
