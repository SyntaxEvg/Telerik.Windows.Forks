using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class StringIndex : Index
	{
		public StringIndex(ICFFFontFile file, long offset)
			: base(file, offset)
		{
		}

		public string this[ushort sid]
		{
			get
			{
				if (StandardStrings.IsStandardString(sid))
				{
					return StandardStrings.GetStandardString(sid);
				}
				sid = (ushort)((int)sid - StandardStrings.StandardStringsCount);
				return this.GetString((int)sid);
			}
		}

		string ReadString(CFFFontReader reader, uint offset, int length)
		{
			reader.BeginReadingBlock();
			long offset2 = base.DataOffset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			string result = reader.ReadString(length);
			reader.EndReadingBlock();
			return result;
		}

		string GetString(int index)
		{
			if (this.strings.Length <= index)
			{
				return ".notdef";
			}
			if (this.strings[index] == null)
			{
				this.strings[index] = this.ReadString(base.Reader, base.Offsets[index], base.GetDataLength(index));
			}
			return this.strings[index];
		}

		public override void Read(CFFFontReader reader)
		{
			base.Read(reader);
			if (base.Count == 0)
			{
				return;
			}
			this.strings = new string[(int)base.Count];
		}

		string[] strings;
	}
}
