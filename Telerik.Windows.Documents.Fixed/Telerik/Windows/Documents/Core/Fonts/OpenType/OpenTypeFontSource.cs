using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeTables;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;
using Telerik.Windows.Documents.Core.Fonts.Type1;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Core.Fonts.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	class OpenTypeFontSource : OpenTypeFontSourceBase
	{
		public OpenTypeFontSource(FontProperties fontProperties, OpenTypeFontReader reader)
			: base(reader)
		{
			this.Offset = 0L;
			this.fontProperties = fontProperties;
			this.Initialize();
		}

		public OpenTypeFontSource(OpenTypeFontReader reader)
			: this(null, reader)
		{
		}

		internal override Outlines GetOutlines()
		{
			if (!this.OffsetTable.HasOpenTypeOutlines)
			{
				return Outlines.TrueType;
			}
			return Outlines.OpenType;
		}

		internal override CMap GetCMap()
		{
			CMap cmap = new CMap(this);
			this.ReadTableData<CMap>(cmap);
			return cmap;
		}

		internal override HorizontalMetrics GetHMtx()
		{
			HorizontalMetrics horizontalMetrics = new HorizontalMetrics(this);
			this.ReadTableData<HorizontalMetrics>(horizontalMetrics);
			return horizontalMetrics;
		}

		internal override Kerning GetKern()
		{
			Kerning kerning = new Kerning(this);
			this.ReadTableData<Kerning>(kerning);
			return kerning;
		}

		internal override GlyphSubstitution GetGSub()
		{
			GlyphSubstitution glyphSubstitution = new GlyphSubstitution(this);
			this.ReadTableData<GlyphSubstitution>(glyphSubstitution);
			return glyphSubstitution;
		}

		internal override Head GetHead()
		{
			Head head = new Head(this);
			this.ReadTableData<Head>(head);
			return head;
		}

		internal override HorizontalHeader GetHHea()
		{
			HorizontalHeader horizontalHeader = new HorizontalHeader(this);
			this.ReadTableData<HorizontalHeader>(horizontalHeader);
			return horizontalHeader;
		}

		internal Post Post
		{
			get
			{
				Post result;
				lock (OpenTypeFontSource.lockObj)
				{
					if (this.post == null)
					{
						lock (OpenTypeFontSource.lockObj)
						{
							this.post = this.ReadTableData<Post>(Tags.POST_TABLE, new ReadTableFormatDelegate<Post>(Post.ReadPostTable));
						}
					}
					result = this.post;
				}
				return result;
			}
		}

		internal Name Name
		{
			get
			{
				Name result;
				lock (OpenTypeFontSource.lockObj)
				{
					if (this.name == null)
					{
						lock (OpenTypeFontSource.lockObj)
						{
							this.name = this.ReadTableData<Name>(Tags.NAME_TABLE, new ReadTableFormatDelegate<Name>(Name.ReadNameTable));
						}
					}
					result = this.name;
				}
				return result;
			}
		}

		internal IndexToLocation Loca
		{
			get
			{
				IndexToLocation result;
				lock (OpenTypeFontSource.lockObj)
				{
					if (this.loca == null)
					{
						lock (OpenTypeFontSource.lockObj)
						{
							this.loca = new IndexToLocation(this);
							this.ReadTableData<IndexToLocation>(this.loca);
						}
					}
					result = this.loca;
				}
				return result;
			}
		}

		internal MaxProfile MaxP
		{
			get
			{
				MaxProfile result;
				lock (OpenTypeFontSource.lockObj)
				{
					if (this.maxp == null)
					{
						lock (OpenTypeFontSource.lockObj)
						{
							this.maxp = new MaxProfile(this);
							this.ReadTableData<MaxProfile>(this.maxp);
						}
					}
					result = this.maxp;
				}
				return result;
			}
		}

		internal Dictionary<uint, TableRecord> Tables
		{
			get
			{
				return this.tables;
			}
		}

		internal OffsetTable OffsetTable { get; set; }

		internal override ushort GetGlyphCount()
		{
			return this.MaxP.NumGlyphs;
		}

		internal long Offset { get; set; }

		public override string GetFontFamily()
		{
			string result;
			if ((result = this.Name.FontFamily) == null)
			{
				if (this.fontProperties != null)
				{
					return this.fontProperties.FontFamilyName;
				}
				result = null;
			}
			return result;
		}

		public override IEnumerable<FontFlag> GetFlags()
		{
			foreach (FontFlag flag in base.GetFlags())
			{
				yield return flag;
			}
			yield return FontFlag.Symbolic;
			yield break;
		}

		public override bool GetIsBold()
		{
			return base.Head.IsBold;
		}

		public override bool GetIsItalic()
		{
			return base.Head.IsItalic;
		}

		public override double GetCapHeight()
		{
			return 0.0;
		}

		public override double GetStemV()
		{
			return 0.0;
		}

		public override double GetItalicAngle()
		{
			return this.Post.ItalicAngle;
		}

		public override double GetUnderlineThickness()
		{
			return base.Scaler.FUnitsToPixels(this.Post.UnderlineThickness, 1000.0);
		}

		public override double GetUnderlinePosition()
		{
			return base.Scaler.FUnitsToPixels(this.Post.UnderlinePosition, 1000.0);
		}

		public override Rect GetBoundingBox()
		{
			return new Rect(base.Scaler.FUnitsToPixels(base.Head.BBox.X, 1000.0), base.Scaler.FUnitsToPixels(base.Head.BBox.Y, 1000.0), base.Scaler.FUnitsToPixels(base.Head.BBox.Width, 1000.0), base.Scaler.FUnitsToPixels(base.Head.BBox.Height, 1000.0));
		}

		public override byte[] GetData()
		{
			return base.Reader.Data;
		}

		internal override CFFFontSource GetCFF()
		{
			if (!this.OffsetTable.HasOpenTypeOutlines)
			{
				return null;
			}
			return this.ReadCFFTable();
		}

		public static OpenTypeFontSourceType GetFontType(OpenTypeFontReader reader)
		{
			OpenTypeFontSourceType result = OpenTypeFontSourceType.Unknown;
			reader.BeginReadingBlock();
			uint num = reader.ReadULong();
			reader.EndReadingBlock();
			reader.BeginReadingBlock();
			double num2 = (double)reader.ReadFixed();
			reader.EndReadingBlock();
			if (num == Tags.TRUE_TYPE_COLLECTION)
			{
				result = OpenTypeFontSourceType.TrueTypeCollection;
			}
			else if (num2 == 1.0)
			{
				result = OpenTypeFontSourceType.TrueType;
			}
			return result;
		}

		public override bool TryGetCharCode(int unicode, out int charCode)
		{
			bool result = false;
			charCode = 0;
			ushort glyphId;
			if (this.TryGetGlyphId(unicode, out glyphId))
			{
				ushort num = 0;
				CMapTable cmapTable = base.CMap.GetCMapTable(3, 0);
				if (cmapTable != null)
				{
					result = cmapTable.TryGetCharId(glyphId, out num);
				}
				cmapTable = base.CMap.GetCMapTable(1, 0);
				if (cmapTable != null)
				{
					result = cmapTable.TryGetCharId(glyphId, out num);
				}
				charCode = (int)num;
				return result;
			}
			return false;
		}

		public override double GetAdvancedWidthOverride(int glyphId)
		{
			return base.Scaler.GetAdvancedWidth((ushort)glyphId, 1000.0);
		}

		internal override GlyphData GetGlyphData(ushort glyphIndex)
		{
			GlyphData glyphData;
			if (!this.glyphsData.TryGetValue(glyphIndex, out glyphData))
			{
				long num = this.Loca.GetOffset(glyphIndex);
				TableRecord tableRecord;
				this.tables.TryGetValue(Tags.GLYF_TABLE, out tableRecord);
				if (num == -1L || tableRecord == null || num >= (long)((ulong)(tableRecord.Offset + tableRecord.Length)))
				{
					return new GlyphData(this, glyphIndex);
				}
				num += (long)((ulong)tableRecord.Offset);
				base.Reader.BeginReadingBlock();
				base.Reader.Seek(num, SeekOrigin.Begin);
				glyphData = GlyphData.ReadGlyf(this, glyphIndex);
				base.Reader.EndReadingBlock();
				this.glyphsData[glyphIndex] = glyphData;
			}
			return glyphData;
		}

		void ReadTableData<T>(T table) where T : TrueTypeTableBase
		{
			TableRecord tableRecord;
			if (!this.tables.TryGetValue(table.Tag, out tableRecord))
			{
				return;
			}
			long offset = this.Offset + (long)((ulong)tableRecord.Offset);
			base.Reader.BeginReadingBlock();
			base.Reader.Seek(offset, SeekOrigin.Begin);
			table.Read(base.Reader);
			table.Offset = offset;
			base.Reader.EndReadingBlock();
		}

		T ReadTableData<T>(uint tag, ReadTableFormatDelegate<T> readTableDelegate) where T : TrueTypeTableBase
		{
			TableRecord tableRecord;
			if (!this.tables.TryGetValue(tag, out tableRecord))
			{
				return default(T);
			}
			base.Reader.BeginReadingBlock();
			long offset = this.Offset + (long)((ulong)tableRecord.Offset);
			base.Reader.Seek(offset, SeekOrigin.Begin);
			T result = readTableDelegate(this, base.Reader);
			result.Offset = offset;
			base.Reader.EndReadingBlock();
			return result;
		}

		int GetTableLength(uint tag)
		{
			TableRecord tableRecord = this.tables[tag];
			int num = (int)(base.Reader.Length - (long)((ulong)tableRecord.Offset));
			foreach (TableRecord tableRecord2 in this.tables.Values)
			{
				int num2 = (int)(tableRecord2.Offset - tableRecord.Offset);
				if (num2 > 0 && num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		CFFFontSource ReadCFFTable()
		{
			int tableLength = this.GetTableLength(Tags.CFF_TABLE);
			byte[] array = new byte[tableLength];
			base.Reader.BeginReadingBlock();
			long offset = this.Offset + (long)((ulong)this.tables[Tags.CFF_TABLE].Offset);
			base.Reader.Seek(offset, SeekOrigin.Begin);
			base.Reader.Read(array, tableLength);
			base.Reader.EndReadingBlock();
			CFFFontFile cfffontFile = new CFFFontFile(array);
			return cfffontFile.FontSource;
		}

		void ReadTableRecords()
		{
			this.tables = new Dictionary<uint, TableRecord>();
			for (int i = 0; i < (int)this.OffsetTable.NumTables; i++)
			{
				TableRecord tableRecord = new TableRecord();
				tableRecord.Read(base.Reader);
				this.tables[tableRecord.Tag] = tableRecord;
			}
		}

		void Initialize()
		{
			this.OffsetTable = new OffsetTable();
			this.OffsetTable.Read(base.Reader);
			this.ReadTableRecords();
			this.glyphsData = new Dictionary<ushort, GlyphData>();
		}

		static object lockObj = new object();

		readonly FontProperties fontProperties;

		Dictionary<ushort, GlyphData> glyphsData;

		Dictionary<uint, TableRecord> tables;

		IndexToLocation loca;

		MaxProfile maxp;

		Name name;

		Post post;
	}
}
