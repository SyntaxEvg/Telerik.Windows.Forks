using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines
{
	class CompositeGlyph : GlyphData
	{
		static OutlinePoint GetTransformedPoint(Matrix matrix, OutlinePoint point)
		{
			return new OutlinePoint(point.Flags)
			{
				Point = matrix.Transform(point.Point)
			};
		}

		public CompositeGlyph(OpenTypeFontSourceBase fontFile, ushort glyphIndex)
			: base(fontFile, glyphIndex)
		{
		}

		internal override IEnumerable<OutlinePoint[]> Contours
		{
			get
			{
				return this.contours;
			}
		}

		void AddGlyph(GlyphDescription gd)
		{
			GlyphData glyphData = base.FontSource.GetGlyphData(gd.GlyphIndex);
			if (glyphData == null)
			{
				return;
			}
			foreach (OutlinePoint[] array in glyphData.Contours)
			{
				OutlinePoint[] array2 = new OutlinePoint[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = CompositeGlyph.GetTransformedPoint(gd.Transform, array[i]);
				}
				this.contours.Add(array2);
			}
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.contours = new List<OutlinePoint[]>();
			GlyphDescription glyphDescription;
			do
			{
				glyphDescription = new GlyphDescription();
				glyphDescription.Read(reader);
				this.AddGlyph(glyphDescription);
			}
			while (glyphDescription.CheckFlag(5));
		}

		List<OutlinePoint[]> contours;
	}
}
