using System;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Features
{
	class IsolatedFeatureInfo : FeatureInfo
	{
		public override uint Tag
		{
			get
			{
				return Tags.FEATURE_ISOLATED_FORMS;
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
			return glyphInfo.Form == GlyphForm.Isolated;
		}
	}
}
