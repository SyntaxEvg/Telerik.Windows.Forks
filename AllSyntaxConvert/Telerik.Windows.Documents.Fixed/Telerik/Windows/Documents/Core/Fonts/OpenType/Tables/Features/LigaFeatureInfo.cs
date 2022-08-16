using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features
{
	class LigaFeatureInfo : FeatureInfo
	{
		public override uint Tag
		{
			get
			{
				return Tags.FEATURE_STANDARD_LIGATURES;
			}
		}

		public override FeatureInfo.FeatureType Type
		{
			get
			{
				return FeatureInfo.FeatureType.MultipleSubstitution;
			}
		}

		public override bool ShouldApply(GlyphInfo glyphInfo)
		{
			return false;
		}
	}
}
