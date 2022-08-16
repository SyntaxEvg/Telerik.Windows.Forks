using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class Feature : TableBase
	{
		public Feature(OpenTypeFontSourceBase fontFile, FeatureInfo featureInfo)
			: base(fontFile)
		{
			this.FeatureInfo = featureInfo;
		}

		public FeatureInfo FeatureInfo { get; set; }

		public ushort[] LookupsListIndices
		{
			get
			{
				return this.lookupListIndices;
			}
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.lookupListIndices = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.lookupListIndices[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteULong(this.FeatureInfo.Tag);
			writer.WriteUShort((ushort)this.lookupListIndices.Length);
			for (int i = 0; i < this.lookupListIndices.Length; i++)
			{
				writer.WriteUShort(this.lookupListIndices[i]);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.lookupListIndices = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.lookupListIndices[i] = reader.ReadUShort();
			}
		}

		public override string ToString()
		{
			if (this.FeatureInfo != null)
			{
				return Tags.GetStringFromTag(this.FeatureInfo.Tag);
			}
			return "Not supported";
		}

		ushort[] lookupListIndices;
	}
}
