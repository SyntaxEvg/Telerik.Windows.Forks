using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class FeatureList : TableBase
	{
		public FeatureList(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		Feature ReadFeature(OpenTypeFontReader reader, FeatureRecord record)
		{
			FeatureInfo featureInfo = FeatureInfo.CreateFeatureInfo(record.FeatureTag);
			if (featureInfo == null)
			{
				return null;
			}
			reader.BeginReadingBlock();
			reader.Seek(base.Offset + (long)((ulong)record.FeatureOffset), SeekOrigin.Begin);
			Feature feature = new Feature(base.FontSource, featureInfo);
			feature.Read(reader);
			reader.EndReadingBlock();
			return feature;
		}

		public Feature GetFeature(int index)
		{
			if (this.features[index] == null)
			{
				this.features[index] = this.ReadFeature(base.Reader, this.featureRecords[index]);
			}
			return this.features[index];
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.featureRecords = new FeatureRecord[(int)num];
			this.features = new Feature[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.featureRecords[i] = new FeatureRecord();
				this.featureRecords[i].Read(reader);
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort((ushort)this.featureRecords.Length);
			for (int i = 0; i < this.featureRecords.Length; i++)
			{
				Feature feature = this.GetFeature(i);
				if (feature == null)
				{
					writer.WriteULong(Tags.NULL_TAG);
				}
				else
				{
					feature.Write(writer);
				}
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.features = new Feature[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				uint num2 = reader.ReadULong();
				if (num2 != Tags.NULL_TAG)
				{
					Feature feature = new Feature(base.FontSource, FeatureInfo.CreateFeatureInfo(num2));
					feature.Import(reader);
					this.features[i] = feature;
				}
			}
		}

		FeatureRecord[] featureRecords;

		Feature[] features;
	}
}
