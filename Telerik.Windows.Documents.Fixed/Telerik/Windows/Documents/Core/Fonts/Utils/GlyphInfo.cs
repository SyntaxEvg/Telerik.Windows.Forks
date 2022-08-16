using System;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class GlyphInfo
	{
		public GlyphInfo(ushort glyphId)
		{
			this.GlyphId = glyphId;
			this.Form = GlyphForm.Undefined;
		}

		public GlyphInfo(ushort glyphID, GlyphForm form)
		{
			this.GlyphId = glyphID;
			this.Form = form;
		}

		public ushort GlyphId { get; set; }

		public GlyphForm Form { get; set; }

		public override string ToString()
		{
			return string.Format("{0}({1});", this.GlyphId, this.Form);
		}
	}
}
