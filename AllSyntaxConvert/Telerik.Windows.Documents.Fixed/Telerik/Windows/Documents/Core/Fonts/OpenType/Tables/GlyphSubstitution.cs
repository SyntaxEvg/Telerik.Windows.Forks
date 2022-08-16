using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class GlyphSubstitution : TrueTypeTableBase
	{
		public GlyphSubstitution(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.GSUB_TABLE;
			}
		}

		void ReadTable<T>(T table, ushort offset) where T : TableBase
		{
			if (offset == 0)
			{
				return;
			}
			long offset2 = base.Offset + (long)((ulong)offset);
			table.Offset = offset2;
			base.Reader.BeginReadingBlock();
			base.Reader.Seek(offset2, SeekOrigin.Begin);
			table.Read(base.Reader);
			base.Reader.EndReadingBlock();
		}

		void ReadScriptList()
		{
			this.scriptList = new ScriptList(base.FontSource);
			this.ReadTable<ScriptList>(this.scriptList, this.scriptListOffset);
		}

		void ReadFeatureList()
		{
			this.featureList = new FeatureList(base.FontSource);
			this.ReadTable<FeatureList>(this.featureList, this.featureListOffset);
		}

		void ReadLookupList()
		{
			this.lookupList = new LookupList(base.FontSource);
			this.ReadTable<LookupList>(this.lookupList, this.lookupListOffset);
		}

		public Script GetScript(uint tag)
		{
			if (this.scriptList == null)
			{
				this.ReadScriptList();
			}
			return this.scriptList.GetScript(tag);
		}

		public Feature GetFeature(ushort index)
		{
			if (this.featureList == null)
			{
				this.ReadFeatureList();
			}
			return this.featureList.GetFeature((int)index);
		}

		public Lookup GetLookup(ushort index)
		{
			if (this.lookupList == null)
			{
				this.ReadLookupList();
			}
			return this.lookupList.GetLookup(index);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadFixed();
			this.scriptListOffset = reader.ReadUShort();
			this.featureListOffset = reader.ReadUShort();
			this.lookupListOffset = reader.ReadUShort();
		}

		internal override void Write(FontWriter writer)
		{
			this.ReadScriptList();
			this.ReadFeatureList();
			this.ReadLookupList();
			if (this.scriptList == null || this.featureList == null || this.lookupList == null)
			{
				writer.Write(0);
				return;
			}
			writer.Write(1);
			this.scriptList.Write(writer);
			this.featureList.Write(writer);
			this.lookupList.Write(writer);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			byte b = reader.Read();
			if (b > 0)
			{
				this.scriptList = new ScriptList(base.FontSource);
				this.featureList = new FeatureList(base.FontSource);
				this.lookupList = new LookupList(base.FontSource);
				this.scriptList.Import(reader);
				this.featureList.Import(reader);
				this.lookupList.Import(reader);
			}
		}

		ushort scriptListOffset;

		ushort featureListOffset;

		ushort lookupListOffset;

		ScriptList scriptList;

		LookupList lookupList;

		FeatureList featureList;
	}
}
