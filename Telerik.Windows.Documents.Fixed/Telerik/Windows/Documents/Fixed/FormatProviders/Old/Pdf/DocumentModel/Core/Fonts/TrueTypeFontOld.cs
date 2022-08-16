using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "TrueType")]
	class TrueTypeFontOld : SimpleFontOld
	{
		public TrueTypeFontOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		protected override FontSource FontSource
		{
			get
			{
				this.EnsureGlyphInitializationInfo();
				return this.initializationInfo.FontSource;
			}
		}

		protected override IEnumerable<GlyphOld> GetGlyphs(PdfStringOld str)
		{
			this.EnsureGlyphInitializationInfo();
			if (this.initializationInfo.Platform == TrueTypeGlyphInitializationInfo.PlatformType.Unknown)
			{
				return base.GetGlyphs(str);
			}
			GlyphOld[] array = new GlyphOld[str.Value.Length];
			for (int i = 0; i < array.Length; i++)
			{
				GlyphOld glyphOld = new GlyphOld();
				byte b = str.Value[i];
				glyphOld.CharId = new CharCodeOld(b);
				TrueTypeFont.InitializeCoreGlyph(glyphOld, this.initializationInfo, b);
				array[i] = glyphOld;
			}
			return array;
		}

		void EnsureGlyphInitializationInfo()
		{
			if (this.initializationInfo == null)
			{
				OpenTypeFontSource openTypeFontSource = null;
				if (base.FontDescriptor != null)
				{
					openTypeFontSource = base.FontDescriptor.GetOpenTypeFontSource();
				}
				if (openTypeFontSource == null && base.BaseFont != null && base.BaseFont.Value != null)
				{
					openTypeFontSource = base.ContentManager.FontsManager.GetTrueTypeFontSource(base.BaseFont.Value);
				}
				if (openTypeFontSource == null)
				{
					openTypeFontSource = base.ContentManager.FontsManager.GetTrueTypeFontSource(FixedDocumentDefaults.TrueTypeFallbackFontName);
				}
				this.initializationInfo = TrueTypeFont.GetGlyphInitializationInfo(openTypeFontSource, base.FontDescriptor, base.Encoding);
			}
		}

		TrueTypeGlyphInitializationInfo initializationInfo;
	}
}
