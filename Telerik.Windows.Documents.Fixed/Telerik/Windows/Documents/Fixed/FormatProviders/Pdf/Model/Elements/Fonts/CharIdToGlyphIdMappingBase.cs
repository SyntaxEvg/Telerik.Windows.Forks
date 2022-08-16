using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	abstract class CharIdToGlyphIdMappingBase : PdfStreamObject
	{
		public static CharIdToGlyphIdMappingBase Identity
		{
			get
			{
				return CharIdToGlyphIdMappingBase.identity;
			}
		}

		static readonly CharIdToGlyphIdMappingBase identity = new CharIdToGlyphIdMappingIdentity();
	}
}
