using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	class EncodingIdentityH : EncodingPredefined
	{
		public EncodingIdentityH()
			: base("Identity-H")
		{
		}

		public override IEnumerable<CharCode> BuildCharCodes(PdfString s)
		{
			for (int i = 0; i < s.Value.Length; i += 2)
			{
				byte[] b = new byte[2];
				if (i + 1 >= s.Value.Length)
				{
					b[0] = 0;
					b[1] = s.Value[i];
				}
				else
				{
					b[0] = s.Value[i];
					b[1] = s.Value[i + 1];
				}
				yield return new CharCode(b);
			}
			yield break;
		}
	}
}
