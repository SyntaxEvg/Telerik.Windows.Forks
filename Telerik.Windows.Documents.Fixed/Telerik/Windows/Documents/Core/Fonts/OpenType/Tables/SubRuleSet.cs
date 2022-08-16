using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class SubRuleSet : TableBase
	{
		public SubRuleSet(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public SubRule[] SubRules
		{
			get
			{
				if (this.subRules == null)
				{
					this.subRules = new SubRule[this.subRuleOffsets.Length];
					for (int i = 0; i < this.subRules.Length; i++)
					{
						this.subRules[i] = this.ReadSubRule(base.Reader, this.subRuleOffsets[i]);
					}
				}
				return this.subRules;
			}
		}

		SubRule ReadSubRule(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			long offset2 = base.Offset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			SubRule subRule = new SubRule(base.FontSource);
			subRule.Read(reader);
			subRule.Offset = offset2;
			reader.EndReadingBlock();
			return subRule;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.subRuleOffsets = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.subRuleOffsets[i] = reader.ReadUShort();
			}
		}

		ushort[] subRuleOffsets;

		SubRule[] subRules;
	}
}
