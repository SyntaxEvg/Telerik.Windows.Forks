using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class Coverage : TableBase
	{
		static Coverage CreateCoverageTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			long position = reader.Position;
			Coverage coverage;
			switch (reader.ReadUShort())
			{
			case 1:
				coverage = new CoverageFormat1(fontSource);
				break;
			case 2:
				coverage = new CoverageFormat2(fontSource);
				break;
			default:
				return null;
			}
			coverage.Offset = position;
			return coverage;
		}

		internal static Coverage ReadCoverageTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			Coverage coverage = Coverage.CreateCoverageTable(fontSource, reader);
			if (coverage != null)
			{
				coverage.Read(reader);
			}
			return coverage;
		}

		internal static Coverage ImportCoverageTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			Coverage coverage = Coverage.CreateCoverageTable(fontSource, reader);
			if (coverage != null)
			{
				coverage.Import(reader);
			}
			return coverage;
		}

		public Coverage(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.Read(reader);
		}

		public abstract int GetCoverageIndex(ushort glyphIndex);
	}
}
