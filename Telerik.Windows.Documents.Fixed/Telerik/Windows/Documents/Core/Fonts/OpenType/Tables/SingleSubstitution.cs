using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class SingleSubstitution : SubTable
	{
		internal static SingleSubstitution CreateSingleSubstitutionTable(OpenTypeFontSourceBase fontFile, ushort format)
		{
			switch (format)
			{
			case 1:
				return new SingleSubstitutionFormat1(fontFile);
			case 2:
				return new SingleSubstitutionFormat2(fontFile);
			default:
				return null;
			}
		}

		public SingleSubstitution(OpenTypeFontSourceBase fontFile)
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

		public override void Read(OpenTypeFontReader reader)
		{
			this.coverageOffset = reader.ReadUShort();
		}

		internal override void Write(FontWriter writer)
		{
			this.Coverage.Write(writer);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.coverage = Coverage.ImportCoverageTable(base.FontSource, reader);
		}

		ushort coverageOffset;

		Coverage coverage;
	}
}
