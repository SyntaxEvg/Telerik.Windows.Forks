using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class LigatureSubstitution : SubTable
	{
		public LigatureSubstitution(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public Coverage Coverage
		{
			get
			{
				if (this.coverage == null)
				{
					this.coverage = base.ReadCoverage(base.Reader, this.coverageOffset);
				}
				return this.coverage;
			}
		}

		public LigatureSet[] LigatureSets
		{
			get
			{
				if (this.ligatureSets == null)
				{
					this.ligatureSets = new LigatureSet[this.ligatureSetOffsets.Length];
					for (int i = 0; i < this.ligatureSets.Length; i++)
					{
						this.ligatureSets[i] = this.ReadLigatureSet(base.Reader, this.ligatureSetOffsets[i]);
					}
				}
				return this.ligatureSets;
			}
		}

		LigatureSet ReadLigatureSet(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			long offset2 = base.Offset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			LigatureSet ligatureSet = new LigatureSet(base.FontSource);
			ligatureSet.Read(reader);
			ligatureSet.Offset = offset2;
			reader.EndReadingBlock();
			return ligatureSet;
		}

		public override GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = new GlyphsSequence(glyphIDs.Count);
			for (int i = 0; i < glyphIDs.Count; i++)
			{
				int coverageIndex = this.Coverage.GetCoverageIndex(glyphIDs[i].GlyphId);
				if (coverageIndex < 0)
				{
					glyphsSequence.Add(glyphIDs[i]);
				}
				else
				{
					Ligature ligature = this.LigatureSets[coverageIndex].FindLigature(glyphIDs, i);
					if (ligature != null)
					{
						glyphsSequence.Add(ligature.LigatureGlyphId);
						i += ligature.Length;
					}
					else
					{
						glyphsSequence.Add(glyphIDs[i]);
					}
				}
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			this.coverageOffset = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.ligatureSetOffsets = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.ligatureSetOffsets[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			this.Coverage.Write(writer);
			writer.WriteUShort((ushort)this.ligatureSetOffsets.Length);
			foreach (LigatureSet ligatureSet in this.LigatureSets)
			{
				ligatureSet.Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.coverage = Coverage.ImportCoverageTable(base.FontSource, reader);
			ushort num = reader.ReadUShort();
			this.ligatureSets = new LigatureSet[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				LigatureSet ligatureSet = new LigatureSet(base.FontSource);
				ligatureSet.Import(reader);
				this.ligatureSets[i] = ligatureSet;
			}
		}

		ushort coverageOffset;

		ushort[] ligatureSetOffsets;

		LigatureSet[] ligatureSets;

		Coverage coverage;
	}
}
