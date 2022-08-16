using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;
using Telerik.Windows.Documents.Core.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Encoding : CFFTable, IEncoding
	{
		public Encoding(ICFFFontFile file, Charset charset, long offset)
			: base(file, offset)
		{
			this.charset = charset;
		}

		public SupplementalEncoding SupplementalEncoding { get; set; }

		void ReadFormat0(CFFFontReader reader)
		{
			byte b = reader.ReadCard8();
			this.gids = new List<ushort>((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				this.gids.Add((ushort)reader.ReadCard8());
			}
		}

		void ReadFormat1(CFFFontReader reader)
		{
			byte b = reader.ReadCard8();
			this.gids = new List<ushort>();
			for (int i = 0; i < (int)b; i++)
			{
				byte b2 = reader.ReadCard8();
				byte b3 = reader.ReadCard8();
				this.gids.Add((ushort)b2);
				for (int j = 0; j < (int)b3; j++)
				{
					this.gids.Add((ushort)((byte)((int)b2 + j + 1)));
				}
			}
		}

		public override void Read(CFFFontReader reader)
		{
			byte n = reader.ReadCard8();
			if (BitsHelper.GetBit((int)n, 0))
			{
				this.ReadFormat1(reader);
			}
			else
			{
				this.ReadFormat0(reader);
			}
			if (BitsHelper.GetBit((int)n, 7))
			{
				this.SupplementalEncoding = new SupplementalEncoding();
				this.SupplementalEncoding.Read(reader);
			}
		}

		public string GetGlyphName(ICFFFontFile fontFile, ushort index)
		{
			int num = this.gids.IndexOf(index);
			if (num < 0)
			{
				return ".notdef";
			}
			return this.charset[(ushort)num];
		}

		readonly Charset charset;

		List<ushort> gids;
	}
}
