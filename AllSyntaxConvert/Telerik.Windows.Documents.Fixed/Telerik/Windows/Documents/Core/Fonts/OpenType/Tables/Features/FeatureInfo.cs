using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features
{
	abstract class FeatureInfo
	{
		internal static FeatureInfo CreateFeatureInfo(uint featureTag)
		{
			if (featureTag == Tags.FEATURE_INITIAL_FORMS)
			{
				return new InitFeatureInfo();
			}
			if (featureTag == Tags.FEATURE_TERMINAL_FORMS)
			{
				return new FinalFeatureInfo();
			}
			if (featureTag == Tags.FEATURE_ISOLATED_FORMS)
			{
				return new IsolatedFeatureInfo();
			}
			if (featureTag == Tags.FEATURE_MEDIAL_FORMS)
			{
				return new MedialFeatureInfo();
			}
			if (featureTag == Tags.FEATURE_REQUIRED_LIGATURES)
			{
				return new RLigFeatureInfo();
			}
			if (featureTag == Tags.FEATURE_STANDARD_LIGATURES)
			{
				return new LigaFeatureInfo();
			}
			return null;
		}

		static GlyphsSequence ApplyMultipleSubstitutionLookup(Lookup lookup, GlyphsSequence glyphIDs)
		{
			return lookup.Apply(glyphIDs);
		}

		public abstract uint Tag { get; }

		public abstract FeatureInfo.FeatureType Type { get; }

		public abstract bool ShouldApply(GlyphInfo glyphIndex);

		public GlyphsSequence ApplyLookup(Lookup lookup, GlyphsSequence glyphIDs)
		{
			if (lookup == null)
			{
				return glyphIDs;
			}
			switch (this.Type)
			{
			case FeatureInfo.FeatureType.SingleSubstitution:
				return this.ApplySingleGlyphSubstitutionLookup(lookup, glyphIDs);
			case FeatureInfo.FeatureType.MultipleSubstitution:
				return FeatureInfo.ApplyMultipleSubstitutionLookup(lookup, glyphIDs);
			default:
				return glyphIDs;
			}
		}

		GlyphsSequence ApplySingleGlyphSubstitutionLookup(Lookup lookup, GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = new GlyphsSequence();
			foreach (GlyphInfo glyphInfo in glyphIDs)
			{
				if (this.ShouldApply(glyphInfo))
				{
					glyphsSequence.AddRange(lookup.Apply(new GlyphsSequence(1) { glyphInfo }));
				}
				else
				{
					glyphsSequence.Add(glyphInfo);
				}
			}
			return glyphsSequence;
		}

		internal enum FeatureType
		{
			SingleSubstitution,
			MultipleSubstitution,
			Unsupported
		}
	}
}
