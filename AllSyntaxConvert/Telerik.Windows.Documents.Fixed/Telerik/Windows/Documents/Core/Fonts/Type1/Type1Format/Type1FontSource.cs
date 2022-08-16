using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries;
using Telerik.Windows.Documents.Core.PostScript;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format
{
	class Type1FontSource : FontSource
	{
		public Type1FontSource(byte[] data)
		{
			this.data = data;
		}

		public override string GetFontFamily()
		{
			return this.Font.FontInfo.FamilyName;
		}

		public override double GetItalicAngle()
		{
			return 0.0;
		}

		public override bool GetIsItalic()
		{
			return false;
		}

		public override bool GetIsBold()
		{
			throw new NotImplementedException();
		}

		public override double GetUnderlineThickness()
		{
			return this.Font.FontInfo.UnderlineThickness;
		}

		public override double GetUnderlinePosition()
		{
			return this.Font.FontInfo.UnderlinePosition;
		}

		public override double GetAscent()
		{
			throw new NotImplementedException();
		}

		public override double GetDescent()
		{
			throw new NotImplementedException();
		}

		public override double GetLineGap()
		{
			throw new NotImplementedException();
		}

		public override double GetCapHeight()
		{
			throw new NotImplementedException();
		}

		public override double GetStemV()
		{
			throw new NotImplementedException();
		}

		public override Rect GetBoundingBox()
		{
			return this.Font.FontBBox.ToRect();
		}

		public override byte[] GetData()
		{
			return this.data;
		}

		public Type1Font Font
		{
			get
			{
				Type1Font result;
				lock (Type1FontSource.lockObject)
				{
					if (this.font == null)
					{
						lock (Type1FontSource.lockObject)
						{
							this.font = Type1FontSource.ReadType1Font(this.data);
						}
					}
					result = this.font;
				}
				return result;
			}
		}

		public virtual string GetGlyphName(ushort cid)
		{
			return this.Font.GetGlyphName(cid);
		}

		static Type1Font ReadType1Font(byte[] data)
		{
			Interpreter interpreter = new Interpreter();
			interpreter.Execute(Type1FontReader.StripData(data));
			return interpreter.Fonts.Values.First<Type1Font>();
		}

		public override void GetAdvancedWidthOverride(Glyph glyph)
		{
			glyph.AdvancedWidth = (double)this.Font.GetAdvancedWidth(glyph.Name) / 1000.0;
		}

		public override void InitializeGlyphOutlinesOverride(Glyph glyph, double fontSize)
		{
			this.Font.GetGlyphOutlines(glyph, fontSize);
		}

		public override bool TryGetCharCode(int unicode, out int charCode)
		{
			ushort num;
			bool result = this.Font.TryGetCharCode((ushort)unicode, out num);
			charCode = (int)num;
			return result;
		}

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			int num;
			bool result = this.TryGetCharCode(unicode, out num);
			glyphId = (ushort)num;
			return result;
		}

		public override double GetAdvancedWidthOverride(int charCode)
		{
			string glyphName = this.GetGlyphName((ushort)charCode);
			if (!string.IsNullOrEmpty(glyphName))
			{
				return (double)this.Font.GetAdvancedWidth(this.GetGlyphName((ushort)charCode));
			}
			return 0.0;
		}

		static readonly object lockObject = new object();

		readonly byte[] data;

		Type1Font font;
	}
}
