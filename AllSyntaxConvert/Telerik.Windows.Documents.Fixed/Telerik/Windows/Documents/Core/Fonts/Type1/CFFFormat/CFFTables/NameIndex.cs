using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class NameIndex : Index
	{
		public NameIndex(ICFFFontFile file, long offset)
			: base(file, offset)
		{
		}

		public string this[ushort sid]
		{
			get
			{
				return this.GetString((int)sid);
			}
		}

		string ReadString(CFFFontReader reader, uint offset, int length)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.DataOffset + (long)((ulong)offset), SeekOrigin.Begin);
			string result = reader.ReadString(length);
			reader.EndReadingBlock();
			return result;
		}

		string GetString(int index)
		{
			if (this.names[index] == null)
			{
				this.names[index] = this.ReadString(base.Reader, base.Offsets[index], base.GetDataLength(index));
			}
			return this.names[index];
		}

		public override void Read(CFFFontReader reader)
		{
			base.Read(reader);
			if (base.Count == 0)
			{
				return;
			}
			this.names = new string[(int)base.Count];
		}

		string[] names;
	}
}
