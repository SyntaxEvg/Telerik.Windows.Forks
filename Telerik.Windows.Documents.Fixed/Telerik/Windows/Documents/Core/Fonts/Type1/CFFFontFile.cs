using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables;

namespace Telerik.Windows.Documents.Core.Fonts.Type1
{
	class CFFFontFile : ICFFFontFile
	{
		public CFFFontFile(byte[] data)
		{
			this.reader = new CFFFontReader(data);
			this.Initialize();
		}

		public CFFFontSource FontSource
		{
			get
			{
				return this.fontSource;
			}
		}

		public Header Header { get; set; }

		public NameIndex Name { get; set; }

		public TopIndex TopIndex { get; set; }

		public StringIndex StringIndex { get; set; }

		public SubrsIndex GlobalSubrs { get; set; }

		public CFFFontReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public string ReadString(ushort sid)
		{
			return this.StringIndex[sid];
		}

		public void ReadTable(CFFTable table)
		{
			this.Reader.BeginReadingBlock();
			this.Reader.Seek(table.Offset, SeekOrigin.Begin);
			table.Read(this.Reader);
			this.Reader.EndReadingBlock();
		}

		void Initialize()
		{
			this.Header = new Header(this);
			this.ReadTable(this.Header);
			this.Name = new NameIndex(this, (long)((ulong)this.Header.HeaderSize));
			this.ReadTable(this.Name);
			this.TopIndex = new TopIndex(this, this.Name.SkipOffset);
			this.ReadTable(this.TopIndex);
			this.StringIndex = new StringIndex(this, this.TopIndex.SkipOffset);
			this.ReadTable(this.StringIndex);
			Top top = this.TopIndex[0];
			this.GlobalSubrs = new SubrsIndex(this, top.CharstringType, this.StringIndex.SkipOffset);
			this.ReadTable(this.GlobalSubrs);
			this.fontSource = new CFFFontSource(this, top);
		}

		readonly CFFFontReader reader;

		CFFFontSource fontSource;
	}
}
