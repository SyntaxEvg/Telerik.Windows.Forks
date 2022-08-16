using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class ScriptList : TableBase
	{
		public ScriptList(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		Script ReadScript(OpenTypeFontReader reader, ScriptRecord record)
		{
			reader.BeginReadingBlock();
			long offset = base.Offset + (long)((ulong)record.ScriptOffset);
			reader.Seek(offset, SeekOrigin.Begin);
			Script script = new Script(base.FontSource, record.ScriptTag);
			script.Offset = offset;
			script.Read(reader);
			reader.EndReadingBlock();
			return script;
		}

		public Script GetScript(uint tag)
		{
			Script script;
			if (!this.scripts.TryGetValue(tag, out script) && this.scriptRecords != null)
			{
				ScriptRecord record;
				if (this.scriptRecords.TryGetValue(tag, out record) || this.scriptRecords.TryGetValue(Tags.DEFAULT_TABLE_SCRIPT_TAG, out record))
				{
					script = this.ReadScript(base.Reader, record);
				}
				else
				{
					script = null;
				}
				this.scripts[tag] = script;
			}
			return script;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.scriptRecords = new Dictionary<uint, ScriptRecord>((int)num);
			this.scripts = new Dictionary<uint, Script>((int)num);
			for (int i = 0; i < (int)num; i++)
			{
				ScriptRecord scriptRecord = new ScriptRecord();
				scriptRecord.Read(reader);
				this.scriptRecords[scriptRecord.ScriptTag] = scriptRecord;
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort((ushort)this.scriptRecords.Count);
			foreach (uint key in this.scriptRecords.Keys)
			{
				Script script = this.ReadScript(base.Reader, this.scriptRecords[key]);
				script.Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.scripts = new Dictionary<uint, Script>((int)num);
			for (int i = 0; i < (int)num; i++)
			{
				uint num2 = reader.ReadULong();
				if (num2 != Tags.NULL_TAG)
				{
					Script script = new Script(base.FontSource, num2);
					script.Import(reader);
					this.scripts[num2] = script;
				}
			}
		}

		Dictionary<uint, ScriptRecord> scriptRecords;

		Dictionary<uint, Script> scripts;
	}
}
