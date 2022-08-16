using System;

namespace BitMiracle.LibTiff.Classic
{
	class TiffFieldInfo
	{
		public TiffFieldInfo(TiffTag tag, short readCount, short writeCount, TiffType type, short bit, bool okToChange, bool passCount, string name)
		{
			this.m_tag = tag;
			this.m_readCount = readCount;
			this.m_writeCount = writeCount;
			this.m_type = type;
			this.m_bit = bit;
			this.m_okToChange = okToChange;
			this.m_passCount = passCount;
			this.m_name = name;
		}

		public override string ToString()
		{
			if (this.m_bit != 65 || this.m_name.Length == 0)
			{
				return this.m_tag.ToString();
			}
			return this.m_name;
		}

		public TiffTag Tag
		{
			get
			{
				return this.m_tag;
			}
		}

		public short ReadCount
		{
			get
			{
				return this.m_readCount;
			}
		}

		public short WriteCount
		{
			get
			{
				return this.m_writeCount;
			}
		}

		public TiffType Type
		{
			get
			{
				return this.m_type;
			}
		}

		public short Bit
		{
			get
			{
				return this.m_bit;
			}
		}

		public bool OkToChange
		{
			get
			{
				return this.m_okToChange;
			}
		}

		public bool PassCount
		{
			get
			{
				return this.m_passCount;
			}
		}

		public string Name
		{
			get
			{
				return this.m_name;
			}
			internal set
			{
				this.m_name = value;
			}
		}

		public const short Variable = -1;

		public const short Spp = -2;

		public const short Variable2 = -3;

		TiffTag m_tag;

		short m_readCount;

		short m_writeCount;

		TiffType m_type;

		short m_bit;

		bool m_okToChange;

		bool m_passCount;

		string m_name;
	}
}
