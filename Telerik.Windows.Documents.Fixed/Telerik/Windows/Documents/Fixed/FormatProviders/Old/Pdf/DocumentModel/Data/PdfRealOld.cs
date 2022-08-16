using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfRealOld : PdfSimpleTypeOld<double>
	{
		public PdfRealOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PdfRealOld(PdfContentManager contentManager, double val)
			: base(contentManager)
		{
			base.Value = val;
			base.IsLoaded = true;
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			double value;
			Helper.UnboxDouble(indirectObject.Value, out value);
			base.Value = value;
			base.Load(indirectObject);
		}
	}
}
