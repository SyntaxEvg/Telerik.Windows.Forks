using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class NameFormat0 : Name
	{
		public NameFormat0(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public override string FontFamily
		{
			get
			{
				if (this.fontFamily == null)
				{
					this.fontFamily = this.ReadString(base.Reader, 1033, 1);
				}
				return this.fontFamily;
			}
		}

		string ReadString(OpenTypeFontReader reader, ushort languageId, ushort nameId)
		{
			IEnumerable<NameRecord> enumerable = this.FindNameRecords(3, languageId, nameId);
			foreach (NameRecord nameRecord in enumerable)
			{
				Encoding encodingFromEncodingID = IDs.GetEncodingFromEncodingID(nameRecord.EncodingID);
				if (encodingFromEncodingID != null)
				{
					return this.ReadString(reader, nameRecord, encodingFromEncodingID);
				}
			}
			return null;
		}

		string ReadString(OpenTypeFontReader reader, NameRecord record, Encoding encoding)
		{
			string @string;
			if (!this.strings.TryGetValue(record, out @string))
			{
				reader.BeginReadingBlock();
				long offset = base.Offset + (long)((ulong)this.stringOffset) + (long)((ulong)record.Offset);
				reader.Seek(offset, SeekOrigin.Begin);
				byte[] array = new byte[(int)record.Length];
				reader.Read(array, (int)record.Length);
				reader.EndReadingBlock();
				@string = encoding.GetString(array, 0, array.Length);
				this.strings[record] = @string;
			}
			return @string;
		}

		IEnumerable<NameRecord> FindNameRecords(int platformId, ushort languageId, ushort nameId)
		{
			return from r in this.nameRecords
				where (int)r.PlatformID == platformId && r.LanguageID == languageId && r.NameID == nameId
				select r;
		}

		internal override string ReadName(ushort languageID, ushort nameID)
		{
			return this.ReadString(base.Reader, languageID, nameID);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.stringOffset = reader.ReadUShort();
			this.nameRecords = new NameRecord[(int)num];
			this.strings = new Dictionary<NameRecord, string>();
			for (int i = 0; i < (int)num; i++)
			{
				this.nameRecords[i] = new NameRecord();
				this.nameRecords[i].Read(reader);
			}
		}

		ushort stringOffset;

		NameRecord[] nameRecords;

		Dictionary<NameRecord, string> strings;

		string fontFamily;
	}
}
