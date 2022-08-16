using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfBoolOld : PdfSimpleTypeOld<bool>
	{
		public PdfBoolOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PdfBoolOld(PdfContentManager contentManager, bool value)
			: this(contentManager)
		{
			base.Value = value;
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			bool value;
			Helper.UnboxBool(indirectObject.Value, out value);
			base.Value = value;
			base.Load(indirectObject);
		}
	}
}
