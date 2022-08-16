using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CMap : TrueTypeTableBase
	{
		static CMap()
		{
			CMap.supportedPlatformIdToEncodingIdCMapTables.Add(new Tuple<ushort, ushort>(3, 1));
			CMap.supportedPlatformIdToEncodingIdCMapTables.Add(new Tuple<ushort, ushort>(3, 0));
		}

		public CMap(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.CMAP_TABLE;
			}
		}

		void Initialize()
		{
			lock (CMap.lockObject)
			{
				if (!this.isInitialized)
				{
					int num = 0;
					while (num < CMap.supportedPlatformIdToEncodingIdCMapTables.Count && this.encoding == null)
					{
						Tuple<ushort, ushort> tuple = CMap.supportedPlatformIdToEncodingIdCMapTables[num];
						this.encoding = this.GetCMapTable(tuple.Item1, tuple.Item2);
						num++;
					}
					this.isInitialized = true;
				}
			}
		}

		CMapTable GetCMapTable(OpenTypeFontReader reader, EncodingRecord record)
		{
			CMapTable cmapTable;
			if (!this.tables.TryGetValue(record, out cmapTable))
			{
				reader.BeginReadingBlock();
				reader.Seek(base.Offset + (long)((ulong)record.Offset), SeekOrigin.Begin);
				cmapTable = CMapTable.ReadCMapTable(reader);
				reader.EndReadingBlock();
				this.tables[record] = cmapTable;
			}
			return cmapTable;
		}

		public virtual CMapTable GetCMapTable(ushort platformId, ushort encodingId)
		{
			if (this.encodings == null)
			{
				return null;
			}
			EncodingRecord encodingRecord = this.encodings.FirstOrDefault((EncodingRecord e) => e.PlatformId == platformId && e.EncodingId == encodingId);
			if (encodingRecord == null)
			{
				return null;
			}
			return this.GetCMapTable(base.Reader, encodingRecord);
		}

		public bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			glyphId = 0;
			this.Initialize();
			return this.encoding != null && this.encoding.TryGetGlyphId(unicode, out glyphId);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			lock (CMap.lockObject)
			{
				reader.ReadUShort();
				ushort num = reader.ReadUShort();
				this.encodings = new EncodingRecord[(int)num];
				this.tables = new Dictionary<EncodingRecord, CMapTable>((int)num);
				for (int i = 0; i < (int)num; i++)
				{
					EncodingRecord encodingRecord = new EncodingRecord();
					encodingRecord.Read(reader);
					this.encodings[i] = encodingRecord;
				}
			}
		}

		internal override void Write(FontWriter writer)
		{
			this.Initialize();
			this.encoding.Write(writer);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.encoding = CMapTable.ImportCMapTable(reader);
			this.isInitialized = true;
		}

		const ushort UNICODE_PLATFORM_ID = 0;

		const ushort WINDOWS_PLATFORM_ID = 3;

		const ushort DEFAULT_SEMANTIC_ID = 0;

		const ushort SYMBOL_ENCODING_ID = 0;

		const ushort UNICODE_ENCODING_ID = 1;

		internal const ushort MISSING_GLYPH_ID = 0;

		static readonly object lockObject = new object();

		static readonly List<Tuple<ushort, ushort>> supportedPlatformIdToEncodingIdCMapTables = new List<Tuple<ushort, ushort>>();

		EncodingRecord[] encodings;

		Dictionary<EncodingRecord, CMapTable> tables;

		bool isInitialized;

		CMapTable encoding;
	}
}
