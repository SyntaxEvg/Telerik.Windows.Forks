using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	abstract class FontObject : PdfObject
	{
		public FontObject()
		{
			this.baseFont = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("BaseFont", true));
			this.toUnicode = base.RegisterReferenceProperty<ToUnicodeCMap>(new PdfPropertyDescriptor("ToUnicode", true));
		}

		public PdfName BaseFont
		{
			get
			{
				return this.baseFont.GetValue();
			}
			set
			{
				this.baseFont.SetValue(value);
			}
		}

		public ToUnicodeCMap ToUnicode
		{
			get
			{
				return this.toUnicode.GetValue();
			}
			set
			{
				this.toUnicode.SetValue(value);
			}
		}

		public FontBase ToFont(PostScriptReader reader, IPdfImportContext context)
		{
			this.PrepareForImport(reader, context);
			FontBase fontBase;
			if (!this.TryCreateFontFromFontFile(out fontBase) && !this.TryCreateFontFromFontName(out fontBase) && !this.TryCreateFontFromFontFamily(out fontBase))
			{
				fontBase = this.CreateFontWithoutFontFile();
			}
			if (fontBase != null)
			{
				this.CopyPropertiesToFont(fontBase, reader, context);
			}
			return fontBase;
		}

		public IEnumerable<CharInfo> GetCharacters(PdfString str)
		{
			Guard.ThrowExceptionIfNull<PdfString>(str, "str");
			return this.GetCharactersOverride(str);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.CopyPropertiesFromOverride(context, font);
		}

		protected abstract bool TryGetFontFamily(out FontFamily fontFamily);

		protected abstract bool TryCreateFontFromFontFile(out FontBase font);

		protected abstract FontBase CreateFontWithoutFontFile();

		protected abstract bool TryCreateFontFromFontProperties(FontProperties fontProperties, out FontBase font);

		protected virtual void CopyPropertiesFromOverride(IPdfExportContext context, FontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.BaseFont = font.FontName.ToPdfName();
			this.ToUnicode = font.ToUnicode.ToPrimitive((CMapEncoding toUnicode) => new ToUnicodeCMap(toUnicode), () => FontObject.CreateToUnicodeCMapFromFont(context, font));
		}

		protected virtual IEnumerable<CharInfo> GetCharactersOverride(PdfString str)
		{
			Guard.ThrowExceptionIfNull<PdfString>(str, "str");
			foreach (byte b in str.Value)
			{
				CharCode code = new CharCode(b);
				yield return new CharInfo(this.GetToUnicode(code), code);
			}
			yield break;
		}

		protected virtual string GetToUnicode(CharCode code)
		{
			string result;
			if (this.ToUnicode != null && this.ToUnicode.Encoding.TryGetToUnicode(code, out result))
			{
				return result;
			}
			return ((char)code.Code).ToString();
		}

		protected virtual void PrepareForImport(PostScriptReader reader, IPdfImportContext context)
		{
		}

		protected virtual bool TryCreateFontFromFontName(out FontBase font)
		{
			string familyName;
			FontWeight fontWeight;
			FontStyle fontStyle;
			FontsHelper.GetFontFamily(this.BaseFont.Value, out familyName, out fontWeight, out fontStyle);
			FontProperties fontProperties = new FontProperties(new FontFamily(familyName), fontStyle, fontWeight);
			return this.TryCreateFontFromFontProperties(fontProperties, out font);
		}

		protected void CopyFontDescriptorPropertiesToFont(FontDescriptor descriptor, FontBase font)
		{
			if (descriptor != null)
			{
				descriptor.Ascent.CopyToProperty(font.Ascent, (PdfReal ascent) => ascent.Value / 1000.0);
				descriptor.Descent.CopyToProperty(font.Descent, (PdfReal descent) => descent.Value / 1000.0);
				descriptor.CapHeight.CopyToProperty(font.CapHeight, (PdfReal capHeight) => capHeight.Value);
				descriptor.FontBBox.CopyToProperty(font.BoundingBox, delegate(PdfArray box)
				{
					double[] array = box.ToDoubleArray();
					double num = array[0];
					double num2 = array[1];
					double width = array[2] - num;
					double height = array[3] - num2;
					return new Rect(num, num2, width, height);
				});
				descriptor.ItalicAngle.CopyToProperty(font.ItalicAngle, (PdfReal angle) => angle.Value);
				descriptor.StemV.CopyToProperty(font.StemV, (PdfReal stemV) => stemV.Value);
				descriptor.FontFamily.CopyToProperty(font.FontFamily, (PdfString fontFamily) => fontFamily.ToString());
				descriptor.CharSet.CopyToProperty(font.CharSet, (PdfString charSet) => charSet.ToString());
				descriptor.FontStretch.CopyToProperty(font.FontStretch, (PdfName fontStretch) => fontStretch.Value);
				descriptor.FontWeight.CopyToProperty(font.FontWeight, (PdfReal weight) => weight.Value);
				descriptor.Leading.CopyToProperty(font.Leading, (PdfReal leading) => leading.Value);
				descriptor.XHeight.CopyToProperty(font.XHeight, (PdfReal xHeight) => xHeight.Value);
				descriptor.StemH.CopyToProperty(font.StemH, (PdfReal stemH) => stemH.Value);
				descriptor.AvgWidth.CopyToProperty(font.AvgWidth, (PdfReal width) => width.Value);
				descriptor.MaxWidth.CopyToProperty(font.MaxWidth, (PdfReal width) => width.Value);
				descriptor.MissingWidth.CopyToProperty(font.MissingWidth, (PdfReal width) => width.Value);
				if (descriptor.Flags != null)
				{
					this.CopyFlagsToFont(descriptor.Flags.Value, font);
				}
			}
		}

		protected virtual void CopyPropertiesToFont(FontBase font, PostScriptReader reader, IPdfImportContext context)
		{
			this.ToUnicode.CopyToProperty(font.ToUnicode, (ToUnicodeCMap toUnicode) => toUnicode.Encoding);
		}

		void CopyFlagsToFont(int flags, FontBase font)
		{
			FlagReader<FontFlag> flagReader = new FlagReader<FontFlag>(flags);
			font.IsFixedPitch = flagReader.IsSet(FontFlag.FixedPitch);
			font.IsSerif = flagReader.IsSet(FontFlag.Serif);
			font.IsSymbolic = flagReader.IsSet(FontFlag.Symbolic);
			font.IsScript = flagReader.IsSet(FontFlag.Script);
			font.IsNonSymbolic = flagReader.IsSet(FontFlag.Nonsymbolic);
			font.IsItalic = flagReader.IsSet(FontFlag.Italic);
			font.IsAllCap = flagReader.IsSet(FontFlag.AllCap);
			font.IsSmallCap = flagReader.IsSet(FontFlag.SmallCap);
			font.IsForcingBold = flagReader.IsSet(FontFlag.ForceBold);
		}

		bool TryCreateFontFromFontFamily(out FontBase font)
		{
			bool result = false;
			FontFamily fontFamily;
			if (this.TryGetFontFamily(out fontFamily))
			{
				result = this.TryCreateFontFromFontProperties(new FontProperties(fontFamily), out font);
			}
			else
			{
				font = null;
			}
			return result;
		}

		static ToUnicodeCMap CreateToUnicodeCMapFromFont(IPdfExportContext context, FontBase font)
		{
			ToUnicodeCMap toUnicodeCMap = new ToUnicodeCMap();
			toUnicodeCMap.CopyPropertiesFrom(context, context.GetUsedCharacters(font));
			return toUnicodeCMap;
		}

		readonly DirectProperty<PdfName> baseFont;

		readonly ReferenceProperty<ToUnicodeCMap> toUnicode;
	}
}
