using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Exceptions;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Charset : CFFTable
	{
		public Charset(ICFFFontFile file, long offset, int count)
			: base(file, offset)
		{
			this.count = count;
		}

		public Charset(ICFFFontFile file, ushort[] glyphs)
			: base(file, -1L)
		{
			this.Initialize(glyphs);
		}

		public ushort this[string name]
		{
			get
			{
				ushort result;
				if (this.nameToGidMapping.TryGetValue(name, out result))
				{
					return result;
				}
				return 0;
			}
		}

		public string this[ushort index]
		{
			get
			{
				return this.names[(int)index];
			}
		}

		public bool TryGetGlyphIdByCharacterName(string name, out ushort glyphId)
		{
			return this.nameToGidMapping.TryGetValue(name, out glyphId);
		}

		public bool TryGetGlyphIdByCharacterIdentifier(ushort cid, out ushort glyphId)
		{
			return this.cidToGidMapping.TryGetValue(cid, out glyphId);
		}

		List<ushort> ReadFormat0(CFFFontReader reader)
		{
			int num = this.count - 1;
			List<ushort> list = new List<ushort>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.ReadSID());
			}
			return list;
		}

		List<ushort> ReadFormat1(CFFFontReader reader)
		{
			int num = this.count - 1;
			List<ushort> list = new List<ushort>(num);
			while (list.Count < num)
			{
				ushort num2 = reader.ReadSID();
				byte b = reader.ReadCard8();
				list.Add(num2);
				for (int i = 0; i < (int)b; i++)
				{
					list.Add((ushort)((int)num2 + i + 1));
				}
			}
			return list;
		}

		List<ushort> ReadFormat2(CFFFontReader reader)
		{
			int num = this.count - 1;
			List<ushort> list = new List<ushort>(num);
			while (list.Count < num)
			{
				ushort num2 = reader.ReadSID();
				ushort num3 = reader.ReadCard16();
				list.Add(num2);
				for (int i = 0; i < (int)num3; i++)
				{
					list.Add((ushort)((int)num2 + i + 1));
				}
			}
			return list;
		}

		void Initialize(ushort[] glyphs)
		{
			this.nameToGidMapping = new Dictionary<string, ushort>(glyphs.Length);
			this.names = new string[glyphs.Length];
			this.cidToGidMapping = new Dictionary<ushort, ushort>(glyphs.Length);
			ushort num = 0;
			while ((int)num < glyphs.Length)
			{
				ushort num2 = glyphs[(int)num];
				string text = base.File.ReadString(num2);
				ushort value = num + 1;
				this.nameToGidMapping[text] = value;
				this.names[(int)num] = text;
				this.cidToGidMapping[num2] = value;
				num += 1;
			}
		}

		public override void Read(CFFFontReader reader)
		{
			byte charsetFormat = reader.ReadCard8();
			List<ushort> list;
			switch (charsetFormat)
			{
			case 0:
				list = this.ReadFormat0(reader);
				break;
			case 1:
				list = this.ReadFormat1(reader);
				break;
			case 2:
				list = this.ReadFormat2(reader);
				break;
			default:
				throw new NotSupportedCharsetFormatException((int)charsetFormat);
			}
			this.Initialize(list.ToArray());
		}

		internal const ushort NotDefIndex = 0;

		readonly int count;

		Dictionary<string, ushort> nameToGidMapping;

		Dictionary<ushort, ushort> cidToGidMapping;

		string[] names;
	}
}
