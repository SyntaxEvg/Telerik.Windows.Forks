using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeTables
{
	class Format0KerningSubTable : KerningSubTable
	{
		public Format0KerningSubTable(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public override short GetValue(ushort leftGlyphIndex, ushort rightGlyphIndex)
		{
			Dictionary<ushort, short> dictionary;
			if (!this.values.TryGetValue(leftGlyphIndex, out dictionary))
			{
				return 0;
			}
			short result;
			if (!dictionary.TryGetValue(rightGlyphIndex, out result))
			{
				return 0;
			}
			return result;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.values = new Dictionary<ushort, Dictionary<ushort, short>>((int)num);
			reader.ReadUShort();
			reader.ReadUShort();
			reader.ReadUShort();
			for (int i = 0; i < (int)num; i++)
			{
				ushort key = reader.ReadUShort();
				ushort key2 = reader.ReadUShort();
				short value = reader.ReadShort();
				Dictionary<ushort, short> dictionary;
				if (!this.values.TryGetValue(key, out dictionary))
				{
					dictionary = new Dictionary<ushort, short>();
					this.values[key] = dictionary;
				}
				dictionary[key2] = value;
			}
		}

		internal override void Write(FontWriter writer)
		{
			base.Write(writer);
			writer.WriteUShort((ushort)this.values.Count);
			foreach (ushort num in this.values.Keys)
			{
				Dictionary<ushort, short> dictionary = this.values[num];
				writer.WriteUShort(num);
				writer.WriteUShort((ushort)dictionary.Count);
				foreach (ushort num2 in dictionary.Keys)
				{
					writer.WriteUShort(num2);
					writer.WriteShort(dictionary[num2]);
				}
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.values = new Dictionary<ushort, Dictionary<ushort, short>>((int)num);
			for (int i = 0; i < (int)num; i++)
			{
				ushort key = reader.ReadUShort();
				ushort num2 = reader.ReadUShort();
				Dictionary<ushort, short> dictionary = new Dictionary<ushort, short>((int)num2);
				for (int j = 0; j < (int)num2; j++)
				{
					ushort key2 = reader.ReadUShort();
					dictionary[key2] = reader.ReadShort();
				}
				this.values[key] = dictionary;
			}
		}

		Dictionary<ushort, Dictionary<ushort, short>> values;
	}
}
