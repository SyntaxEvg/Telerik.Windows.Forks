using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfIntOld : PdfSimpleTypeOld<int>
	{
		public PdfIntOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PdfIntOld(PdfContentManager contentManager, int value)
			: base(contentManager)
		{
			base.Value = value;
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			int value;
			Helper.UnboxInt(indirectObject.Value, out value);
			base.Value = value;
			base.Load(indirectObject);
		}
	}
}
