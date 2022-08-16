using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	class ToUnicodeCMap : CMapObject
	{
		public ToUnicodeCMap()
		{
		}

		public ToUnicodeCMap(CMapEncoding encoding)
			: base(encoding)
		{
		}

		public void CopyPropertiesFrom(IPdfExportContext context, IEnumerable<CharInfo> usedCharacters)
		{
			foreach (CharInfo charInfo in usedCharacters)
			{
				base.Encoding.AddUnicodeMapping(charInfo.CharCode, charInfo.Unicode);
			}
		}
	}
}
