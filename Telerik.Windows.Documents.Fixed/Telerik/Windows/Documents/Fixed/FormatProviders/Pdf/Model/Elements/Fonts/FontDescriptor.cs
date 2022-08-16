using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.Cid;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class FontDescriptor : PdfObject
	{
		public FontDescriptor()
		{
			this.fontName = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("FontName", true));
			this.flags = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Flags", true));
			this.fontBBox = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("FontBBox", true));
			this.italicAngle = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("ItalicAngle", true));
			this.ascent = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Ascent", true));
			this.descent = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Descent", true));
			this.capHeight = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("CapHeight", true));
			this.stemV = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("StemV", true));
			this.fontFamily = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("FontFamily"));
			this.charSet = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("CharSet"));
			this.fontStretch = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("FontStretch"));
			this.fontWeight = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("FontWeight"));
			this.leading = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Leading"));
			this.xHeight = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("XHeight"));
			this.stemH = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("StemH"));
			this.avgWidth = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("AvgWidth"));
			this.maxWidth = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("MaxWidth"));
			this.missingWidth = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("MissingWidth"));
			this.fontFile = base.RegisterReferenceProperty<Type1FontStream>(new PdfPropertyDescriptor("FontFile"));
			this.fontFile2 = base.RegisterReferenceProperty<Type2FontStream>(new PdfPropertyDescriptor("FontFile2"));
			this.fontFile3 = base.RegisterReferenceProperty<Type3FontStream>(new PdfPropertyDescriptor("FontFile3"));
			this.lang = base.RegisterReferenceProperty<PdfName>(new PdfPropertyDescriptor("Lang"));
			this.cidSet = base.RegisterReferenceProperty<CidSet>(new PdfPropertyDescriptor("CIDSet"));
		}

		public PdfName FontName
		{
			get
			{
				return this.fontName.GetValue();
			}
			set
			{
				this.fontName.SetValue(value);
			}
		}

		public PdfInt Flags
		{
			get
			{
				return this.flags.GetValue();
			}
			set
			{
				this.flags.SetValue(value);
			}
		}

		public PdfArray FontBBox
		{
			get
			{
				return this.fontBBox.GetValue();
			}
			set
			{
				this.fontBBox.SetValue(value);
			}
		}

		public PdfReal ItalicAngle
		{
			get
			{
				return this.italicAngle.GetValue();
			}
			set
			{
				this.italicAngle.SetValue(value);
			}
		}

		public PdfReal Ascent
		{
			get
			{
				return this.ascent.GetValue();
			}
			set
			{
				this.ascent.SetValue(value);
			}
		}

		public PdfReal Descent
		{
			get
			{
				return this.descent.GetValue();
			}
			set
			{
				this.descent.SetValue(value);
			}
		}

		public PdfReal CapHeight
		{
			get
			{
				return this.capHeight.GetValue();
			}
			set
			{
				this.capHeight.SetValue(value);
			}
		}

		public PdfReal StemV
		{
			get
			{
				return this.stemV.GetValue();
			}
			set
			{
				this.stemV.SetValue(value);
			}
		}

		public PdfString FontFamily
		{
			get
			{
				return this.fontFamily.GetValue();
			}
			set
			{
				this.fontFamily.SetValue(value);
			}
		}

		public PdfName FontStretch
		{
			get
			{
				return this.fontStretch.GetValue();
			}
			set
			{
				this.fontStretch.SetValue(value);
			}
		}

		public PdfReal FontWeight
		{
			get
			{
				return this.fontWeight.GetValue();
			}
			set
			{
				this.fontWeight.SetValue(value);
			}
		}

		public PdfReal Leading
		{
			get
			{
				return this.leading.GetValue();
			}
			set
			{
				this.leading.SetValue(value);
			}
		}

		public PdfReal XHeight
		{
			get
			{
				return this.xHeight.GetValue();
			}
			set
			{
				this.xHeight.SetValue(value);
			}
		}

		public PdfReal StemH
		{
			get
			{
				return this.stemH.GetValue();
			}
			set
			{
				this.stemH.SetValue(value);
			}
		}

		public PdfReal AvgWidth
		{
			get
			{
				return this.avgWidth.GetValue();
			}
			set
			{
				this.avgWidth.SetValue(value);
			}
		}

		public PdfReal MaxWidth
		{
			get
			{
				return this.maxWidth.GetValue();
			}
			set
			{
				this.maxWidth.SetValue(value);
			}
		}

		public PdfReal MissingWidth
		{
			get
			{
				return this.missingWidth.GetValue();
			}
			set
			{
				this.missingWidth.SetValue(value);
			}
		}

		public PdfString CharSet
		{
			get
			{
				return this.charSet.GetValue();
			}
			set
			{
				this.charSet.SetValue(value);
			}
		}

		public Type1FontStream FontFile
		{
			get
			{
				return this.fontFile.GetValue();
			}
			set
			{
				this.fontFile.SetValue(value);
			}
		}

		public Type2FontStream FontFile2
		{
			get
			{
				return this.fontFile2.GetValue();
			}
			set
			{
				this.fontFile2.SetValue(value);
			}
		}

		public Type3FontStream FontFile3
		{
			get
			{
				return this.fontFile3.GetValue();
			}
			set
			{
				this.fontFile3.SetValue(value);
			}
		}

		public PdfName Lang
		{
			get
			{
				return this.lang.GetValue();
			}
			set
			{
				this.lang.SetValue(value);
			}
		}

		public CidSet CidSet
		{
			get
			{
				return this.cidSet.GetValue();
			}
			set
			{
				this.cidSet.SetValue(value);
			}
		}

		public virtual void CopyPropertiesFrom(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.CopyRequiredPropertiesFrom(font);
			this.FontFamily = font.FontFamily.ToPrimitive((string fontFamily) => fontFamily.ToPdfLiteralString(StringType.ASCII), null);
			this.CharSet = font.CharSet.ToPrimitive((string charSet) => charSet.ToPdfLiteralString(StringType.ASCII), null);
			this.FontStretch = font.FontStretch.ToPrimitive((string fontStretch) => new PdfName(fontStretch), null);
			this.FontWeight = font.FontWeight.ToPrimitive((double weight) => new PdfReal(weight), null);
			this.Leading = font.Leading.ToPrimitive((double leading) => new PdfReal(leading), null);
			this.XHeight = font.XHeight.ToPrimitive((double xHeight) => new PdfReal(xHeight), null);
			this.StemH = font.StemH.ToPrimitive((double stemH) => new PdfReal(stemH), null);
			this.AvgWidth = font.AvgWidth.ToPrimitive((double width) => new PdfReal(width), null);
			this.MaxWidth = font.MaxWidth.ToPrimitive((double width) => new PdfReal(width), null);
			this.MissingWidth = font.MissingWidth.ToPrimitive((double width) => new PdfReal(width), null);
			if (!this.TryExportImportedFontFile(font) && font.FontSource != EmptyFontSource.Instance)
			{
				this.CreateFontFile(context, font);
			}
			CidFontBase cidFont = font as CidFontBase;
			if (cidFont != null)
			{
				this.Lang = cidFont.Lang.ToPrimitive((string language) => new PdfName(language), null);
				this.CidSet = cidFont.CidSet.ToPrimitive((byte[] cidSet) => new CidSet
				{
					Data = cidSet
				}, () => this.CalculateCidSet(context, cidFont));
			}
		}

		CidSet CalculateCidSet(IPdfExportContext context, CidFontBase font)
		{
			CidSet cidSet = null;
			FontType type = font.Type;
			if (type == FontType.CidType2Font)
			{
				cidSet = new CidSet();
				cidSet.CopyPropertiesFrom(context, font);
			}
			return cidSet;
		}

		bool TryExportImportedFontFile(FontBase font)
		{
			bool hasValue = font.FontFileInfo.HasValue;
			if (hasValue)
			{
				switch (font.FontFileInfo.Value.FileType.Value)
				{
				case FontFileType.FontFile:
					this.FontFile = new Type1FontStream();
					this.InitializeFontFile(font);
					break;
				case FontFileType.FontFile2:
					this.FontFile2 = new Type2FontStream();
					this.InitializeFontFile2(font);
					break;
				case FontFileType.FontFile3:
					this.FontFile3 = new Type3FontStream();
					this.InitializeFontFile3(font);
					break;
				}
			}
			return hasValue;
		}

		void InitializeFontFile2(FontBase font)
		{
			FontFileInfo value = font.FontFileInfo.Value;
			this.FontFile2.Metadata = value.Metadata.ToPrimitive((byte[] meta) => new PdfStreamObject
			{
				Data = meta
			}, null);
			this.FontFile2.Length1 = value.Length1.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile2.Data = font.FontSource.Data;
		}

		void InitializeFontFile(FontBase font)
		{
			FontFileInfo value = font.FontFileInfo.Value;
			this.FontFile.Metadata = value.Metadata.ToPrimitive((byte[] meta) => new PdfStreamObject
			{
				Data = meta
			}, null);
			this.FontFile.Length1 = value.Length1.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile.Length2 = value.Length2.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile.Length3 = value.Length3.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile.Data = font.FontSource.Data;
		}

		void InitializeFontFile3(FontBase font)
		{
			FontFileInfo value = font.FontFileInfo.Value;
			this.FontFile3.Subtype = value.FileSubType.ToPrimitive((FontFileSubType subtype) => new PdfName(subtype.ToString()), null);
			this.FontFile3.Metadata = value.Metadata.ToPrimitive((byte[] meta) => new PdfStreamObject
			{
				Data = meta
			}, null);
			this.FontFile3.Length1 = value.Length1.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile3.Length2 = value.Length2.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile3.Length3 = value.Length3.ToPrimitive((int length) => new PdfInt(length), null);
			this.FontFile3.Data = font.FontSource.Data;
		}

		void CreateFontFile(IPdfExportContext context, FontBase font)
		{
			switch (font.Type)
			{
			case FontType.TrueType:
			case FontType.CidType2Font:
				this.FontFile2 = new Type2FontStream();
				this.FontFile2.CopyPropertiesFrom(context, font);
				break;
			case FontType.CidType0Font:
				break;
			default:
				return;
			}
		}

		void CopyRequiredPropertiesFrom(FontBase font)
		{
			int defaultValue = FontDescriptor.GetFlags(font);
			this.Flags = new PdfInt(defaultValue);
			this.FontName = font.FontName.ToPdfName();
			this.FontBBox = font.BoundingBox.Value.ToPdfArray();
			this.ItalicAngle = font.ItalicAngle.Value.ToPdfReal();
			this.Ascent = (font.Ascent.Value * 1000.0).ToPdfReal();
			this.Descent = (font.Descent.Value * 1000.0).ToPdfReal();
			this.CapHeight = font.CapHeight.Value.ToPdfReal();
			this.StemV = font.StemV.Value.ToPdfReal();
		}

		static int GetFlags(FontBase font)
		{
			FlagWriter<FontFlag> flagWriter = new FlagWriter<FontFlag>();
			flagWriter.SetFlagOnCondition(FontFlag.FixedPitch, font.IsFixedPitch);
			flagWriter.SetFlagOnCondition(FontFlag.Serif, font.IsSerif);
			flagWriter.SetFlagOnCondition(FontFlag.Symbolic, font.IsSymbolic);
			flagWriter.SetFlagOnCondition(FontFlag.Script, font.IsScript);
			flagWriter.SetFlagOnCondition(FontFlag.Nonsymbolic, font.IsNonSymbolic);
			flagWriter.SetFlagOnCondition(FontFlag.Italic, font.IsItalic);
			flagWriter.SetFlagOnCondition(FontFlag.AllCap, font.IsAllCap);
			flagWriter.SetFlagOnCondition(FontFlag.SmallCap, font.IsSmallCap);
			flagWriter.SetFlagOnCondition(FontFlag.ForceBold, font.IsForcingBold);
			return flagWriter.ResultFlags;
		}

		readonly DirectProperty<PdfName> fontName;

		readonly DirectProperty<PdfString> fontFamily;

		readonly DirectProperty<PdfInt> flags;

		readonly DirectProperty<PdfArray> fontBBox;

		readonly DirectProperty<PdfReal> italicAngle;

		readonly DirectProperty<PdfReal> ascent;

		readonly DirectProperty<PdfReal> descent;

		readonly DirectProperty<PdfReal> capHeight;

		readonly DirectProperty<PdfReal> stemV;

		readonly DirectProperty<PdfName> fontStretch;

		readonly DirectProperty<PdfReal> fontWeight;

		readonly DirectProperty<PdfReal> leading;

		readonly DirectProperty<PdfReal> xHeight;

		readonly DirectProperty<PdfReal> stemH;

		readonly DirectProperty<PdfReal> avgWidth;

		readonly DirectProperty<PdfReal> maxWidth;

		readonly DirectProperty<PdfReal> missingWidth;

		readonly DirectProperty<PdfString> charSet;

		readonly ReferenceProperty<Type1FontStream> fontFile;

		readonly ReferenceProperty<Type2FontStream> fontFile2;

		readonly ReferenceProperty<Type3FontStream> fontFile3;

		readonly ReferenceProperty<PdfName> lang;

		readonly ReferenceProperty<CidSet> cidSet;
	}
}
