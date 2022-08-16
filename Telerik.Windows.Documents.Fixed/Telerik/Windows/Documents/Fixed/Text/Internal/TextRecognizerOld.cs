using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text.Internal
{
	class TextRecognizerOld : TextRecognizer
	{
		public TextRecognizerOld(TextDocument parent)
			: base(parent)
		{
		}

		public TextPage CreateTextPage(RadFixedPageInternal page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPageInternal>(page, "page");
			TextPage result = null;
			lock (this.lockObject)
			{
				base.StartNewTextPage(page.PublicPage);
				ContentCollection contentCollection = page.Content ?? page.Document.FormatProvider.ParseOnlyTextRelatedContent(page);
				IEnumerable<GlyphOld> allNonWhiteSpaceGlyphs = contentCollection.GetAllNonWhiteSpaceGlyphs();
				foreach (GlyphOld glyphOld in allNonWhiteSpaceGlyphs)
				{
					Character character = new Character(glyphOld.BoundingRect, glyphOld.ToUnicode, glyphOld.CharSpacing);
					base.ProcessCharacter(character);
				}
				result = base.FinishCurrentTextPage();
			}
			return result;
		}

		readonly object lockObject = new object();
	}
}
