using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class HorizontalMetrics : TrueTypeTableBase
	{
		public HorizontalMetrics(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.HMTX_TABLE;
			}
		}

		public ushort GetAdvancedWidth(int glyphID)
		{
			if (glyphID < this.hMetrics.Length)
			{
				return this.hMetrics[glyphID].AdvanceWidth;
			}
			LongHorMetric longHorMetric = this.hMetrics[this.hMetrics.Length - 1];
			return longHorMetric.AdvanceWidth;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.hMetrics = new LongHorMetric[(int)base.FontSource.NumberOfHorizontalMetrics];
			for (int i = 0; i < this.hMetrics.Length; i++)
			{
				LongHorMetric longHorMetric = new LongHorMetric();
				longHorMetric.Read(reader);
				this.hMetrics[i] = longHorMetric;
			}
		}

		internal override void Write(FontWriter writer)
		{
			for (int i = 0; i < this.hMetrics.Length; i++)
			{
				this.hMetrics[i].Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.Read(reader);
		}

		LongHorMetric[] hMetrics;
	}
}
