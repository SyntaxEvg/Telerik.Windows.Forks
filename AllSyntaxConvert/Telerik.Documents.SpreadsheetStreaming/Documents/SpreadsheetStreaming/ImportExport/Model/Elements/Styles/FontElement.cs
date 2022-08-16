using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FontElement : DirectElementBase<FontProperties>
	{
		public FontElement()
		{
			this.elementNameToReadMethod = new Dictionary<string, Action<ElementBase, FontProperties>>();
			this.elementNameToReadMethod.Add("b", new Action<ElementBase, FontProperties>(this.ReadBoldElement));
			this.elementNameToReadMethod.Add("i", new Action<ElementBase, FontProperties>(this.ReadItalicElement));
			this.elementNameToReadMethod.Add("color", new Action<ElementBase, FontProperties>(this.ReadColorElement));
			this.elementNameToReadMethod.Add("sz", new Action<ElementBase, FontProperties>(this.ReadFontSizeElement));
			this.elementNameToReadMethod.Add("name", new Action<ElementBase, FontProperties>(this.ReadFontNameElement));
			this.elementNameToReadMethod.Add("u", new Action<ElementBase, FontProperties>(this.ReadUnderlineElement));
			this.elementNameToReadMethod.Add("family", new Action<ElementBase, FontProperties>(this.ReadFontFamilyElement));
			this.elementNameToReadMethod.Add("scheme", new Action<ElementBase, FontProperties>(this.ReadSchemeElement));
			this.elementNameToReadMethod.Add("vertAlign", new Action<ElementBase, FontProperties>(this.ReadVerticalAlignmentElement));
			this.elementNameToReadMethod.Add("strike", new Action<ElementBase, FontProperties>(this.ReadStrikeElement));
		}

		public override string ElementName
		{
			get
			{
				return "font";
			}
		}

		protected override void InitializeAttributesOverride(FontProperties value)
		{
		}

		protected override void WriteChildElementsOverride(FontProperties value)
		{
			if (value.IsBold != null)
			{
				BoldElement boldElement = base.CreateChildElement<BoldElement>();
				boldElement.Write(value.IsBold.Value);
			}
			if (value.IsItalic != null)
			{
				ItalicElement italicElement = base.CreateChildElement<ItalicElement>();
				italicElement.Write(value.IsItalic.Value);
			}
			if (value.ForeColor != null)
			{
				ColorElement colorElement = base.CreateChildElement<ColorElement>();
				colorElement.Write(value.ForeColor);
			}
			if (value.FontSize != null)
			{
				FontSizeElement fontSizeElement = base.CreateChildElement<FontSizeElement>();
				fontSizeElement.Write(value.FontSize.Value);
			}
			if (value.FontFamily != null)
			{
				FontNameElement fontNameElement = base.CreateChildElement<FontNameElement>();
				fontNameElement.Write(value.FontFamily.GetActualValue(base.Theme));
				if (value.FontFamily.IsFromTheme)
				{
					SchemeElement schemeElement = base.CreateChildElement<SchemeElement>();
					schemeElement.Write(FontSchemesMapper.GetFontSchemeName(value.FontFamily.ThemeFontType));
				}
			}
			if (value.Underline != null)
			{
				UnderlineElement underlineElement = base.CreateChildElement<UnderlineElement>();
				underlineElement.Write(UnderlineValuesMapper.GetUnderlineValueName(value.Underline.Value));
			}
			if (value.VerticalAlignment != null)
			{
				VerticalAlignmentElement verticalAlignmentElement = base.CreateChildElement<VerticalAlignmentElement>();
				verticalAlignmentElement.Write(value.VerticalAlignment);
			}
			if (value.Strike != null)
			{
				StrikeElement strikeElement = base.CreateChildElement<StrikeElement>();
				strikeElement.Write(value.Strike.Value);
			}
		}

		protected override void CopyAttributesOverride(ref FontProperties value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref FontProperties value)
		{
			this.elementNameToReadMethod[element.ElementName](element, value);
		}

		void ReadBoldElement(ElementBase element, FontProperties value)
		{
			BoldElement boldElement = element as BoldElement;
			bool value2 = false;
			boldElement.Read(ref value2);
			value.IsBold = new bool?(value2);
		}

		void ReadItalicElement(ElementBase element, FontProperties value)
		{
			ItalicElement italicElement = element as ItalicElement;
			bool value2 = false;
			italicElement.Read(ref value2);
			value.IsItalic = new bool?(value2);
		}

		void ReadColorElement(ElementBase element, FontProperties value)
		{
			ColorElement colorElement = element as ColorElement;
			SpreadThemableColor spreadThemableColor = null;
			colorElement.Read(ref spreadThemableColor);
			if (spreadThemableColor != null)
			{
				value.ForeColor = spreadThemableColor;
			}
		}

		void ReadFontSizeElement(ElementBase element, FontProperties value)
		{
			FontSizeElement fontSizeElement = element as FontSizeElement;
			double num = 1.0;
			fontSizeElement.Read(ref num);
			if (num > 0.0)
			{
				value.FontSize = new double?(num);
			}
		}

		void ReadFontNameElement(ElementBase element, FontProperties value)
		{
			FontNameElement fontNameElement = element as FontNameElement;
			string text = null;
			fontNameElement.Read(ref text);
			if (text != null)
			{
				value.FontFamily = new SpreadThemableFontFamily(text);
			}
		}

		void ReadFontFamilyElement(ElementBase element, FontProperties value)
		{
			FontFamilyElement fontFamilyElement = element as FontFamilyElement;
			string text = null;
			fontFamilyElement.Read(ref text);
		}

		void ReadSchemeElement(ElementBase element, FontProperties value)
		{
			SchemeElement schemeElement = element as SchemeElement;
			string text = null;
			schemeElement.Read(ref text);
			if (text != null)
			{
				SpreadThemeFontType? fontScheme = FontSchemesMapper.GetFontScheme(text);
				if (fontScheme != null)
				{
					value.FontFamily = new SpreadThemableFontFamily(fontScheme.Value);
				}
			}
		}

		void ReadUnderlineElement(ElementBase element, FontProperties value)
		{
			UnderlineElement underlineElement = element as UnderlineElement;
			string name = null;
			underlineElement.Read(ref name);
			value.Underline = new SpreadUnderlineType?(UnderlineValuesMapper.GetUnderlineValue(name));
		}

		void ReadVerticalAlignmentElement(ElementBase element, FontProperties value)
		{
			VerticalAlignmentElement verticalAlignmentElement = element as VerticalAlignmentElement;
			string verticalAlignment = null;
			verticalAlignmentElement.Read(ref verticalAlignment);
			value.VerticalAlignment = verticalAlignment;
		}

		void ReadStrikeElement(ElementBase element, FontProperties value)
		{
			StrikeElement strikeElement = element as StrikeElement;
			bool value2 = false;
			strikeElement.Read(ref value2);
			value.Strike = new bool?(value2);
		}

		readonly Dictionary<string, Action<ElementBase, FontProperties>> elementNameToReadMethod;
	}
}
