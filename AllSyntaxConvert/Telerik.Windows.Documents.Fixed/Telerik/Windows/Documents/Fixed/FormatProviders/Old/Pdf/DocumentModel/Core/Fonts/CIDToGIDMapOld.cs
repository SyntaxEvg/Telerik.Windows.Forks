using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	class CIDToGIDMapOld : PdfStreamOld, ICidToGidMap
	{
		public CIDToGIDMapOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public byte[] Data { get; set; }

		public override void Load(PdfDataStream stream)
		{
			base.Load(stream);
			this.Data = stream.ReadData(base.ContentManager);
		}

		public ushort GetGlyphId(int cid)
		{
			if (this.Data == null)
			{
				return (ushort)cid;
			}
			return (ushort)BytesHelperOld.ToInt16(this.Data, 2 * cid, 2);
		}
	}
}
