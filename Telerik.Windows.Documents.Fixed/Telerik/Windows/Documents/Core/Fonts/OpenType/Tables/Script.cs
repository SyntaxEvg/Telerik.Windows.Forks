using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class Script : TableBase
	{
		public Script(OpenTypeFontSourceBase fontFile, uint scriptTag)
			: base(fontFile)
		{
			this.ScriptTag = scriptTag;
		}

		public uint ScriptTag { get; set; }

		public LangSys DefaultLangSys
		{
			get
			{
				if (this.defaultLangSys == null && this.defaultLangSysOffset != 0)
				{
					this.defaultLangSys = this.ReadLangSys(base.Reader, this.defaultLangSysOffset);
				}
				return this.defaultLangSys;
			}
		}

		LangSys ReadLangSys(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			reader.Seek(base.Offset + (long)((ulong)offset), SeekOrigin.Begin);
			LangSys langSys = new LangSys(base.FontSource);
			langSys.Read(reader);
			reader.EndReadingBlock();
			return langSys;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.defaultLangSysOffset = reader.ReadUShort();
		}

		internal override void Write(FontWriter writer)
		{
			if (this.DefaultLangSys != null)
			{
				writer.WriteULong(this.ScriptTag);
				this.DefaultLangSys.Write(writer);
				return;
			}
			writer.WriteULong(Tags.NULL_TAG);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.defaultLangSys = new LangSys(base.FontSource);
			this.defaultLangSys.Import(reader);
		}

		public override string ToString()
		{
			return Tags.GetStringFromTag(this.ScriptTag);
		}

		ushort defaultLangSysOffset;

		LangSys defaultLangSys;
	}
}
