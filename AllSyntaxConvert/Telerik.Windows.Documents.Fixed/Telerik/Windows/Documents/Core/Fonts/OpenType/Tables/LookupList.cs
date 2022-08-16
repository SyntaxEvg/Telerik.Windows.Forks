using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class LookupList : TableBase
	{
		public LookupList(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		Lookup ReadLookup(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			long offset2 = base.Offset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			ushort type = reader.ReadUShort();
			if (!Lookup.IsSupported(type))
			{
				return null;
			}
			Lookup lookup = new Lookup(base.FontSource, type);
			lookup.Offset = offset2;
			lookup.Read(reader);
			reader.EndReadingBlock();
			return lookup;
		}

		public Lookup GetLookup(ushort index)
		{
			if (this.lookups[(int)index] == null && this.lookupOffsets != null)
			{
				this.lookups[(int)index] = this.ReadLookup(base.Reader, this.lookupOffsets[(int)index]);
			}
			return this.lookups[(int)index];
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.lookupOffsets = new ushort[(int)num];
			this.lookups = new Lookup[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.lookupOffsets[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort((ushort)this.lookupOffsets.Length);
			ushort num = 0;
			while ((int)num < this.lookupOffsets.Length)
			{
				Lookup lookup = this.GetLookup(num);
				if (lookup == null)
				{
					writer.WriteUShort(Tags.NULL_TYPE);
				}
				else
				{
					lookup.Write(writer);
				}
				num += 1;
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.lookups = new Lookup[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				ushort num2 = reader.ReadUShort();
				if (num2 != Tags.NULL_TYPE)
				{
					Lookup lookup = new Lookup(base.FontSource, num2);
					lookup.Import(reader);
				}
			}
		}

		ushort[] lookupOffsets;

		Lookup[] lookups;
	}
}
