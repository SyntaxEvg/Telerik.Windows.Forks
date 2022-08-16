using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class TrueTypeGlyphInitializationInfo
	{
		public TrueTypeGlyphInitializationInfo.PlatformType Platform { get; set; }

		public ISimpleEncoding Encoding { get; set; }

		public CMapTable CMap { get; set; }

		public OpenTypeFontSource FontSource { get; set; }

		public byte? AppendingFirstByte { get; set; }

		public enum PlatformType
		{
			MicrosoftNonSymbolic,
			MacintoshNonSymbolic,
			MicrosoftSymbolic,
			MacintoshSymbolic,
			Unknown
		}
	}
}
