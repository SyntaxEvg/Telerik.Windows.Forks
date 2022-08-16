using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts.Cid
{
	class CidSet : PdfStreamObject
	{
		internal void CopyPropertiesFrom(IPdfExportContext context, FontBase font)
		{
			HashSet<int> hashSet = new HashSet<int>(from g in context.GetUsedCharacters(font)
				select g.CharCode.Code);
			List<byte> list = new List<byte>();
			if (hashSet.Count > 0)
			{
				int num = hashSet.Max();
				BitsWriter bitsWriter = new BitsWriter();
				for (int i = 0; i < num; i += 8)
				{
					bitsWriter.Clear();
					for (int j = 0; j < 8; j++)
					{
						byte value = ((byte)(hashSet.Contains(i + j) ? 1 : 0));
						bitsWriter.WriteBits(value, 1);
					}
					list.Add(bitsWriter.Data);
				}
			}
			base.Data = list.ToArray();
		}
	}
}
