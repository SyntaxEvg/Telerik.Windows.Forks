using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class LangSys : TableBase
	{
		public LangSys(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		IEnumerable<Tuple<FeatureInfo, Lookup>> Lookups
		{
			get
			{
				if (this.lookups == null)
				{
					IEnumerable<Tuple<FeatureInfo, ushort>> lookupIndices = this.GetLookupIndices();
					this.lookups = new List<Tuple<FeatureInfo, Lookup>>(lookupIndices.Count<Tuple<FeatureInfo, ushort>>());
					foreach (Tuple<FeatureInfo, ushort> tuple in lookupIndices)
					{
						this.lookups.Add(new Tuple<FeatureInfo, Lookup>(tuple.Item1, base.FontSource.GetLookup(tuple.Item2)));
					}
				}
				return this.lookups;
			}
		}

		int Compare(Tuple<FeatureInfo, ushort> left, Tuple<FeatureInfo, ushort> right)
		{
			return left.Item2.CompareTo(right.Item2);
		}

		IEnumerable<Feature> GetFeatures()
		{
			List<Feature> list = new List<Feature>(this.featureIndices.Length);
			for (int i = 0; i < this.featureIndices.Length; i++)
			{
				Feature feature = base.FontSource.GetFeature(this.featureIndices[i]);
				if (feature != null)
				{
					list.Add(feature);
				}
			}
			if (this.reqFeatureIndex != 65535)
			{
				list.Add(base.FontSource.GetFeature(this.reqFeatureIndex));
			}
			return list;
		}

		IEnumerable<Tuple<FeatureInfo, ushort>> GetLookupIndices()
		{
			List<Tuple<FeatureInfo, ushort>> list = new List<Tuple<FeatureInfo, ushort>>();
			foreach (Feature feature in this.GetFeatures())
			{
				foreach (ushort item in feature.LookupsListIndices)
				{
					list.Add(new Tuple<FeatureInfo, ushort>(feature.FeatureInfo, item));
				}
			}
			list.Sort(new Comparison<Tuple<FeatureInfo, ushort>>(this.Compare));
			return list;
		}

		public GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = glyphIDs;
			foreach (Tuple<FeatureInfo, Lookup> tuple in this.Lookups)
			{
				glyphsSequence = tuple.Item1.ApplyLookup(tuple.Item2, glyphsSequence);
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			this.reqFeatureIndex = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.featureIndices = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.featureIndices[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(this.reqFeatureIndex);
			writer.WriteUShort((ushort)this.featureIndices.Length);
			for (int i = 0; i < this.featureIndices.Length; i++)
			{
				writer.WriteUShort(this.featureIndices[i]);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.reqFeatureIndex = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.featureIndices = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.featureIndices[i] = reader.ReadUShort();
			}
		}

		ushort reqFeatureIndex;

		ushort[] featureIndices;

		List<Tuple<FeatureInfo, Lookup>> lookups;
	}
}
