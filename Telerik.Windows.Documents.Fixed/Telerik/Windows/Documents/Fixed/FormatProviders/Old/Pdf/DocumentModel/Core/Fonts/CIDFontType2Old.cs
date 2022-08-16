using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	[PdfClass(TypeName = "Font", SubtypeProperty = "Subtype", SubtypeValue = "CIDFontType2")]
	class CIDFontType2Old : CIDFontOld
	{
		public CIDFontType2Old(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.cidToGIDMap = base.CreateInstantLoadProperty<CIDToGIDMapOld>(new PdfPropertyDescriptor
			{
				Name = "CIDToGIDMap"
			}, new CIDToGIDMapOld(contentManager), Converters.CidToGidMapConverter);
		}

		public override FontSource FontSource
		{
			get
			{
				return this.OpenTypeFontSource;
			}
		}

		public CIDToGIDMapOld CIDToGIDMap
		{
			get
			{
				return this.cidToGIDMap.GetValue();
			}
			set
			{
				this.cidToGIDMap.SetValue(value);
			}
		}

		OpenTypeFontSource OpenTypeFontSource
		{
			get
			{
				if (this.fontSource == null)
				{
					if (this.fontSource == null && base.FontDescriptor != null)
					{
						this.fontSource = base.FontDescriptor.GetOpenTypeFontSource();
					}
					if (this.fontSource == null && base.BaseFont != null)
					{
						this.fontSource = base.ContentManager.FontsManager.GetTrueTypeFontSource(base.BaseFont.Value);
					}
				}
				return this.fontSource;
			}
		}

		public override IEnumerable<GlyphOld> GetGlyphs(IEnumerable<CharCodeOld> charIds)
		{
			if (this.OpenTypeFontSource != null && this.CIDToGIDMap != null)
			{
				return CIDFontType2Old.EnumerateGlyphs(charIds, this.CIDToGIDMap);
			}
			return null;
		}

		static IEnumerable<GlyphOld> EnumerateGlyphs(IEnumerable<CharCodeOld> charIds, CIDToGIDMapOld mapping)
		{
			foreach (CharCodeOld cid in charIds)
			{
				GlyphOld g = new GlyphOld();
				g.CharId = cid;
				Glyph g2 = g;
				CharCodeOld charCodeOld = cid;
				CidType2Font.InitializeCoreGlyph(g2, mapping, charCodeOld.IntValue);
				yield return g;
			}
			yield break;
		}

		readonly InstantLoadProperty<CIDToGIDMapOld> cidToGIDMap;

		OpenTypeFontSource fontSource;
	}
}
