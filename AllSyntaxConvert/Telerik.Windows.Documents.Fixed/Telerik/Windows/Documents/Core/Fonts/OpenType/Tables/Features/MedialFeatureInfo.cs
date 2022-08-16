using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features
{
	class MedialFeatureInfo : FeatureInfo
	{
		public override uint Tag
		{
			get
			{
				return Tags.FEATURE_MEDIAL_FORMS;
			}
		}

		public override FeatureInfo.FeatureType Type
		{
			get
			{
				return FeatureInfo.FeatureType.SingleSubstitution;
			}
		}

		public override bool ShouldApply(GlyphInfo glyphInfo)
		{
			return glyphInfo.Form == GlyphForm.Medial;
		}
	}
}
