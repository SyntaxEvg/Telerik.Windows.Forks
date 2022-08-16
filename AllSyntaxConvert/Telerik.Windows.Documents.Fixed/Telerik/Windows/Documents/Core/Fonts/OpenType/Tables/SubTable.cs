using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class SubTable : TableBase
	{
		static SubTable CreateSubTable(OpenTypeFontSourceBase fontSource, ushort type, OpenTypeFontReader reader)
		{
			long position = reader.Position;
			SubTable subTable;
			switch (type)
			{
			case 1:
			{
				ushort format = reader.ReadUShort();
				subTable = SingleSubstitution.CreateSingleSubstitutionTable(fontSource, format);
				goto IL_48;
			}
			case 2:
				subTable = new MultipleSubstitution(fontSource);
				goto IL_48;
			case 4:
				subTable = new LigatureSubstitution(fontSource);
				goto IL_48;
			}
			return null;
			IL_48:
			subTable.Offset = position;
			return subTable;
		}

		internal static SubTable ReadSubTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader, ushort type)
		{
			SubTable subTable = SubTable.CreateSubTable(fontSource, type, reader);
			if (subTable != null)
			{
				subTable.Read(reader);
			}
			return subTable;
		}

		internal static SubTable ImportSubTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader, ushort type)
		{
			SubTable subTable = SubTable.CreateSubTable(fontSource, type, reader);
			if (subTable != null)
			{
				subTable.Import(reader);
			}
			return subTable;
		}

		public SubTable(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		protected Coverage ReadCoverage(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.Offset + (long)((ulong)offset), SeekOrigin.Begin);
			Coverage result = Coverage.ReadCoverageTable(base.FontSource, reader);
			reader.EndReadingBlock();
			return result;
		}

		public abstract GlyphsSequence Apply(GlyphsSequence glyphIndices);
	}
}
