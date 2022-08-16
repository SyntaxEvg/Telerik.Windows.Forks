using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "CIDFontType0")]
	class CIDFontType0Old : CIDFontOld
	{
		public CIDFontType0Old(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override FontSource FontSource
		{
			get
			{
				return this.CFFFontSource;
			}
		}

		CFFFontSource CFFFontSource
		{
			get
			{
				if (this.ccfFontSource == null)
				{
					this.ccfFontSource = base.FontDescriptor.GetCFFFontSource();
				}
				return this.ccfFontSource;
			}
		}

		public override IEnumerable<GlyphOld> GetGlyphs(IEnumerable<CharCodeOld> charIds)
		{
			if (this.CFFFontSource != null)
			{
				foreach (CharCodeOld b in charIds)
				{
					GlyphOld g = new GlyphOld();
					g.CharId = b;
					Glyph g2 = g;
					CFFFontSource cfffontSource = this.CFFFontSource;
					CharCodeOld charCodeOld = b;
					CidType0Font.InitializeCoreGlyph(g2, cfffontSource, charCodeOld.IntValue);
					yield return g;
				}
			}
			yield break;
		}

		CFFFontSource ccfFontSource;
	}
}
