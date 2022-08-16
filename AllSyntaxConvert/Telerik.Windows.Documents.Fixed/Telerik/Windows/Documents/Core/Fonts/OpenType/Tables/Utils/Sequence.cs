using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class Sequence
	{
		public ushort[] Subsitutes { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.Subsitutes = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.Subsitutes[i] = reader.ReadUShort();
			}
		}

		internal void Write(FontWriter writer)
		{
			writer.WriteUShort((ushort)this.Subsitutes.Length);
			for (int i = 0; i < this.Subsitutes.Length; i++)
			{
				writer.WriteUShort(this.Subsitutes[i]);
			}
		}

		internal void Import(OpenTypeFontReader reader)
		{
			this.Read(reader);
		}
	}
}
