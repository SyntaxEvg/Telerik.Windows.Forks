using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class MultipleSubstitution : SubTable
	{
		public MultipleSubstitution(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		protected Coverage Coverage
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

		protected Sequence[] Sequences
		{
			get
			{
				if (this.sequences == null)
				{
					this.sequences = new Sequence[this.sequenceOffsets.Length];
					for (int i = 0; i < this.sequences.Length; i++)
					{
						this.sequences[i] = this.ReadSequence(base.Reader, this.sequenceOffsets[i]);
					}
				}
				return this.sequences;
			}
		}

		Sequence ReadSequence(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.Offset + (long)((ulong)offset), SeekOrigin.Begin);
			Sequence sequence = new Sequence();
			sequence.Read(reader);
			reader.EndReadingBlock();
			return sequence;
		}

		public override GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = new GlyphsSequence(glyphIDs.Count);
			for (int i = 0; i < glyphIDs.Count; i++)
			{
				int coverageIndex = this.Coverage.GetCoverageIndex(glyphIDs[i].GlyphId);
				if (coverageIndex >= 0)
				{
					glyphsSequence.AddRange(this.sequences[coverageIndex].Subsitutes);
				}
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			this.coverageOffset = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.sequenceOffsets = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.sequenceOffsets[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			this.Coverage.Write(writer);
			writer.WriteUShort((ushort)this.Sequences.Length);
			for (int i = 0; i < this.Sequences.Length; i++)
			{
				this.Sequences[i].Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.coverage = Coverage.ImportCoverageTable(base.FontSource, reader);
			ushort num = reader.ReadUShort();
			this.sequences = new Sequence[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				Sequence sequence = new Sequence();
				sequence.Import(reader);
				this.sequences[i] = sequence;
			}
		}

		ushort coverageOffset;

		ushort[] sequenceOffsets;

		Coverage coverage;

		Sequence[] sequences;
	}
}
