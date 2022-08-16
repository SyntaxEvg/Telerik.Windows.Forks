using System;
using System.IO;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class CharString : Index
	{
		public CharString(Top top, long offset)
			: base(top.File, offset)
		{
			Guard.ThrowExceptionIfNull<Top>(top, "top");
			this.top = top;
		}

		public GlyphData this[ushort index]
		{
			get
			{
				if (this.glyphOutlines[(int)index] == null)
				{
					this.glyphOutlines[(int)index] = this.ReadGlyphData(base.Reader, base.Offsets[(int)index], base.GetDataLength((int)index));
				}
				return this.glyphOutlines[(int)index];
			}
		}

		public int GetAdvancedWidth(ushort glyphId, int defaultWidth, int nominalWidth)
		{
			GlyphData glyphData = this[glyphId];
			if (!glyphData.HasWidth)
			{
				return defaultWidth;
			}
			return (int)glyphData.AdvancedWidth + nominalWidth;
		}

		public void GetGlyphOutlines(Glyph glyph, double fontSize)
		{
			GlyphData glyphData = this[glyph.GlyphId];
			GlyphOutlinesCollection glyphOutlinesCollection = glyphData.Oultlines.Clone();
			Matrix fontMatrix = this.top.FontMatrix;
			fontMatrix.ScaleAppend(fontSize, -fontSize, 0.0, 0.0);
			glyphOutlinesCollection.Transform(fontMatrix);
			glyph.Outlines = glyphOutlinesCollection;
		}

		GlyphData ReadGlyphData(CFFFontReader reader, uint offset, int length)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.DataOffset + (long)((ulong)offset), SeekOrigin.Begin);
			byte[] array = new byte[length];
			reader.Read(array, array.Length);
			BuildChar buildChar = new BuildChar(this.top);
			buildChar.Execute(array);
			reader.EndReadingBlock();
			GlyphOutlinesCollection outlines = buildChar.GlyphOutlines;
			int? width = buildChar.Width;
			return new GlyphData(outlines, (width != null) ? new ushort?((ushort)width.GetValueOrDefault()) : null);
		}

		public override void Read(CFFFontReader reader)
		{
			base.Read(reader);
			this.glyphOutlines = new GlyphData[(int)base.Count];
		}

		GlyphData[] glyphOutlines;

		Top top;
	}
}
