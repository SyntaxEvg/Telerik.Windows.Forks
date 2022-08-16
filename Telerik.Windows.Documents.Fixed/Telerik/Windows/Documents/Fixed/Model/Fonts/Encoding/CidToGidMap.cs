using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class CidToGidMap : ICidToGidMap
	{
		public CidToGidMap()
		{
			this.data = null;
		}

		public CidToGidMap(byte[] data)
		{
			this.data = data;
		}

		public bool IsIdentityMapping
		{
			get
			{
				return this.data == null;
			}
		}

		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		public ushort GetGlyphId(int charCode)
		{
			if (this.IsIdentityMapping)
			{
				return (ushort)charCode;
			}
			return BitConverter.ToUInt16(this.data, 2 * charCode);
		}

		readonly byte[] data;
	}
}
