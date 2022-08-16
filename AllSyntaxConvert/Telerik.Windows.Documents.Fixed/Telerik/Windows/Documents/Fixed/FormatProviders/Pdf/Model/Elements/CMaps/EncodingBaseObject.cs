using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	abstract class EncodingBaseObject : CMapObject
	{
		public static EncodingBaseObject IdentityH
		{
			get
			{
				return EncodingBaseObject.identityH;
			}
		}

		public abstract IEnumerable<CharCode> BuildCharCodes(PdfString s);

		static readonly EncodingBaseObject identityH = new EncodingIdentityH();
	}
}
