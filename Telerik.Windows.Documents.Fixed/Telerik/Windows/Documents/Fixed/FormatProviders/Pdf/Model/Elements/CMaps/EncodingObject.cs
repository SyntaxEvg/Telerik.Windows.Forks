using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	class EncodingObject : EncodingBaseObject
	{
		public override IEnumerable<CharCode> BuildCharCodes(PdfString s)
		{
			return Enumerable.Empty<CharCode>();
		}
	}
}
