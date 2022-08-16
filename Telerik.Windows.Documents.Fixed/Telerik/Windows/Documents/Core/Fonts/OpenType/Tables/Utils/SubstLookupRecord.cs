using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class SubstLookupRecord : TableBase
	{
		public SubstLookupRecord(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public ushort SequenceIndex { get; set; }

		public Lookup Lookup
		{
			get
			{
				if (this.lookup == null)
				{
					this.lookup = base.FontSource.GetLookup(this.lookupIndex);
				}
				return this.lookup;
			}
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.SequenceIndex = reader.ReadUShort();
			this.lookupIndex = reader.ReadUShort();
		}

		ushort lookupIndex;

		Lookup lookup;
	}
}
