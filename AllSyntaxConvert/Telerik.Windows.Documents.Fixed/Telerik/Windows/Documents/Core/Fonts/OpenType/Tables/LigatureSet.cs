using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class LigatureSet : TableBase
	{
		public LigatureSet(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public Ligature[] Ligatures
		{
			get
			{
				if (this.ligatures == null)
				{
					this.ligatures = new Ligature[this.ligatureOffsets.Length];
					for (int i = 0; i < this.ligatures.Length; i++)
					{
						this.ligatures[i] = this.ReadLigature(base.Reader, this.ligatureOffsets[i]);
					}
				}
				return this.ligatures;
			}
		}

		Ligature ReadLigature(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.Offset + (long)((ulong)offset), SeekOrigin.Begin);
			Ligature ligature = new Ligature(base.FontSource);
			ligature.Read(reader);
			reader.EndReadingBlock();
			return ligature;
		}

		public Ligature FindLigature(GlyphsSequence glyphIDs, int startIndex)
		{
			foreach (Ligature ligature in this.Ligatures)
			{
				if (ligature.IsMatch(glyphIDs, startIndex + 1))
				{
					return ligature;
				}
			}
			return null;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.ligatureOffsets = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.ligatureOffsets[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort((ushort)this.Ligatures.Length);
			for (int i = 0; i < this.Ligatures.Length; i++)
			{
				this.Ligatures[i].Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.ligatures = new Ligature[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				Ligature ligature = new Ligature(base.FontSource);
				ligature.Import(reader);
				this.ligatures[i] = ligature;
			}
		}

		ushort[] ligatureOffsets;

		Ligature[] ligatures;
	}
}
